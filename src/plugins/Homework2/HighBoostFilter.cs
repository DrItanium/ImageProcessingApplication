using System;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Libraries.Imaging;
using Libraries.Filter;
using System.Threading;
using System.ComponentModel;
using System.Collections;


namespace CS555.Homework2
{
	[Filter("High-Boosting Filter")]
		public class HighBoostingFilter : SpatialFilter
	{
		private static int[] square = new int[256];
		static HighBoostingFilter() 
		{
			for(int i = 0; i < 256; i++)
			{
				square[i] = (i * i);
			}
		}
		public HighBoostingFilter(string name) : base(name) { }
		protected override string InputFormAddition { get { return "label new \"weightLabel\" \"Name\" imbue \"Weight\" \"Text\" imbue 13 72 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"weight\" \"Name\" imbue 80 72 point \"Location\" imbue \"Controls.Add\" imbue"; } }
		protected override Hashtable TranslateData_Impl(Hashtable input)
		{
			input["weight"] = decimal.Parse((string)input["weight"]); 
			return input;
		}
		public override byte[][] Transform(Hashtable input)
		{
			byte[][] image = (byte[][])input["image"];
			int width = image.Length;
			int height = image[0].Length;
			byte[][] resultant = new byte[width][];
			//Blur the image
			byte[][] blurred = Blur((int)input["mask"], image);
			decimal weight = (decimal)input["weight"];
			for(int x = 0; x < width; x++)
			{
				byte[] iX = image[x];
				byte[] bX = blurred[x];
				byte[] rX = new byte[height];
				for(int y = 0; y < height; y++)
				{
					int originalValue = iX[y];
					int blurValue = bX[y];

					int maskVal = (int)((decimal)(originalValue - blurValue) * weight);
					int final = originalValue + maskVal;
					if(final < 0) 
					{
						final = 0;
					}
					else if(final > 255) 
					{
						final = 255;
					}
					rX[y] = (byte)final;
				}
				resultant[x] = rX;
			}
			return resultant;
		}
		protected override byte Operation(int a, int b, int c, int d, byte[][] e, Hashtable f) { return (byte)0; }
		private byte[][] Blur(int maskSize, byte[][] input)
		{
			int width = input.Length;
			int height = input[0].Length;
			byte[][] clone = new byte[width][];
			int m = maskSize;	
			int a = (m - 1) >> 1;
			int b = a;
			for(int x = 0; x < width; x++)
			{
				byte[] z = new byte[height];
				for(int y = 0; y < height; y++) 
				{
					z[y] = Blur0(a, b, x, y, input);
				}
				clone[x] = z;
			}
			return clone;
		}
		private static byte Blur0(int a, int b, int x, int y, byte[][] input)
		{
			int width = input.Length;
			int height = input[0].Length;
			decimal denominator = 0.0M;
			decimal numerator = 0.0M;
			for(int s = -a; s < a; s++)
			{
				int wX = x + s;
				if(wX < 0 || wX >= width)
					continue;
				byte[] iX = input[wX];
				for(int t = -b; t < b; t++)
				{
					int wY = y + t;
					if(wY < 0 || wY >= height)
						continue;
					int w = iX[wY];
					//get rid of f because we can do the same thing with a memoized table
					//This removes a ton of potential multiplies in really large images
					//
					//This is possible because f was always equal to w and since w is
					//always between 0 and 255, we can save a lot of time by storing the
					//results in an array    
					numerator += square[w];
					denominator += w;
				}
			}	
			return (byte)((int)(numerator / denominator));
		}
	}
}
