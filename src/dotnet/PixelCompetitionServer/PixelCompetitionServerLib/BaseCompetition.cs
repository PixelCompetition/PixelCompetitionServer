using System.Drawing;

namespace PixelCompetitionServerLib
{
	public abstract class BaseCompetition : ICompetition
	{
		protected ICanvas Canvas;
		protected ResponseWriter ResponseChannel;
		protected BaseCompetition(ICanvas canvas, ResponseWriter responseWriter)
		{
			Canvas = canvas;
			ResponseChannel = responseWriter;
		}
		protected static int switchToken(string token, string[] validTokens, bool strict, string name, 
			ICompetition.ProcessState state)
		{
			int count = 0;
			foreach (string validToken in validTokens)
			{
				if (token == validToken) return count;
				count++;
			}

			if (strict)
			{
				string values = string.Join(", ", validTokens);
				state.protocolError($"{name} is not a valid value, one of {values} was expected", token);
			}
			return -1;
		}
		protected static int parseInt(string token, int minValue, int maxValue, string name,
			ICompetition.ProcessState state)
		{
			if (!int.TryParse(token, out var res)) state.protocolError($"{name} is not a valid integer", token);
			if (res < minValue) state.protocolError($"{name} value is too small, should be at least {minValue}", token);
			if (res > maxValue) state.protocolError($"{name} value is too big, should be smaller than {maxValue}", token);
			return res;
		}

		protected static Color? parseColor(string html, ICompetition.ProcessState state)
		{
			Color color;
			try
			{
				color = ColorTranslator.FromHtml(html);
			}
			catch
			{
				state.protocolError("Could not parse color value", html);
				return null;
			}
			return color;
		}

		protected static Color? parseColor(string redToken, string greenToken, string blueToken,
			ICompetition.ProcessState state)
		{
			try
			{
				int r = parseInt(redToken, 0, 256, "pixel red component", state);
				int g = parseInt(greenToken, 0, 256, "pixel green component", state);
				int b = parseInt(blueToken, 0, 256, "pixel blue component", state);
				if (state.HasError) return null;
				return Color.FromArgb(r,g,b);
			}
			catch
			{
				state.protocolError("Could not parse color value", redToken+" "+redToken+""+blueToken);
				return null;
			}
		}

		protected static bool checkStringValue(string token, string value)
		{
			if (token == value) return true;

			return false;
		}
		public abstract bool processInput(string[] command, ICompetition.ProcessState state);

	}
}
