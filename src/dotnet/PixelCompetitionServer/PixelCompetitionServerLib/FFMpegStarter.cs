using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace PixelCompetitionServerLib
{
	public class FfMpegStarter
	{
		private readonly PixelCompetitionServerConfig _config;
		private readonly string _pipeName;

		public FfMpegStarter(PixelCompetitionServerConfig config, string pipeName)
		{
			_config = config;
			_pipeName = pipeName;
		}
		public static FfMpegStarter startProcess(PixelCompetitionServerConfig config, string pipeName)
		{
			FfMpegStarter process = new FfMpegStarter(config, pipeName);
			new Thread(process.start) {IsBackground = true}.Start();
			return process;
		}

		public void start()
		{
			while (true)
			{
				Process process = new Process {StartInfo = {FileName = _config.FFMpegCommand}};

				string parameters = 
					_config.FFMpegParams
					.Trim()
					.Replace("\n", " ")
					.Replace("\r", " ");

				Console.WriteLine("Parameters from config:");
				Console.WriteLine(parameters);

				Dictionary<string, string> values = new Dictionary<string, string>()
				{
					{"{CanvasWidth}", _config.CanvasWidth.ToString()},
					{"{CanvasHeight}", _config.CanvasHeight.ToString()},
					{"{Framerate}", _config.Framerate.ToString()},
					{"{2Framerate}", (_config.Framerate * 2).ToString()},
					{"{PipeName}", _pipeName},
					{"{RawVideoTcpPort}", _config.RawVideoTcpPort.ToString()}, 
					{"{RawVideoTcpHost}", _config.RawVideoTcpHost}
				};

				Console.WriteLine(String.Join("", values.Keys));

				Console.WriteLine(String.Join(Environment.NewLine,
					values.Select(e => e.Key + ": " + e.Value)
					));

				foreach (var keyValuePair in values)
				{
					parameters = parameters.Replace(keyValuePair.Key, keyValuePair.Value);
				}

				Console.WriteLine("Parameters to execute:");
				Console.WriteLine(parameters);

				process.StartInfo.Arguments = parameters;

				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;

				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.RedirectStandardInput = true;

				if (_config.ShowFFMpegOutput)
				{
					process.OutputDataReceived += (sender, data) => { Console.WriteLine(data.Data); };
					process.ErrorDataReceived += (sender, data) => { Console.WriteLine(data.Data); };
				}
				else
				{
					process.OutputDataReceived += (sender, data) =>
					{
					};
					process.ErrorDataReceived += (sender, data) =>
					{
					};
				}

				Console.WriteLine(process.StartInfo.FileName + " " + process.StartInfo.Arguments);
				
				process.Start();
				if (_config.ShowFFMpegOutput)
				{
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
				}

				process.WaitForExit();
			}
			// ReSharper disable once FunctionNeverReturns
		}
	}
}