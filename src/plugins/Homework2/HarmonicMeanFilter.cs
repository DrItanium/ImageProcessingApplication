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
  [Filter("Harmonic Mean Filter")]		
    public class HarmonicMeanFilter : SpatialFilter 
  {
    public HarmonicMeanFilter(string name) : base(name) { }
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable elements)
    {
      int width = input.Length;
      int height = input[0].Length;
      double total = 0;
      int size = 0;
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
          double value = input[wX][wY];
          total += (1.0 / value);	
          size++;
        }
      }
      return (byte)(((double)size) / total);
    }
  }
}
