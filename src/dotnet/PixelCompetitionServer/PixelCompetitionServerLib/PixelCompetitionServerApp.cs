using System;
using System.IO;
using System.Threading;

namespace PixelCompetitionServerLib
{
	public class PixelCompetitionServerApp
	{
		public CompetitionFactoryLoader FactoryLoader { get; }= new CompetitionFactoryLoader();
		public PixelCompetitionServerConfig Config { get; } = new PixelCompetitionServerConfig();
		public PixelCompetitionServer Server { get; private set; }
		public ImageSender ImageSender { get; private set; }

		

		public ICanvas Canvas { get; set; }
		public FfMpegStarter Starter { get; private set; }

		public void run()
		{
			while (true)
			{
				try
				{
					if (Canvas == null)
					{
						Canvas = new RawVideoCanvas(
							Config.CanvasWidth, Config.CanvasHeight);
					}

					Server = PixelCompetitionServer.startServer(Config, Canvas);

					string pipeName = null;
					RawVideoCanvas canvas = Canvas as RawVideoCanvas;

					switch (Config.RawVideoInterface)
					{
						case PixelCompetitionServerConfig.RawVideoInterfaceType.Pipe:
							pipeName = Path.GetTempFileName();
							if (File.Exists(pipeName)) File.Delete(pipeName);
							if (canvas == null) break;
							ImageSender = new ImageSenderSocket(Config, canvas.Buffer, pipeName);
							break;
						case PixelCompetitionServerConfig.RawVideoInterfaceType.Tcp:
							if (canvas == null) break;
							ImageSender = new ImageSenderTcp(Config, canvas.Buffer);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					ImageSender?.startThread();

					Starter = FfMpegStarter.startProcess(Config, pipeName);

					if (!Config.UseSchedule)
					{
						var startFactory = FactoryLoader.createFactory(Config.StartCompetitionNumber);
						Server.startCompetition(new[]{startFactory});
						while (true)
						{
							Thread.Sleep(1000 * 60 * 60);
						}
					}

					Scheduler scheduler = new Scheduler(Config);
					var startFactories = scheduler.Current == null 
						? new[] {FactoryLoader.createFactory(Config.StartCompetitionNumber)}
						: FactoryLoader.createFactories(scheduler.Current.Competition);

					Server.startCompetition(startFactories);
					while (true)
					{
						if (scheduler.next())
						{
							Server.startCompetition(FactoryLoader.createFactories(scheduler.Current.Competition));
						}
						Thread.Sleep(60*1000);
						Server.cleanUp();
					}

				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
		}
	}
}
