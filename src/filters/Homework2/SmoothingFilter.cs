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

    protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable values)
    {
      int height = input[0].Length;
      int width = input.Length;
      double denominatorRed = 0.0,
             denominatorGreen = 0.0,
             denominatorBlue = 0.0,
             numeratorRed = 0.0,
             numeratorGreen = 0.0,
             numeratorBlue = 0.0;
      for(int t = -b; t < b; t++)
      {
        int wY = y + t;
        if(wY < 0 || wY >= height)
          continue;
        int[] iX = input[wX];
        for(int s = -a; s < a; s++)
        {
          int wX = x + s;
          if(wX < 0 || wX >= width)
            continue;
          Color w = Color.FromArgb(iX[wY]);
          if(w.R == w.G && w.G == w.B) 
          {
            int vN = square[w.R];
            int vD = w.R;
            numeratorRed += vN;
            numeratorGreen += vN;
            numeratorBlue += vN;
            denominatorRed += vD;
            denominatorGreen += vD;
            denominatorBlue += vD;
          }
          else 
          {
            numeratorRed += square[w.R];
            numeratorGreen += square[w.G];
            numeratorBlue += square[w.B];
            denominatorRed += w.R;
            denominatorGreen += w.G;
            denominatorBlue += w.B;
          }
        }
      }	
      return Color.FromArgb(
          (denominatorRed == 0.0) ? (int)numeratorRed : (int)(numeratorRed / denominatorRed),
          (denominatorGreen == 0.0) ? (int)numeratorGreen : (int)(numeratorGreen / denominatorGreen),
          (denominatorBlue == 0.0) ? (int)numeratorBlue : (int)(numeratorBlue / denominatorBlue)).ToArgb();
    }
  }
}
