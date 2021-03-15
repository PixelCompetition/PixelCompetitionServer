using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelCompetitionServerLib;
using PixelCompetitionServerLib.B.B001;

namespace MyPixelCompetition
{
	static class Program
	{

		public static PixelCompetitionServerConfig Config = new PixelCompetitionServerConfig();

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);



			var exeFolder = Environment.CurrentDirectory;
			Console.WriteLine(exeFolder);
			string configFileName = Path.Join(exeFolder, "config.txt");
			if (args.Length > 0)
			{
				configFileName = args[0];
				if (!File.Exists(configFileName))
				{
					configFileName = Path.Join(exeFolder, configFileName);
				}
			}
			Console.WriteLine("Config file: " + configFileName);
			if (File.Exists(configFileName))
			{
				var cfg = TextConfigReader.readConfig(configFileName);
				Console.WriteLine(TextConfigReader.formatConfig(cfg));
				Config.assign(cfg);
			}


			try
			{
				var form = new Form1();

				Config.CurrentCanvas = form;
				Config.CurrentFactories.Add(new CommonCompetitionFactory()); //TODO language
				Config.CurrentFactories.Add(new PixelFlutFactory());
				PixelCompetitionServer.startServer(Config);

				Application.Run(form);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
