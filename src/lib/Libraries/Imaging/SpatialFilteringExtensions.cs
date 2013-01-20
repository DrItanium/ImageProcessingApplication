using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;


namespace Libraries.Imaging
{
	public static partial class SpatialFilteringExtensions
	{
		public static Bitmap Correlation(this Bitmap input, byte[,] mask, int maskWidth, int maskHeight)
		{
			Bitmap clone = input.Clone() as Bitmap;
			int total = 0;
			int a = (maskWidth - 1) >> 1;
			int b = (maskHeight - 1) >> 1;
			for(int x = 0; x < input.Width; x++)
			{
				for(int y = 0; y < input.Height; y++)
				{
					for(int s = 0, _s = -a; s < maskWidth; s++, _s++)
					{
						int wX = x + _s;
						if(wX < 0 || wX >= input.Width)
							continue;
						for(int t = 0, _t = -b; t < maskHeight; t++, _t++)
						{
							int wY = y + _t;
							if(wY < 0 || wY >= input.Height)
								continue;
							int w = mask[s, t];
							int f = input.GetPixel(wX, wY).R;
							total += (w * f);
						}
					}	
					byte value = (byte)total;
					clone.SetPixel(x,y, Color.FromArgb(255, value, value, value));
					total = 0;
				}
			}
			return clone;
		}
		public static Bitmap Convolution(this Bitmap input, byte[,] mask, int maskWidth, int maskHeight)
		{
			Bitmap clone = input.Clone() as Bitmap;
			int total = 0;
			int a = (maskWidth - 1) >> 1;
			int b = (maskHeight - 1) >> 1;
			for(int x = 0; x < input.Width; x++)
			{
				for(int y = 0; y < input.Height; y++)
				{
					for(int s = 0, _s = -a; s < maskWidth; s++, _s++)
					{
						int fX = x - _s;
						if(fX >= 0)
							continue;
						for(int t = 0, _t = -b; t < maskHeight; t++, _t++)
						{
							int fY = y - _t;
							if(fY >= 0)
								continue;
							int w = mask[s, t];
							int f = input.GetPixel(fX, fY).R;
							total += (w * f);
						}
					}	
					byte value = (byte)total;
					clone.SetPixel(x,y, Color.FromArgb(255, value, value, value));
					total = 0;
				}
			}
			return clone;
		}
	}
}
