using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PixelCompetitionServerLib.Riddle
{

	static class RandomExtensions
	{

		// TODO: There might be better solution
		public static void shuffle<T>(this Random rng, T[] array)
		{
			int n = array.Length;
			while (n > 1)
			{
				int k = rng.Next(n--);
				T temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}
		}
	}
	public abstract class RiddleCompetitionFactory : BaseCompetitionFactory
	{
		private Riddle[,] _currentRiddles;
		protected int MinRiddleCacheSize = 1024;
		protected ConcurrentQueue<Riddle> Riddles = new ConcurrentQueue<Riddle>();
		public abstract IEnumerable<Riddle> createRiddles(int need);

		public Task FillRiddlesTask;

		public void fillRiddles(int len = -1)
		{
			if (FillRiddlesTask != null) return;
			if (len < 0) len = MinRiddleCacheSize;
			if (Riddles.Count >= len) return;
			try
			{
				FillRiddlesTask = Task.Run(() =>
					{
						try
						{
							while (Riddles.Count < len)
							{
								foreach (var riddle in createRiddles(len - Riddles.Count))
								{
									Riddles.Enqueue(riddle);
								}
							}
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							//ignore
						}
					}
					);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				//ignore
			}
			finally
			{
				FillRiddlesTask = null;
			}
		}

		private int Width => Server?.CurrentCanvas?.Width ?? 0;
		private int Height => Server?.CurrentCanvas?.Height ?? 0;
		public override void assignServer(PixelCompetitionServer server)
		{
			base.assignServer(server);
			_currentRiddles = new Riddle[Width,Height];
			fillRiddles(Width*Height + MinRiddleCacheSize);
		}

		public Riddle getRiddle(int x, int y)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height) return null;
			fillRiddles();

			if (_currentRiddles[x, y] != null) return _currentRiddles[x, y];
			{
				lock (_currentRiddles)
				{
					if (_currentRiddles[x,y] != null) return _currentRiddles[x, y];
					if (Riddles.TryDequeue(out var res))
					{
						_currentRiddles[x, y] = res;
						return res;
					}
					else
					{
						return null;
					}
				}
			}
		}

		public void riddleSolved(Riddle riddle)
		{
			fillRiddles();
			if (Riddles.TryDequeue(out var newRiddle))
			{
				riddle.assign(newRiddle);
			}
			// if no new riddle was available, we just keep the old one
		}

		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseChannel)
		{
			return new RiddleCompetition(canvas, responseChannel, this);
		}
	}
}
