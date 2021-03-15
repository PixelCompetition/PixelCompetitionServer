using System.Drawing;
using System.Linq;

namespace PixelCompetitionServerLib.Riddle
{
	public class RiddleCompetition : BaseCompetition
	{

		protected RiddleCompetitionFactory Factory;

		public RiddleCompetition(ICanvas canvas, ResponseWriter responseWriter, RiddleCompetitionFactory factory) : base(
			canvas, responseWriter)
		{
			Factory = factory;
		}

		public override bool processInput(string[] command, ICompetition.ProcessState state)
		{
			if (command.Length < 3)
			{
				state.protocolError(
					"Each command shall have at least 2 parameters in this competition.",
					(command.Length - 1).ToString());
				return false;
			}

			int cmdSwitch = switchToken(command[0], new[] {"ASK", "PX"}, true, "command", state);
			if (cmdSwitch != 0 && cmdSwitch != 1) return false;
			int x = parseInt(command[1], 0, Canvas.Width, "X coordinate", state);
			int y = parseInt(command[2], 0, Canvas.Height, "Y coordinate", state);
			if (state.HasError) return false;
			Riddle riddle = Factory.getRiddle(x, y);
			if (riddle == null)
			{
				state.protocolError("Internal Server Error", command[0]);
				return false;
			}

			switch (cmdSwitch)
			{
				case 0: // ASK
				{
					ResponseChannel.writeLine($"RIDDLE {x} {y} {riddle.Question}");
					return true;
				}
				case 1: // PX
				{
					if (command.Length < 5)
					{
						state.protocolError(
							"Each PX command shall have at least 4 parameters in this competition.",
							(command.Length - 1).ToString());
						return false;
					}

					Color color;
					try
					{
						color = ColorTranslator.FromHtml(command[3]);
					}
					catch
					{
						state.protocolError("Could not parse color value", command[3]);
						return false;
					}

					if (state.HasError) return false;

					bool error = false;
					lock (riddle)
					{
						if (riddle.Answer.Equals(string.Join(" ", command.Skip(4))))
						{
							Canvas.drawPixel(x, y, color);
							Factory.riddleSolved(riddle);
						}
						else
						{
							error = true;
						}
					}

					if (error)
					{
						ResponseChannel.writeLine($"Nope {x} {y}");
						ResponseChannel.writeLine($"RIDDLE {x} {y} {riddle.Question}");
					}
					else
					{
						ResponseChannel.writeLine("OK");
					}
					return true;
				}
				default: return false;
			}

		}
	}
}