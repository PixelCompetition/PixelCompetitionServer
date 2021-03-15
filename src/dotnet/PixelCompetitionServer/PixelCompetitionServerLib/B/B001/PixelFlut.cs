using System.Drawing;

namespace PixelCompetitionServerLib.B.B001
{
	public class PixelFlut : BaseCompetition
	{
		public PixelFlut(ICanvas canvas, ResponseWriter responseWriter):base(canvas, responseWriter)
		{
		}
		public override bool processInput(string[] command, ICompetition.ProcessState state)
		{
			if (command.Length != 6 && command.Length != 4)
			{
				state.protocolError(
					"Each PX command shall have 3 or 5 parameters in this competition.",
					(command.Length - 1).ToString());
				return false;
			}
			if (!checkStringValue(command[0], "PX")) return false;
			int x = parseInt(command[1], 0, Canvas.Width, "X coordinate", state);
			int y = parseInt(command[2], 0, Canvas.Height, "Y coordinate", state);
			if (command.Length == 6)
			{
				int r = parseInt(command[3], 0, 256, "pixel red component", state);
				int g = parseInt(command[4], 0, 256, "pixel green component", state);
				int b = parseInt(command[5], 0, 256, "pixel blue component", state);
				if (state.HasError) return false;
				Canvas.drawPixel(x, y, (byte)r, (byte)g, (byte)b);
				return true;
			}

			if (command.Length == 4)
			{
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
				Canvas.drawPixel(x, y, color);
				return true;
			}
			return false;
		}
	}
}
