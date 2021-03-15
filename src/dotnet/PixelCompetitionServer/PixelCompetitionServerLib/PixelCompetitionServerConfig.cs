using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace PixelCompetitionServerLib
{

	public class MergeHelper
	{ 
		public static void copyValues<T>(T source, T target)
		{
			Type t = typeof(T);

			var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

			foreach (var prop in properties)
			{
				var value = prop.GetValue(source, null);
				if (value != null)
					prop.SetValue(target, value, null);
			}
		}
	}

	public class ScheduleEntry
	{
		public DateTime Start;
		[XmlElement("Competition")]
		public List<string> Competition;
	}


	public class PixelCompetitionServerConfig
	{
		public enum RawVideoInterfaceType
		{
			Pipe,
			Tcp
		}
		public int LocalPort { get; set; } = 2342;
		public int CanvasWidth { get; set; } = 1920;
		public int CanvasHeight { get; set; } = 1080;
		public int Framerate { get; set; } = 5;

		public string DataFolder { get; set; }
		public RawVideoInterfaceType RawVideoInterface { get; set; }
		public string RawVideoTcpHost { get; set; } = "127.0.0.1";
		public int RawVideoTcpPort { get; set; } = 4455;

		public string StartCompetitionNumber { get; set; } = "B001";

		public int ClientTimeOut { get; set; } = 1000;


		public string InstanceWelcome { get; set; } =
			"Pixel-Competition @ vanilla build from public git-repo";

		// ReSharper disable once InconsistentNaming
		public string FFMpegCommand { get; set; } = "ffmpeg";
		// ReSharper disable once InconsistentNaming
		public string FFMpegParams { get; set; } = "";
		// ReSharper disable once InconsistentNaming
		public bool ShowFFMpegOutput { get; set; } = true;

		public bool UseSchedule { get; set; } = true;

		[XmlElement("Schedule")]
		public List<ScheduleEntry> Schedule = new List<ScheduleEntry>();


		public static PixelCompetitionServerConfig readConfig(List<string> configFileNames)
		{
			PixelCompetitionServerConfig res = new PixelCompetitionServerConfig();
			var currentFolder = Environment.CurrentDirectory;
			Console.WriteLine("Current folder: " + currentFolder);

			res.assign(
				configFileNames.Select(
					fn => new[] {Path.Join(currentFolder, fn), fn}).SelectMany(fn => fn));

			return res;
		}

		public string toXml()
		{
			StringWriter writer = new StringWriter();
			XmlSerializer serializer = new XmlSerializer(GetType());
			serializer.Serialize(writer, this);
			return writer.ToString();
		}

		public void assign(IEnumerable<string> fileNames)
		{
			foreach (var fileName in fileNames)
			{
				assign(fileName);
			}
		}

		public void assign(string fileName)
		{
			Console.WriteLine($"Searching for config file {fileName} ...");
			if (!File.Exists(fileName))
			{
				Console.WriteLine("... not found");
				return;
			}
			try
			{
				Console.WriteLine($"Read config file {fileName} ...");
				var deserializer = new XmlSerializer(GetType());
				var loaded = deserializer.Deserialize(File.OpenText(fileName)) as PixelCompetitionServerConfig;
				if (loaded == null) return;
				Console.WriteLine("Config before load:");
				Console.WriteLine(this.toXml());
				assign(loaded);
				Console.WriteLine("Config after load:");
				Console.WriteLine(this.toXml());
				Console.WriteLine($"Config applied from {fileName}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"... could not read this file: {ex}");
			}
		}

		public void assign(PixelCompetitionServerConfig loaded)
		{
			MergeHelper.copyValues(loaded, this);
			if (Schedule == null)
			{
				Schedule = new List<ScheduleEntry>(loaded.Schedule);
			}
			if (loaded.Schedule != null)
			{
				Schedule.AddRange(loaded.Schedule);
			}
		}
	}
}
