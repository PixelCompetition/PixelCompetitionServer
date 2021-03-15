using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelCompetitionServerLib;

namespace MyPixelCompetition
{
	public partial class Form1 : Form, ICanvas
	{
		public Form1()
		{
			InitializeComponent();
		}

		int ICanvas.Width
		{
			get { return Width; }
		}

		int ICanvas.Height
		{
			get { return Height; }
		}

		public void DrawPixel(int x, int y, Color color)
		{
			Brush brush = new SolidBrush(color);
			Canvas.FillRectangle(brush, x, y, 1,1);
		}
		public void DrawPixel(int x, int y, byte r, byte g, byte b)
		{
			DrawPixel(x, y, Color.FromArgb(r, g, b));
		}

		private Graphics Canvas = null;
		private void Form1_Load(object sender, EventArgs e)
		{
			Canvas = this.CreateGraphics();
		}
	}
}
