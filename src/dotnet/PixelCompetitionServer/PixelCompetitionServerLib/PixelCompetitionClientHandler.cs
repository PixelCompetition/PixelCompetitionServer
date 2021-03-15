using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PixelCompetitionServerLib
{
    /// <summary>
    /// Main class to handle a client session against the server, it is initialized by the server on demand
    /// and uses ICompetitionFactories to instantiate an ICompetition for each client.
    /// </summary>
	public class PixelCompetitionClientHandler : IDisposable
	{
		// Internal
		private CancellationTokenSource _clientCancellationTokenSource = new CancellationTokenSource();
		string _stopMessage;
		readonly byte[] _buffer = new byte[1024];
		string _preFix = "";


		readonly char[] _whiteSpaces = new char[] { ' ', '\t' };

		// Given
		private TcpClient _client;
		private PixelCompetitionServer _server;

		// Generated
		private ICompetition[] _competitions;
		NetworkStream _stream;
		ResponseWriter _writer;

		public PixelCompetitionClientHandler(TcpClient client, PixelCompetitionServer server)
		{
			_client = client;
			_server = server;
		}

		private async Task init()
		{
			_stream = _client.GetStream();
			_writer = new ResponseWriter(_stream);
			await Task.Run(
				() =>
				{
					List<ICompetition> competitions = new List<ICompetition>();
					foreach (var serverCurrentFactory in _server.CurrentFactories)
					{
						competitions.Add(
							serverCurrentFactory.generateCompetition(_server.CurrentCanvas, _writer)
						);
					}
					_competitions = competitions.ToArray();
				});
		}

		private void welcome()
		{
			_writer.writeLine("Welcome to PixelCompetition");
			_writer.writeLine(_server.Config.InstanceWelcome);

			foreach (var configCurrentFactory in _server.CurrentFactories)
			{
				_writer.writeLine(configCurrentFactory.getDefaultLanguage().Welcome);
			}
            
            _writer.writeLine($"Competitions {_server.CurrentCompetitionNumbersString}");
            _writer.writeLine("Send 'help' for getting help.");
			_writer.writeLine($"Canvas Width {_server.CurrentCanvas.Width}");
			_writer.writeLine($"Canvas Height {_server.CurrentCanvas.Height}");
		}

		public void processLine(string line)
		{
			if (string.IsNullOrWhiteSpace(value: line)) return;
			line = line.Trim();
			var tokens = line.Split(separator: _whiteSpaces,
				options: StringSplitOptions.RemoveEmptyEntries);
			try
			{
				ICompetition.ProcessState processState = new ICompetition.ProcessState();
				bool handled = false;
				foreach (var competition in _competitions)
				{
					handled = competition.processInput(tokens, processState);
					if (handled) break;
				}

				if (processState.HasError)
				{
					_writer.writeLine("Last command was ignored:");
					foreach (var error in processState.Errors)
					{
						_writer.writeLine("Error:");
						_writer.writeLine($"Found token {error.Value}");
						_writer.writeLine(error.Message);
					}
					return;
				}

				if (!handled)
				{
					_writer.writeLine($"Last command was ignored: {line}");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(value: e);
			}
		}

		public async Task handleAsync()
		{
			//await Task.Yield();
			await Task.Run(async() =>
			{
				try
				{
					await init();
					welcome();
					_clientCancellationTokenSource.CancelAfter(_server.Config.ClientTimeOut);
					await _writer.flush(_clientCancellationTokenSource.Token);
					int max = _buffer.Length;
					while (_client.Connected)
					{
						_clientCancellationTokenSource.CancelAfter(_server.Config.ClientTimeOut);
						if (_clientCancellationTokenSource.IsCancellationRequested) break;

						int count = 0;
						try
						{
							count = await _stream.ReadAsync(_buffer, 0, max, _clientCancellationTokenSource.Token);
                            // TCP, if this returns with 0, the client has closed the connection
						}
						catch
						{
							// ignore
							// Should not break here because of processing Prefix
							//Console.WriteLine(ex);
						}

						_clientCancellationTokenSource.CancelAfter(_server.Config.ClientTimeOut);

						string buf = _preFix + System.Text.Encoding.UTF8.GetString(_buffer, 0, count);
						if (buf.Length == 0)
						{
							continue;
						}

						var lines = buf.Split("\n");

						_preFix = count == 0 ? "" : lines[^1];
                        int length = count == 0 ? lines.Length : lines.Length - 1;

						for (int i = 0; i < length; i++)
						{
							processLine(lines[i]);
						}

						_clientCancellationTokenSource.CancelAfter(_server.Config.ClientTimeOut);
						if (lines.Length > 1)
							await _writer.flush(_clientCancellationTokenSource.Token);

						if (!_client.Connected && _client.Available == 0)
						{
							processLine(_preFix);
							break;
						}

						if (_stopMessage != null || count == 0) // Here we need the break, since Client.Connect is only the local state
							break;
					}

					await close();
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					Console.WriteLine("Connection closed.");
				}
			});
			await close();
			Console.WriteLine("Task closed.");
			await close();
		}

		public async Task close()
		{
			try
			{
				_server?.removeClientHandler(this);
				_server = null;

				if (_stopMessage != null)
				{
					_writer?.writeLine(_stopMessage);
				}

				if (_client != null && _client.Connected)
					if (_writer != null)
						await _writer?.flush();
			}
			catch
			{
				// ignore
			}
		}

		public void stop(String message)
		{
			_stopMessage = message;
			_clientCancellationTokenSource.Cancel();
		}

		~PixelCompetitionClientHandler()
		{
			Dispose(true);
		}

		private void releaseUnmanagedResources()
		{
			// TODO release unmanaged resources here
		}

		// ReSharper disable once InconsistentNaming
		private void Dispose(bool disposing)
		{
			releaseUnmanagedResources();
			if (disposing)
			{
				_clientCancellationTokenSource?.Dispose();
				_clientCancellationTokenSource = null;
				_client?.Dispose();
				_client = null;
				_stream?.Dispose();
				_stream = null;
				_writer?.Dispose();
				_writer = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public bool stillAlive()
		{
			if (_client == null) return false;
			if (_client.Connected) return true;
			if (_server == null) return false;
			if (_client.Available > 0) return true;
			return false;
		}

	}
}
