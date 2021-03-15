using System;
using System.Threading;

namespace PixelCompetitionServerLib
{
	public abstract class ImageSender 
	{
		protected PixelCompetitionServerConfig Config;
		protected byte[] Buffer;
		protected string PipeName;
		protected long Step;
		protected long Next;

		public Thread startThread()
		{
			var res = new Thread(this.start) {IsBackground = true};
				res.Start();
				return res;
		}

		public abstract bool createPipe();
		public abstract bool writeToPipe();
		public abstract void closePipe();

		private void start()
		{
			while (true)
			{
				Console.WriteLine($"Try to connect to {PipeName} ...");
				if (!createPipe())
				{
					throw new Exception($"Could not connect to {PipeName}");
				}
				Console.WriteLine("Connected");
				Step = 1000 / Config.Framerate;
				Next = DateTime.Now.Ticks + Step;

				while (true)
				{
					long nowTicks = DateTime.Now.Ticks;
					while (nowTicks < Next)
					{
						Thread.Sleep((int) (Next - nowTicks));
						nowTicks = DateTime.Now.Ticks;
					}

					while (DateTime.Now.Ticks > Next) Next += Step;
					if (!writeToPipe()) break;
				}
				closePipe();
			}	
		}
		protected ImageSender(PixelCompetitionServerConfig config, byte[] buffer, string pipeName)
		{
			Config = config;
			Buffer = buffer;
			PipeName = pipeName;
		}

		~ImageSender()
		{
			closePipe();
		}
	}
}