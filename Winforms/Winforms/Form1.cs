using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms
{
	public partial class Form1 : Form
	{
		List<Bitmap> _bitmaps = new List<Bitmap>();
		Random _random = new Random();
		public Form1()
		{
			InitializeComponent();
		}

		private async void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var sw = Stopwatch.StartNew();
				menuStrip1.Enabled = trackBar1.Enabled = false;
				pictureBox1.Image = null;
				_bitmaps.Clear();
				var bitmap = new Bitmap(openFileDialog1.FileName);
				await Task.Run(() => RunProcessing(bitmap));
				menuStrip1.Enabled = trackBar1.Enabled = true;
				pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
				sw.Stop();
				Text = sw.Elapsed.ToString();
			}
		}

		private void RunProcessing(Bitmap bitmap)
		{
			var pixels = GetPixels(bitmap);
			var pixelsInStep = (bitmap.Width * bitmap.Height) / trackBar1.Maximum;
			var currentPixelsSet = new List<Pixel>(pixels.Count - pixelsInStep);
			for (int i = trackBar1.Minimum; i < trackBar1.Maximum; i++)
			{
				for (int j = 0; j < pixelsInStep; j++)
				{
					var index = _random.Next(pixels.Count);
					currentPixelsSet.Add(pixels[index]);
					pixels.RemoveAt(index);
				}

				var currentBitmap = new Bitmap(bitmap.Width, bitmap.Height);
				foreach (var pixel in currentPixelsSet)
					currentBitmap.SetPixel(pixel.Point.X, pixel.Point.Y, pixel.Color);
				_bitmaps.Add(currentBitmap);

				this.Invoke(new Action(() => Text = $"{i} %"));
			}
			_bitmaps.Add(bitmap);
		}

		private List<Pixel> GetPixels(Bitmap bitmap)
		{
			var pixels = new List<Pixel>(bitmap.Width * bitmap.Height);

			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					pixels.Add(new Pixel(bitmap.GetPixel(x, y), x, y));
				}
			}
			return pixels;
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			if (_bitmaps == null || _bitmaps.Count == 0)
				return;

			pictureBox1.Image = _bitmaps[trackBar1.Value - 1];
		}
	}
}
