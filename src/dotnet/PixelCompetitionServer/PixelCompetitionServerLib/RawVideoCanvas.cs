using System.Drawing;

namespace PixelCompetitionServerLib
{
	public class RawVideoCanvas : ICanvas
	{
		public byte[] Buffer { get; }
		public RawVideoCanvas(int width, int height)
		{
			Buffer = new byte[width * height * 3];
			Width = width;
			Height = height;
		}

		public int Width { get; }
		public int Height { get; }
		public void drawPixel(int x, int y, Color color)
		{
			drawPixel(x,y,color.R, color.G, color.B);
		}

		public void drawPixel(int x, int y, byte r, byte g, byte b)
		{
			if (x < 0 || y < 0 || x >= Width || y >= Height) return;
			int offset = 3 * (y * Width  + x); // TODO double check calculation
			Buffer[offset] = r;
			Buffer[offset+1] = g;
			Buffer[offset+2] = b;
		}
	}
}