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
	[Filter("Alpha Trimmed Mean Filter")]
		public class AlphaTrimmedMeanFilter : SpatialFilter
	{
		private List<byte> redElements;
		private List<byte> blueElements;
		private List<byte> greenElements;
		private int removalFactor;
		protected override string InputFormAddition
		{
			get
			{
				return "label new 13 60 point \"Location\" imbue 63 13 size \"Size\" imbue \"dLabel\" \"Name\" imbue \"D\" \"Text\" imbue \"Controls.Add\" imbue textbox new 80 60 point \"Location\" imbue \"d\" \"Name\" imbue \"Controls.Add\" imbue";
			}
		}
		public AlphaTrimmedMeanFilter(string name) : base(name)
		{
			redElements = new List<byte>();
			blueElements = new List<byte>();
			greenElements = new List<byte>();
		}
		protected override Hashtable TranslateData_Impl(Hashtable input)
		{
			//use the same hashtable
			decimal val = decimal.Parse((string)input["d"]);
			removalFactor = (int)(val / 2.0M);
			input["d"] = val;
			return input;
		}
		protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable values)
		{
			//TODO: Add another stack language for type checking...
			// or just extend off the one we currently have
			int width = input.Length;
			int height = input[0].Length;
      redElements.Clear();
      blueElements.Clear();
      greenElements.Clear();
			for(int s = -a; s < a; s++)
			{
				int wX = x + s;
				if(wX < 0 || wX >= width)
					continue;
				int[] iX = input[wX];
				for(int t = -b; t < b; t++)
				{
					int wY = y + t;
					if(wY < 0 || wY >= height)
						continue;
          Color c = Color.FromArgb(iX[wY]);
          redElements.Add(c.R);
          blueElements.Add(c.B);
          greenElements.Add(c.G);
				}
			}
      redElements.Sort();
      blueElements.Sort();
      greenElements.Sort();
			try
			{
        return Color.FromArgb(255,
            Average(redElements),
            Average(greenElements),
            Average(blueElements)).ToArgb();
			}
			catch(Exception)
			{
				return input[x][y];
			}
		}
		private byte Average(List<byte> elements) 
		{
			int start = removalFactor;
			int finish = elements.Count - removalFactor;
			int total = 0;
			int count = finish - start;
			for(int i = start; i < finish; i++)
			{
				total += elements[i];
			}
			return (byte)(total / count);
		}
	}
}
