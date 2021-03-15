using System.Drawing;
using PixelCompetitionServerLib.C.C050;

namespace PixelCompetitionServerLib.B.B003
{
	public class TurtleBot : BaseCompetition
	{
		public TurtleBot(ICanvas canvas, ResponseWriter responseWriter) : base(canvas, responseWriter)
		{
		}

		private int _x;
		private int _y;
		private int _currentDirection;

		readonly SnailBot.Dir[] _directions =
		{
			new SnailBot.Dir(){X=0, Y=1}, new SnailBot.Dir(){X=1, Y=0}, new SnailBot.Dir(){X=0, Y=-1}, new SnailBot.Dir(){X=-1, Y=0}
		};


		public override bool processInput(string[] command, ICompetition.ProcessState state)
		{
			if (command.Length != 1 && command.Length != 2 && command.Length != 4)
			{
				state.protocolError(
					"Each command shall have 0, 1 or 3 parameters in this competition.",
					(command.Length - 1).ToString());
				return false;
			}

			switch (command.Length)
			{
				case 1:
					switch (switchToken(command[0], new []{"F", "L", "R"}, false, "command", state))
					{
						case 0: 
							_x += _directions[_currentDirection].X;
							_y += _directions[_currentDirection].Y;
							return true;
						case 1:
							_currentDirection++;
							if (_currentDirection >= _directions.Length) _currentDirection = 0;
							return true;
						case 2:
							_currentDirection--;
							if (_currentDirection < 0) _currentDirection = _directions.Length - 1;
							return true;
						default:
							return false;
					}
				case 2:
				{
					if (!checkStringValue(command[0], "F")) return false;
					Color? c = parseColor(command[1], state);
					if (c==null) return false;
					_x += _directions[_currentDirection].X;
					_y += _directions[_currentDirection].Y;
					Canvas.drawPixel(_x,_y,c.Value);
					return true;
				}
				case 4:
				{
					if (!checkStringValue(command[0], "F")) return false;
					Color? c = parseColor(command[1], command[2], command[3],state);
					if (c == null) return false;
					_x += _directions[_currentDirection].X;
					_y += _directions[_currentDirection].Y;
					Canvas.drawPixel(_x, _y, c.Value);
					return true;
				}
				default: return false;
			}
		}
	}
}