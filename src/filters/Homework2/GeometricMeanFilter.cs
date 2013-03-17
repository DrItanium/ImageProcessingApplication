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
	[Filter("Geometric Mean Filter")]
		public class GeometricMeanFilter : SpatialFilter
	{
		public GeometricMeanFilter(string name) : base(name) { }
		protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable _)
		{
      int width = input.Length;
      int height = input[0].Length;
			int total = 1;
			int size = 0;
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
					total *= iX[wY];
					size++;
				}
			}
			double power = (1.0 / (double)(size));		
			double result = Math.Pow(total, power);
			return (byte)result;
		}
	}
}
