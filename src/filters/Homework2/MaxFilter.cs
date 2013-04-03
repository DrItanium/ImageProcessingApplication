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
  [Filter("Max Filter")]		
    public class MaxFilter : SpatialFilter 
  {
    public MaxFilter(string name) : base(name) { }
    protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable values)
    {
      int width = input.Length;
      int height = input[0].Length;
      byte maxR = (byte)0,
           maxG = (byte)0,
           maxB = (byte)0;
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
          Color c = Color.FromArgb(iX[wY]);
          if(c.R == c.B && c.B == c.G) 
          {
            int value = c.R;
            maxR = Math.Max(maxR, value);
            maxG = Math.Max(maxG, value);
            maxB = Math.Max(maxB, value);
          }
          else
          {
            maxR = Math.Max(maxR, c.R);
            maxG = Math.Max(maxG, c.G);
            maxB = Math.Max(maxB, c.B);
          }
        }
      }
      return Color.FromArgb(maxR, maxG, maxB).ToArgb();
    }
  }
}
