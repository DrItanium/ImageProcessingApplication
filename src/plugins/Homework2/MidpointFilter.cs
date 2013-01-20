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
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
      int width = input.Length;
      int height = input[0].Length;
      byte min = (byte)255;
      byte max = (byte)0;
      for(int s = -a; s < a; s++)
      {
        int wX = x + s;
        if(wX < 0 || wX >= width)
          continue;
        for(int t = -b; t < b; t++)
        {
          int wY = y + t;
          if(wY < 0 || wY >= height)
            continue;
          byte value = input[wX][wY];
          max = Math.Max(max, value);
          min = Math.Min(min, value);
        }
      }

      return (byte)((min + max) >> 1);
    }
  }
}
