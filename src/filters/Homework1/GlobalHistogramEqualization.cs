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
using System.Collections;


namespace CS555.Homework1
{
	[Filter("Global Histogram Equalization")]
		public class GlobalHistogramEqualization : ImageFilter
	{
		public override string InputForm { get { return null; } }
		public GlobalHistogramEqualization(string name) : base(name) { }

		public override Hashtable TranslateData(Hashtable source)
		{
			return source;
		}
		public override int[][] TransformImage(Hashtable source)
		{
			if(source == null)
				return null; 
			else
			{
				int[][] input = (int[][])source["image"];
				//the histogram object automatically creates a pixel intensity
				//distribution upon creation. It also computes the global equalized
				//intensity based upon it's frequency in the given image. 
				ColorHistogram ch = new ColorHistogram(input);
				for(int x = 0; x < input.Length; x++)
				{
					int[] line = input[x];
					for(int y = 0; y < input[y].Length; y++)
					{
						//we just replace the previous pixels with the new ones 
						Color c = Color.FromArgb(line[y]);
						line[y] = Color.FromArgb(255,ch.Red.GlobalEqualizedIntensity[c.R],
								ch.Green.GlobalEqualizedIntensity[c.G],
								ch.Blue.GlobalEqualizedIntensity[c.B]).ToArgb();
					}
				}
				return input;
			}
		}
	}
}
