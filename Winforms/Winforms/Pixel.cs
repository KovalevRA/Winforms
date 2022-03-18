using System.Drawing;

namespace Winforms
{
	public class Pixel
	{
		public Pixel(Color color, int x, int y)
		{
			Color = color;
			Point = new Point(x, y);
		}
		public Color Color { get; private set; }
		public Point Point { get; private set; }
	}
}