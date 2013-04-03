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
  [Filter("Midpoint Filter")]
    public class MidpointFilter : SpatialFilter
  {
    public MidpointFilter(string name) : base(name) { }
    protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable values)
    {
      int width = input.Length;
      int height = input[0].Length;
      byte minRed = (byte)255,
           minGreen = (byte)255,
           minBlue = (byte)255,
           maxRed = (byte)0,
           maxGreen = (byte)0,
           maxBlue = (byte)0;
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
          maxRed = Math.Max(maxRed, c.R); 
          maxGreen = Math.Max(maxGreen, c.G); 
          maxBlue = Math.Max(maxBlue, c.B); 
          minRed = Math.Min(minRed, c.R); 
          minGreen = Math.Min(minGreen, c.G); 
          minBlue = Math.Min(minBlue, c.B); 
        }
      }
      return Color.FromArgb(255, ((minRed + maxRed) >> 1),
          ((minGreen + maxGreen) >> 1),
          ((minBlue + maxBlue) >> 1)).ToArgb();
    }
  }
}
