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
		[Filter("Arithmetic Mean Filter")]		
			public class ArithmeticMeanFilter : SpatialFilter 
		{
			public ArithmeticMeanFilter(string name) : base(name) { }
      protected override string InputFormAddition { get { return string.Empty; } }
      protected override Hashtable TranslateData_Impl(Hashtable input) { return input; }
			protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
			{
        int width = input.Length;
        int height = input[0].Length;
				int count = 0;
				int total = 0;
        int m = (a + 1) << 1;
        int n = (b + 1) << 1;
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
						total += iX[wY];
						count++; //this will probably brighten the image
					}
				}
        if(m == n && m == 1)
          return (byte)total;
        else
				  return (byte)(total / count);
			}
    }
}
