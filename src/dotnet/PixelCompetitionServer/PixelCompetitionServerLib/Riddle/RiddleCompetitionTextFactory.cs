using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PixelCompetitionServerLib.Riddle
{
	public abstract class RiddleCompetitionTextFactory : RiddleCompetitionFactory
	{
		private const int RiddlePerFile = 2 ^ 14; // Just a convention, might be different
		protected abstract string FolderName { get; }

		readonly Random _random = new Random();
		private readonly ConcurrentStack<string> _fileNames = new ConcurrentStack<string>();

		private string nextFileName()
		{
            if (_fileNames.IsEmpty)
            {

                var fileNames =
                    Directory.EnumerateFiles(
                        Path.Join(Server.Config.DataFolder, FolderName)
                    ).ToArray();
                _random.shuffle(fileNames);
                _fileNames.PushRange(fileNames);
            }
            return _fileNames.TryPop(out var fileName) ? fileName : null;
		}

		public override IEnumerable<Riddle> createRiddles(int len)
		{

			List<Riddle> loadedRiddles = new List<Riddle>(len + RiddlePerFile); 
			// no need for growth, but riddlePerFile might be violated

			while (loadedRiddles.Count < len)
			{
				string fileName = nextFileName();
				Console.WriteLine($"Load file {fileName}");
				string answer = null;
				foreach (var line in File.ReadLines(fileName))
				{
					if (answer == null)
					{
						answer = line;
					}
					else
					{
						loadedRiddles.Add(new Riddle(line,answer));
						answer = null;
					}
				}
			}
			var arr = loadedRiddles.ToArray();
			_random.shuffle(arr);
			return arr;
		}
	}
}
