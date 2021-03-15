using System;
using System.Collections.Generic;
using PixelCompetitionServerLib;

namespace PixelCompetitionServerApp
{
	class App
	{
		// ReSharper disable once InconsistentNaming
		static void Main(string[] args)
		{
			Console.WriteLine("PixelCompetitionServer started");

			List<string> fileNames = new List<string>(args);
			fileNames.Insert(0, "config.xml");
			PixelCompetitionServerLib.PixelCompetitionServerApp app = new PixelCompetitionServerLib.PixelCompetitionServerApp();
			app.Config.assign(PixelCompetitionServerConfig.readConfig(fileNames));

			Console.WriteLine("Starting with Config:");
			Console.WriteLine(app.Config.toXml());

			app.run();
		}
	}
}

