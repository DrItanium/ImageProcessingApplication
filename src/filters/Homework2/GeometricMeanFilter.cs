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
    protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable _)
    {
      int width = input.Length;
      int height = input[0].Length;
      int totalRed = 1,
          totalGreen = 1,
          totalBlue = 1;
      int size = 0;
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
          totalRed *= c.R;
          totalBlue *= c.B;
          totalGreen *= c.G;
          size++;
        }
      }
      double power = (1.0 / (double)(size));		
      if(totalRed == totalBlue && totalBlue == totalGreen) 
      {
        int result = (int)Math.Pow(totalRed, power);
        return Color.FromArgb(result, result, result).ToArgb();
      }
      else
      {
        return Color.FromArgb((int)Math.Pow(totalRed, power),
            (int)Math.Pow(totalGreen, power),
            (int)Math.Pow(totalBlue, power)).ToArgb();
      }
    }
  }
}
