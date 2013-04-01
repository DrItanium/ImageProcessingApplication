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
	[Filter("Bilinear Interpolation Scaling")]
		public class BilinearInterpolationFilter : BasicScalingFilter
	{
		public BilinearInterpolationFilter(string name) : base(name) { }

		protected override int[][] Interpolate(int[][] srcImage, int?[][] elements,
				float wFac, float hFac)
		{
			int sWidth = srcImage.Length - 1;
			int sHeight = srcImage[0].Length - 1;
			int width = elements.Length;
			int height = elements[0].Length;
			float[] iW = new float[width]; //precomputed width
			float[] iH = new float[height]; //precomputed height
			int[][] output = new int[width][];
			for(int i = 0; i < width; i++)
			{
				//iW contains a translation table that converts offsets in the larger
				//image to offsets in the smaller one. So, if the source image is p
				//pixels wide and the destination image is 2p pixels wide then each
				//cell in iW will refer twice to a pixel in the source image.
				//
				//This purpose of this table is to make accessing pixels in the source
				//image as easy as possible. The use of memoization also cuts down on
				//recomputation as well as code duplication. 
				//
				//Floats are used to make sure that we can get sub-pixel results
				iW[i] = (Math.Min(sWidth, ((float)i) / wFac));
				//Instatiate a line of pixels for the output image
				output[i] = new int[height];
			}
			for(int i = 0; i < height; i++) 
			{
				//compute the translation from destination image offsets to source
				//image offsets for the height component. 
				iH[i] = Math.Min(sHeight, ((float)i) / hFac);
			}
			//go back in time and grab the previous non-null x-pixel

			/* The first step is to define some variables to make this understandable
			 * x1, y1, x2, and y2 are variables that represent the four Q values
			 * acquired from the source image to be used to determine the unknown
			 * value at x, y. 
			 *
			 * These variables will then be used to acquire two points R1 and R2
			 * which are used to determine the corresponding value at x and y. 
			 */

			for(int i = 0; i < width; i++) //x
			{
				//this isn't going to change...why continually recompute it?
				//start interpolation in the x-direction
				//get the translation table offset
				float x = iW[i];
				//take the floor of x
				int rPx = (int)Math.Floor(x);
				//if the x coordinate is equal to the width of the source image - 1
				//then subtract by one again. This ensures that we have four pixels to 
				//interpolate with 
				if(rPx == sWidth) 
				{
					//since rPx == sWidth we don't need to reference sWidth again
					//Just -- rPx. This should save a register on some architectures
					rPx--;
				}
				float x1 = rPx;
				float x2 = rPx + 1;
				//compute the divisor or the distance between the two points 
				//This may not always be one
				float divisor0 = x2 - x1;
				//compute the distances from x to x2
				float num0 = x2 - x;
				//compute the distance from x1 to x
				float num1 = x - x1;
				//take the distance from x to x2 and divide it by the distance between
				//x1 and x2
				float factor0 = num0 / divisor0;
				//take advantage of the principle of locality since these are arrays of
				//arrays instead of a flatarray with offsets
				int?[] line = elements[i];
				int[] srcLine1 = srcImage[(int)x1];
				int[] srcLine2 = srcImage[(int)x2];
				int[] outLine = output[i];
				for(int j = 0; j < height; j++) //y
				{
					//a non null byte means that a pixel is already there
					int? val = line[j];
					if(val == null)
					{
						//get the offset value for the height component
						float y = iH[j];
						//compute the floor of y
						int rPy = (int)Math.Floor(y);
						//make sure that we have at least two pixels to interpolate off of.
						if(rPy == sHeight)
						{
							//Save a register, use -- instead of rPy = sHeight - 1;
							rPy--;
						}
						//store those points
						float y1 = rPy;
						float y2 = rPy + 1;
						//get the distance between the y coordinates
						float divisor1 = y2 - y1;
						//take the distance from the current pixel to x1 
						//and divide it by the distance between y1 and y2
						float factor1 = num1 / divisor1;

						//these are the bilinear interpolation points
						//surrounding the target pixel
						//Grab the values stored in our interpolation points
						//Since this is color, we need to decompose them into three sets of
						//values for red, green , and blue
						Color qc11 = Color.FromArgb(srcLine1[(int)y1]);
						Color qc21 = Color.FromArgb(srcLine2[(int)y1]);
						Color qc12 = Color.FromArgb(srcLine1[(int)y2]);
						Color qc22 = Color.FromArgb(srcLine2[(int)y2]);

						//store the result in our output line
						outLine[j] = Conv(qc11, qc21, qc12, qc22, factor0, factor1, divisor1, y2, y1, y);
					}
					else
					{
						outLine[j] = (int)val; 
					}
				}
			}
			return output;
		}
		private static int Conv(Color qc11, Color qc21, Color qc12, Color qc22, 
				float factor0, float factor1, float divisor1, float y2, float y1, float y)
		{
			//we need to computer R1 and R2 which are points computed using the
			//four surrounding pixels in the source image
			//to get R1 we multiply factor0 by q11 and add it to 
			//factor1 multiplied by q21
			return Color.FromArgb(255, 
					Conv((float)qc11.R, (float)qc21.R,
						(float)qc12.R, (float)qc22.R,
						factor0, factor1, divisor1, y2, y1, y),
					Conv((float)qc11.G, (float)qc21.G,
						(float)qc12.G, (float)qc22.G,
						factor0, factor1, divisor1, y2, y1, y),
					Conv((float)qc11.B, (float)qc21.B,
						(float)qc12.B, (float)qc22.B,
						factor0, factor1, divisor1, y2, y1, y)).ToArgb();
		}
		private static int Conv(float q11, float q21, float q12, float q22, 
				float factor0, float factor1, float divisor1, float y2, float y1, float y) 
		{
			float fR1a = factor0 * q11;
			float fR1b = factor1 * q21;
			float fR1 = fR1a + fR1b;
			//to get R2 we multiply factor0 by q12 and add it to 
			//factor1 multiplied by q22
			float fR2a = factor0 * q12;
			float fR2b = factor1 * q22;
			float fR2 = fR2a + fR2b; 
			//we then interpolate in the y-direction now that we've finished
			//computing in the x direction
			//
			//Compute the two parts
			float fPa = ((y2 - y) / divisor1) * fR1;
			float fPb = ((y - y1) / divisor1) * fR2;
			//add them together
			float fP = fPa + fPb;
			//if the value is less than zero then the value is zero
			int rslt = (int)Math.Max(0, Math.Round(fP));
			//if the value is greater than 255 (remember bytes)
			//then set the value to 255
			if(rslt > 255)
				rslt = 255;
			return rslt;
		}
	}
}
