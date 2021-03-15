using System.Drawing;

namespace PixelCompetitionServerLib
{
	public interface ICanvas
	{
		int Width { get; }
		int Height { get; }
		void drawPixel(int x, int y, Color color);
		void drawPixel(int x, int y, byte r, byte g, byte b);
	}
}