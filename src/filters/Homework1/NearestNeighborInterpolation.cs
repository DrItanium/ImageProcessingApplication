using System;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Libraries.Imaging;
using Libraries.Filter;

namespace CS555.Homework1
{
	[Filter("Nearest Neighbor Interpolation Scaling")]
		public class NearestNeighborInterpolationFilter : BasicScalingFilter
	{
		public NearestNeighborInterpolationFilter(string name) : base(name) { }

		protected override int[][] Interpolate(int[][] srcImage, int?[][] elements,
				float wFac, float hFac)
		{
			//TODO: Move this over to compression lists to save space
			int sWidth = srcImage.Length - 1;
			int sHeight = srcImage[0].Length - 1;
			int width = elements.Length;
			int height = elements[0].Length;
			int[] iW = new int[width]; //precomputed width
			int[] iH = new int[height]; //precomputed height
			int[][] output = new int[width][];
			for(int i = 0; i < width; i++)
			{
				//multipurpose this!
				iW[i] = (int)(Math.Min(sWidth, ((float)i) / wFac));
				output[i] = new int[height];
			}
			for(int i = 0; i < height; i++)
				iH[i] = (int)(Math.Min(sHeight, ((float)i) / hFac));
			//go back in time and grab the previous non-null x-pixel
			for(int i = 0; i < width; i++) //x
			{
				//this isn't going to change...why continually recompute it?
				int x = iW[i];
				int?[] line = elements[i];
				int[] srcLine = srcImage[x];
				int[] outLine = output[i];
				for(int j = 0; j < height; j++) //y
				{
					int? val = line[j];
					if(val == null)
					{
						int y = iH[j];
						outLine[j] = srcLine[y];
					}
					else
						outLine[j] = (int)val; 
				}
				elements[i] = null; //clean this out as we go along
				line = null; //clean this out as we go along
			}
			return output;
		}
	}
}
