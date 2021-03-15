using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelCompetitionServerLib
{
	public class ResponseWriter:IDisposable
	{
		private int _charCounter = 64;
		private const string NewLine = "\r\n";
		readonly List<string> _lines = new List<string>(); // Handler is not multi threaded to no BlockingCollection needed.
		private Stream _stream;
		private StreamWriter _writer;
		public ResponseWriter(Stream stream)
		{
			if (!stream.CanWrite)
				throw new Exception("Writer not writable.");
			_stream = stream;
			_writer = new StreamWriter(stream)
			{
				AutoFlush = true
			};

		}

		public void writeLine()
		{
			_lines.Add(String.Empty);
			_charCounter += 2;
		}

		public void writeLine(string line)
		{
			_lines.AddRange(line.Split("\n"));
			_charCounter += 2 + line.Length;
		}


		public async Task flush()
		{
			try
			{
				if (!_stream.CanWrite) return;
				CancellationTokenSource tokenSource = new CancellationTokenSource(50000);
				await flush(tokenSource.Token);
			}
			catch
			{
				// ignore
			}
		}
		public async Task flush(CancellationToken token)
		{
			try
			{
                if (_lines.Count == 0) return;
				if (!_stream.CanWrite) return;

                StringBuilder s; 
                lock (this)
                {
                    if (_lines.Count == 0) return;
                    _lines.Add(NewLine);
                    s = new StringBuilder(_charCounter);
                    s.AppendJoin(NewLine, _lines);
                    _lines.Clear();
					_charCounter = 64;
				}
                await _writer.WriteAsync(s, token);
			}
			catch
			{
				//ignore
			}
		}

		// ReSharper disable once InconsistentNaming
		public void Dispose()
		{
			_stream = null;
			_writer = null;
		}
	}
}
