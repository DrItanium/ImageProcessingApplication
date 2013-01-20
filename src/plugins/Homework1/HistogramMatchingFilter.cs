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
		public class HistogramMatching : Filter 
	{
    public override string InputForm
    {
      get
      {
        return "form new \"Histogram Matching\" \"Text\" imbue label new \"openLabel\" \"Name\" imbue \"Select Image\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue button new \"imageSelector\" \"Name\" imbue \"Open\" \"Text\" imbue 75 23 size \"Size\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue return";
      }
    }
		public HistogramMatching(string name) : base(name) { }
    public override Hashtable TranslateData(Hashtable source)
    {
      //nothing to do
      //Console.WriteLine("Typeof source[\"otherImage\"] = {0}", 
      //    source["otherImage"].GetType());
      return source;
    }
    public override byte[][] Transform(Hashtable source)
    {
      byte[][] aImage = (byte[][])source["image"];
      byte[][] oImage = (byte[][])source["otherImage"];
      int aWidth = aImage.Length;
      int aHeight = aImage[0].Length;
      byte[][] cImage = new byte[aWidth][];
      Histogram cloneHisto = new Histogram(aImage);
      Histogram refHisto = new Histogram(oImage);
			byte[] g = new byte[256];
			for(int i = 0; i < 256; i++) 
			{
				g[i] = (byte)Math.Round(Compute(255, i, refHisto));
			}
			//bind s of cloneHisto to G which maps to z
			byte[] s = cloneHisto.GlobalEqualizedIntensity;
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
				result[j] = refHisto.GlobalEqualizedIntensity[ClosestValue(g[index], 
						refHisto.GlobalEqualizedIntensity)];
			}
			//then apply result to the given image 
			for(int i = 0; i < aWidth; i++)
			{
				byte[] aSlice = aImage[i];
        byte[] q = new byte[aHeight];
				for(int j = 0; j < aHeight; j++)
				{
					q[j] = result[aSlice[j]];
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
				if(elements[i] == value)
					return i;
				else if(elements[i] > value) 
				{
					//we've gone "too" far and haven't hit a match
					//which means that any value after this one will be too large as well 
					return i;
				}
				else
				{
					if(elements[i] > max)
						max = elements[i];
				}
				
				//otherwise the value is too large...continue
			}
			//this means that all values are smaller than the target value
			//so bind it to the largest small value there
			return max; 
		}
		private static double Compute(int largestValue, int j, Histogram z)
		{
			double total = 0.0;			
			for(int i = 0; i < j; i++)
				total += z.PK[i];
			return total * largestValue;
		}

  }
}

