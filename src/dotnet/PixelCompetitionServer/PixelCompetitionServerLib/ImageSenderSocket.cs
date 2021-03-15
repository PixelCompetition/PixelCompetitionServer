using System;
using System.IO.Pipes;

namespace PixelCompetitionServerLib
{
	/*
	 * Included Timers rely on Component-Model or Task-Model resulting in to slow frame rates.
	 * Implementing this as as Task would make each client handling having the same priority.
	 * This might cause skipping frames, so I decided to implement it as a Thread.
	 */
	public class ImageSenderSocket : ImageSender
	{
		private NamedPipeServerStream _client;

		public override bool createPipe()
		{
			try
			{
				_client = new NamedPipeServerStream(
					PipeName,
					PipeDirection.Out,
					1,
					PipeTransmissionMode.Byte,
					PipeOptions.WriteThrough);
				_client.WaitForConnection();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public override bool writeToPipe()
		{
			if (!_client.IsConnected) return false;
			try
			{
				_client.Write(Buffer);
				_client.Flush();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public override void closePipe()
		{
			_client.Close();
		}

		public ImageSenderSocket(PixelCompetitionServerConfig config, byte[] buffer, string pipeName) : base(config, buffer, pipeName)
		{
		}
	}
}