using System.Drawing;

namespace PixelCompetitionServerLib.C.C050
{
	public class SnailBot : BaseCompetition
	{
		public SnailBot(ICanvas canvas, ResponseWriter responseWriter): base(canvas, responseWriter)
		{
			_x = Canvas.Width / 2;
			_y = Canvas.Height / 2;
		}

		private int _x;
		private int _y;
		public class Dir
		{
			public int X;
			public int Y;
		}

		readonly Dir[] _directions =
		{
			new Dir(){X=0, Y=1}, new Dir(){X=1, Y=0}, new Dir(){X=0, Y=-1}, new Dir(){X=-1, Y=0}
		};

		private int _currentDirection;
		private int _lineCounter = 2;
		private int _stepsToGo = 1;
		private bool _firstCommand = true;
		public void step()
		{
			_x += _directions[_currentDirection].X;
			_y += _directions[_currentDirection].Y;
			_stepsToGo--;
			if (_stepsToGo == 0)
			{
				_lineCounter++;
				_stepsToGo = _lineCounter / 2;
				_currentDirection++;
				if (_currentDirection >= _directions.Length) _currentDirection = 0;
			}
		}

		public override bool processInput(string[] command, ICompetition.ProcessState state)
		{
			if (command.Length != 2 && command.Length != 3 && command.Length != 4)
			{
				state.protocolError(
					"Each command shall have 1, 2 or 3 parameters in this competition.",
					(command.Length - 1).ToString());
				return false;
			}

			switch (command.Length)
			{
				case 3:
					if (!checkStringValue(command[0], "ST")) return false;
				{
					if (!_firstCommand)
					{
						state.protocolError("ST to set start coordinates has to be the first command.", command[0]);
						return false;
					}
					int x = parseInt(command[1], 0, Canvas.Width, "X coordinate", state);
					int y = parseInt(command[2], 0, Canvas.Height, "Y coordinate", state);
					if (state.HasError) return false;
					_x = x;
					_y = y;
					_firstCommand = false;
					return true;
				}
				case 2:
				{
					if (!checkStringValue(command[0], "PX")) return false;
					Color? c = parseColor(command[1], state);
					if (c==null) return false;
					Canvas.drawPixel(_x,_y,c.Value);
					step();
					_firstCommand = false;
					return true;
				}
				case 4:
				{
					if (!checkStringValue(command[0], "PX")) return false;
					Color? c = parseColor(command[1], command[2], command[3], state);
					if (c == null) return false;
					Canvas.drawPixel(_x, _y, c.Value);
					step();
					_firstCommand = false;
					return true;
				}
				default: return false;
			}
		}
	}
}
