using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PixelCompetitionServerLib.Common;

namespace PixelCompetitionServerLib
{
	public class PixelCompetitionServer
	{
		private TcpListener _server;
		public PixelCompetitionServerConfig Config { get; }

		private readonly List<ICompetitionFactory> _currentFactories = new List<ICompetitionFactory>();

		public IEnumerable<ICompetitionFactory> CurrentFactories => _currentFactories;
        public string CurrentCompetitionNumbersString { get; private set; }
        public ICanvas CurrentCanvas { get; }

		readonly ConcurrentBag<PixelCompetitionClientHandler> _clients = new ConcurrentBag<PixelCompetitionClientHandler>();

		public void startCompetition(IEnumerable<ICompetitionFactory> factories)
		{
			var clientsToClose = _clients.ToArray();
			_clients.Clear();
			_currentFactories.Clear();
			var factoryArray = factories.ToArray();
            CurrentCompetitionNumbersString = string.Join(" ", factoryArray.Select(f => f.Number));

            _currentFactories.Add(new CommonCompetitionFactory(factoryArray));
			_currentFactories.AddRange(factoryArray);
			_currentFactories.ForEach(f=>f.assignServer(this));

			foreach (var pixelCompetitionClientHandler in clientsToClose)
			{
				pixelCompetitionClientHandler?.stop("We are starting a new Competition.");
			}
		}


		public static PixelCompetitionServer startServer(
			PixelCompetitionServerConfig config, ICanvas canvas)
		{
			PixelCompetitionServer server = new PixelCompetitionServer(config, canvas);
			new Thread(server.startListener) { IsBackground = true }.Start();
			return server;
		}


		PixelCompetitionServer(PixelCompetitionServerConfig config, ICanvas canvas)
		{
			Config = config;
			CurrentCanvas = canvas;
		}
		public async void startListener()
		{
			while (true)
			{
				// TODO Would be nice to bind only to given addresses, seems the class is not able to do so.
				_server = new TcpListener(IPAddress.Any, Config.LocalPort);
				_server.Start();
				try
				{
					while (true)
					{
						Console.WriteLine("Waiting for a connection...");

						TcpClient client = await _server.AcceptTcpClientAsync().ConfigureAwait(false);
						if (client == null) continue; // Should not be done!
						PixelCompetitionClientHandler handler = new PixelCompetitionClientHandler(client, this);

						_clients.Add(handler);
						Console.WriteLine(_clients.Count);

						// ReSharper disable once AssignmentIsFullyDiscarded
						_ = handler.handleAsync();
						Console.WriteLine("Connected!");
					}
				}
				catch (SocketException e)
				{
					Console.WriteLine("Restart server");
					Console.WriteLine("SocketException: {0}", e);
					_server.Stop();
				}
			}
		}

		public void removeClientHandler(PixelCompetitionClientHandler client)
		{
			if (client == null) return;
			Console.WriteLine($"Number of clients is {_clients.Count}.");
			_clients.TryTake(out client);
		}

		public void cleanUp()
		{
			Console.WriteLine("Clean Up ...");
			var handlers = _clients.Where(c => !c.stillAlive()).ToArray();

			Console.WriteLine($"Found {handlers.Length} dead clients.");
			foreach (var pixelCompetitionClientHandler in handlers)
			{
				removeClientHandler(pixelCompetitionClientHandler);
			}
		}
	}
}
