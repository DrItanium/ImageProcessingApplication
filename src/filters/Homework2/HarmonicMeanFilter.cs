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
    private static double[] translationTable = new double[256];
    static HarmonicMeanFilter() 
    {
      for(int i = 0; i < 256; i++) 
      {
        translationTable[i] = 1.0 / (double)i;
      }
    }
    public HarmonicMeanFilter(string name) : base(name) { }
    protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable elements)
    {
      int width = input.Length;
      int height = input[0].Length;
      double totalRed = 0.0,
             totalGreen = 0.0,
             totalBlue = 0.0;
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
          if(c.R == c.B && c.B == c.G) 
          {
            double value = translationTable[c.R];
            totalRed += value;
            totalBlue += value;
            totalGreen += value;
          }
          else
          {
            totalRed += translationTable[c.R];
            totalBlue += translationTable[c.B];
            totalGreen += translationTable[c.G];
          }
          size++;
        }
      }
      if(totalRed == totalBlue && totalBlue == totalGreen) 
      {
        byte result = (byte)(((double)size) / total);
        return Color.FromArgb(255, result, result, result).ToArgb();
      }
      else
      {
        return Color.FromArgb(255, (byte)(((double)size) / totalRed),
            (byte)(((double)size) / totalGreen),
            (byte)(((double)size) / totalBlue)).ToArgb();
      }
    }
  }
}
