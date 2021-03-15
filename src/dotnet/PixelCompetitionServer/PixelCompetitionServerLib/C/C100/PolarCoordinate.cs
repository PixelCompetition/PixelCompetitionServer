using System;
using System.Drawing;

namespace PixelCompetitionServerLib.C.C100
{
	public class PolarCoordinate : BaseCompetition
	{
		public PolarCoordinate(ICanvas canvas, ResponseWriter responseWriter):base(canvas, responseWriter)
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
			int l = parseInt(command[1], 0, Canvas.Width, "Length", state);
			int a = parseInt(command[2], 0, 10000, "Angle", state);

			// ReSharper disable once PossibleLossOfFraction
			int x = (int)(l * Math.Cos(2 * Math.PI *  a / 10000) + Canvas.Width / 2);
			// ReSharper disable once PossibleLossOfFraction
			int y = (int)(l * Math.Sin(2 * Math.PI *  a / 10000) + Canvas.Height / 2);
			if (command.Length == 6)
			{
				int r = parseInt(command[3], 0, 256, "pixel red component", state);
				int g = parseInt(command[4], 0, 256, "pixel green component", state);
				int b = parseInt(command[5], 0, 256, "pixel blue component", state);
				if (state.HasError) return false;
				Canvas.drawPixel(x, y, (byte)r, (byte)g, (byte)b);
				ResponseChannel.writeLine($"OK l:{l} a:{a} x:{x} y:{y}");
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
				ResponseChannel.writeLine($"OK l:{l} a:{a} x:{x} y:{y}");
				return true;
			}
			return false;
		}
	}
}
