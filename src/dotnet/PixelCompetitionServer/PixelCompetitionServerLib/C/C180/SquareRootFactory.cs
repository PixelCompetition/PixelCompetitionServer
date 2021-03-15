using System;
using System.Collections.Generic;
using System.Numerics;
using PixelCompetitionServerLib.Riddle;

namespace PixelCompetitionServerLib.C.C180
{
    // ReSharper disable once UnusedMember.Global
    public class SquareRootFactory : RiddleCompetitionFactory
	{
		readonly Random _random = new Random();

		public override IEnumerable<Riddle.Riddle> createRiddles(int need)
		{
			for (int i = 0; i < need; i++)
			{
				BigInteger answer = new BigInteger(_random.Next(1, Int32.MaxValue));
				BigInteger question = answer * answer;
				yield return new Riddle.Riddle(question.ToString(), answer.ToString());
			}
		}
	}
}
