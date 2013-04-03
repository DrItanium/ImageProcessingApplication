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

/* 
 * Local histogram equalization defines a mask that represents a section of
 * pixels around each target pixel that is used to equalize the intensity of
 * that pixel. This is a destructive operation and so updates made to pixels
 * are taken into account for pixels following it
 *
 */

namespace CS555.Homework1
{
	[Filter("Local Histogram Equalization")]
		public class LocalHistogramEqualization : ImageFilter
	{
		public override string InputForm { get { return "form new \"Local Histogram Equalization\" \"Text\" imbue label new \"Mask Size\" \"Text\" imbue \"maskSizeLabel\" \"Name\" imbue 63 13 size \"Size\" imbue 13 35 point \"Location\" imbue \"Controls.Add\" imbue combobox new \"mask\" \"Name\" imbue 80 35 point \"Location\" imbue 75 21 size \"Size\" imbue \"3x3\" \"Items.Add\" imbue \"5x5\" \"Items.Add\" imbue \"7x7\" \"Items.Add\" imbue \"9x9\" \"Items.Add\" imbue \"11x11\" \"Items.Add\" imbue \"13x13\" \"Items.Add\" imbue \"15x15\" \"Items.Add\" imbue \"17x17\" \"Items.Add\" imbue \"19x19\" \"Items.Add\" imbue \"21x21\" \"Items.Add\" imbue \"Controls.Add\" imbue return"; } }
		public LocalHistogramEqualization(string name) : base(name) { }

		public override Hashtable TranslateData(Hashtable source)
		{
			string maskSize = (string)source["mask"];
			switch(maskSize)
			{
				case "3x3":
					source["mask"] = 3;
					break;
				case "5x5":
					source["mask"] = 5;
					break;
				case "7x7":
					source["mask"] = 7;
					break;
				case "9x9":
					source["mask"] = 9;
					break;
				case "11x11":
					source["mask"] = 11;
					break;
				case "13x13":
					source["mask"] = 13;
					break;
				case "15x15":
					source["mask"] = 15;
					break;
				case "17x17":
					source["mask"] = 17;
					break;
				case "19x19":
					source["mask"] = 19;
					break;
				case "21x21":
					source["mask"] = 21;
					break;
				default:
					source["mask"] = 3;
					break;
			}
			return source;
		}
		public override int[][] TransformImage(Hashtable source)
		{
			if(source == null)
				return null; 
			else
			{
				int[][] input = (int[][])source["image"];
				int mask = (int)source["mask"];
				double count = (double)(255.0 / (mask * mask)); //square mask
        ColorHistogram h = new ColorHistogram(mask, mask);
				int width = input.Length;
				int height = input[0].Length;
        Func<Histogram, int, byte> fn = (hi,current) => (byte)(count * (double)hi[0, current + 1]);
        Func<int,int,IEnumerable<int>> grabNeighborhood = (px, py) => input.GrabNeighborhood(px, py, width, height, mask);
				for(int x = 0; x < width; x++)
				{
					int[] iX = input[x];
          Func<int,IEnumerable<int>> gX = (py) => grabNeighborhood(x,py);
					//close over these values 
					for(int y = 0; y < height; y++)
					{
            Color c = Color.FromArgb(iX[y]);
            h.Repurpose(gX(y));
            iX[y] = Color.FromArgb(255, fn(h.Red, (int)c.R),
                                        fn(h.Green, (int)c.G),
                                        fn(h.Blue, (int)c.B)).ToArgb();
					}
				}
				return input;
			}
		}
	}
}
