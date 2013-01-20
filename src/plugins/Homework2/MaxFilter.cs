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
    public MaxFilter(string name) : base(name) 
    {
    }
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
      int width = input.Length;
      int height = input[0].Length;
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
          max = Math.Max(max, input[wX][wY]);
        }
      }
      return max;
    }
  }
}
