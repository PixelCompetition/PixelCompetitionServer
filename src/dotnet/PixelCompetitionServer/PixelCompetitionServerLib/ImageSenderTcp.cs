using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace PixelCompetitionServerLib
{
	public class ImageSenderTcp: ImageSender
	{
		private Stream _stream;
		private TcpClient _client;
		
		public override bool createPipe()
		{
			lock (this)
			{
				if (_client == null)
				{
					_client = new TcpClient(Config.RawVideoTcpHost, Config.RawVideoTcpPort);

					int count = 0;
					while (!_client.Connected && count < 500)
					{
						Thread.Sleep(10);
						count++;
					}

					if (count < 500)
						Console.WriteLine("Connected");
					else
						return false;

					_stream = _client.GetStream();
				}
			}
			return true;
		}

		public override bool writeToPipe()
		{
			if (_client != null && _stream != null && _client.Connected && _stream.CanWrite)
			{
				try
				{
					_stream.Write(Buffer);
					_stream.Flush();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			}
			else
			{
				return false;
			}
			return true;
		}

		public override void closePipe()
		{
			_stream?.Close();
			_stream = null;
			_client?.Close();
			_client = null;
		}

		public ImageSenderTcp(PixelCompetitionServerConfig config, byte[] buffer) : base(config, buffer, null)
		{
			PipeName = "tcp://" + config.RawVideoTcpHost + ":" + config.RawVideoTcpPort;
		}
	}
}