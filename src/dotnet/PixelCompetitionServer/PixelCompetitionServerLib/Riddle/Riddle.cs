using System.Collections.Generic;
using System.Linq;

namespace PixelCompetitionServerLib.Riddle
{
	public class Riddle
	{
		public string Question { get; private set; }
		public string Answer { get; private set; }

		public Riddle(string question, string answer)
		{
			Question = question;
			Answer = answer;
		}
		public void assign(Riddle newRiddle)
		{
			Question = newRiddle.Question;
			Answer = newRiddle.Answer;
		}

		public void combine(IEnumerable<Riddle> newRiddles)
		{
			var arr= newRiddles.ToArray();
			Question = string.Join(" ", arr.Select(r => r.Question));
			Answer = string.Join(" ", arr.Select(r => r.Answer));
		}


	}
}