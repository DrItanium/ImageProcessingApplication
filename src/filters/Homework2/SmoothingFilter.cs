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
  [Filter("Smoothing Filter")]
    public class SmoothingFilter : SpatialFilter
  {
		private static int[] square = new int[256];
		static SmoothingFilter() 
		{
			for(int i = 0; i < 256; i++) 
			{
				square[i] = i * i;
			}
		}
    protected override string InputFormAddition{ get { return string.Empty; } }
    public SmoothingFilter(string name) : base(name) { }

    protected override Hashtable TranslateData_Impl(Hashtable input)
    {
      return input;
    }
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
      int height = input[0].Length;
      int width = input.Length;
      double denominator = 0.0f;
      double numerator = 0.0f;
      for(int t = -b; t < b; t++)
      {
        int wY = y + t;
        if(wY < 0 || wY >= height)
          continue;
        for(int s = -a; s < a; s++)
        {
          int wX = x + s;
          if(wX < 0 || wX >= width)
            continue;
          int w = input[wX][wY];
          numerator += square[w];
          denominator += w;
        }
      }	
      return (denominator == 0.0f) ? (byte)((int)numerator) : (byte)((int)(numerator / denominator));
    }
  }
}
