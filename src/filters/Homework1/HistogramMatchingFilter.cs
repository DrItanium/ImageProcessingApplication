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

/* This filter takes in two images. One that the operation is going to be
 * applied to and the other that is used to match the original's histogram
 * against. 
 */

namespace CS555.Homework1
{
	[Filter("Histogram Matching")]
		public class HistogramMatching : ImageFilter 
	{
		private byte[] glRed, glGreen, glBlue;
		public override string InputForm
		{
			get
			{
				return "form new \"Histogram Matching\" \"Text\" imbue label new \"openLabel\" \"Name\" imbue \"Select Image\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue button new \"imageSelector\" \"Name\" imbue \"Open\" \"Text\" imbue 75 23 size \"Size\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue return";
			}
		}
		public HistogramMatching(string name) : base(name) 
		{
			glRed = new byte[256];
			glBlue = new byte[256];
			glGreen = new byte[256];
		}
		public override Hashtable TranslateData(Hashtable source)
		{
			//nothing to do
			//Console.WriteLine("Typeof source[\"otherImage\"] = {0}", 
			//    source["otherImage"].GetType());
			return source;
		}
		private static byte[] GenerateConversion(byte[] g, byte[] r, byte[] s)
		{
			//bind s of cloneHisto to G which maps to z
			byte[] result = new byte[256];
			for(int j = 0; j < 256; j++)
			{
					//bind r0_k which returns s0_k which we map to r1_k which maps to s1_k
					//thus it means that r0_k needs to return s1_k
					//but to perform the binding we use g
					byte curr = s[j];
					int index = ClosestValue(curr, g);
					//this ensures that each value will map to the proper element in the
					//other image
					result[j] = r[ClosestValue(g[index], r)];
			}
			return result;
		}
		public override int[][] TransformImage(Hashtable source)
		{
			int[][] aImage = (int[][])source["image"];
			int[][] oImage = (int[][])source["otherImage"];
			int aWidth = aImage.Length;
			int aHeight = aImage[0].Length;
			int[][] cImage = new int[aWidth][];
			ColorHistogram cloneHisto = new ColorHistogram(aImage);
			ColorHistogram refHisto = new ColorHistogram(oImage);
			for(int i = 0; i < 256; i++) 
			{
				glRed[i] = (byte)Math.Round(Compute(i, refHisto.Red));
				glGreen[i] = (byte)Math.Round(Compute(i, refHisto.Green));
				glBlue[i] = (byte)Math.Round(Compute(i, refHisto.Blue));
			}
			byte[] resultRed = GenerateConversion(glRed,
					refHisto.Red.GlobalEqualizedIntensity, 
					cloneHisto.Red.GlobalEqualizedIntensity);
			byte[] resultBlue = GenerateConversion(glBlue,
					refHisto.Blue.GlobalEqualizedIntensity, 
					cloneHisto.Blue.GlobalEqualizedIntensity);
			byte[] resultGreen = GenerateConversion(glGreen, 
					refHisto.Green.GlobalEqualizedIntensity, 
					cloneHisto.Green.GlobalEqualizedIntensity);
			//then apply result to the given image 
			//TODO: See if we need to make a copy image.
			//Is it safe to just overwrite the original image memory?
			for(int i = 0; i < aWidth; i++)
			{
				int[] aSlice = aImage[i];
				int[] q = new int[aHeight];
				for(int j = 0; j < aHeight; j++)
				{
					Color c = Color.FromArgb(aSlice[j]);
					q[j] = Color.FromArgb(255, 
							resultRed[c.R],
							resultGreen[c.G],
							resultBlue[c.B]).ToArgb();
				}
				cImage[i] = q;
			}
			return cImage;
		}
		///<summary>
		///Returns the index of the closest, or exact, value in the target array.
		///</summary>
		private static int ClosestValue(byte value, byte[] elements)
		{
			int max = 0;
			for(int i = 0; i < elements.Length; i++)
			{
				byte curr = elements[i];
				if(curr >= value) 
				{
					//we've either hit the exact value or
					//we've gone "too" far and haven't hit a match
					//which means that any value after this one will be too large as 
					//well 
					return i;
				}
				else
				{
					if(curr > max)
						max = curr;
				}

				//otherwise the value is too large...continue
			}
			//this means that all values are smaller than the target value
			//so bind it to the largest small value there
			return max; 
		}
		private static double Compute(int j, Histogram z)
		{
			return Compute(255, j, z);
		}
		private static double Compute(int largestValue, int j, Histogram z)
		{
			double total = 0.0;			
			double[] pk = z.PK;
			for(int i = 0; i < j; i++)
				total += pk[i];
			return total * largestValue;
		}
	}
}

