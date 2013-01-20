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

		protected override byte[][] Interpolate(byte[][] srcImage, byte?[][] elements,
				float wFac, float hFac)
		{
			//TODO: Move this over to compression lists to save space
			int sWidth = srcImage.Length - 1;
			int sHeight = srcImage[0].Length - 1;
			int width = elements.Length;
			int height = elements[0].Length;
			float[] iW = new float[width]; //precomputed width
			float[] iH = new float[height]; //precomputed height
			byte[][] output = new byte[width][];
			for(int i = 0; i < width; i++)
			{
				//multipurpose this!
				iW[i] = (Math.Min(sWidth, ((float)i) / wFac));
				output[i] = new byte[height];
			}
			for(int i = 0; i < height; i++)
				iH[i] = Math.Min(sHeight, ((float)i) / hFac);
			//go back in time and grab the previous non-null x-pixel
			for(int i = 0; i < width; i++) //x
			{
				//this isn't going to change...why continually recompute it?
				float x = iW[i];
				int rPx = (int)Math.Floor(x);
				if(rPx == sWidth)
					rPx = sWidth - 1;
				float x1 = rPx;
				float x2 = rPx + 1;
				float divisor0 = x2 - x1;
				float num0 = x2 - x;
				float num1 = x - x1;
				float factor0 = num0 / divisor0;
				byte?[] line = elements[i];
				byte[] srcLine1 = srcImage[(int)x1];
				byte[] srcLine2 = srcImage[(int)x2];
				byte[] outLine = output[i];
				for(int j = 0; j < height; j++) //y
				{
					byte? val = line[j];
					if(val == null)
					{
						float y = iH[j];
						int rPy = (int)Math.Floor(y);
						if(rPy == sHeight)
							rPy = sHeight - 1;
						float y1 = rPy;
						float y2 = rPy + 1;
						float q11 = srcLine1[(int)y1];
						float q21 = srcLine2[(int)y1];
						float q12 = srcLine1[(int)y2];
						float q22 = srcLine2[(int)y2];
						float divisor1 = y2 - y1;
						float factor1 = num1 / divisor1;
						//{
						float fR1a = factor0 * q11;
						float fR1b = factor1 * q21;
						float fR1 = fR1a + fR1b;
						float fR2a = factor0 * q12;
						float fR2b = factor1 * q22;
						float fR2 = fR2a + fR2b;
						float fPa = ((y2 - y) / divisor1) * fR1;
						float fPb = ((y - y1) / divisor1) * fR2;
						float fP = fPa + fPb;
						//return fP;
						//}
						int rslt = (int)Math.Max(0, Math.Round(fP));
						if(rslt > 255)
							rslt = 255;
						outLine[j] = (byte)rslt;
					}
					else
						outLine[j] = (byte)val; 
				}
			}
			return output;
		}
	}
}
