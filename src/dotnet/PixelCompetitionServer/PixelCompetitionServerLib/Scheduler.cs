using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelCompetitionServerLib
{
	public class Scheduler
	{
		private readonly Stack<ScheduleEntry> _entries;

		public ScheduleEntry Current { get; private set; }

		public bool next()
		{
			if (_entries.Count == 0) return false;
			if (_entries.Peek().Start < DateTime.Now)
			{
				Current = _entries.Pop();
				return true;
			}

			return false;
		}

		public Scheduler(PixelCompetitionServerConfig config)
		{
			_entries = new Stack<ScheduleEntry>(config.Schedule.OrderByDescending(e => e.Start));
			while (next())
			{
			}
		}
	}
}