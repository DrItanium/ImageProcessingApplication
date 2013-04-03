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
  [Filter("High-Boosting Filter")]
    public class HighBoostingFilter : SpatialFilter
  {
    private static int[] square = new int[256];
    static HighBoostingFilter() 
    {
      for(int i = 0; i < 256; i++)
      {
        square[i] = (i * i);
      }
    }
    public HighBoostingFilter(string name) : base(name) { }
    protected override string InputFormAddition { get { return "label new \"weightLabel\" \"Name\" imbue \"Weight\" \"Text\" imbue 13 72 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"weight\" \"Name\" imbue 80 72 point \"Location\" imbue \"Controls.Add\" imbue"; } }
    protected override Hashtable TranslateData_Impl(Hashtable input)
    {
      input["weight"] = decimal.Parse((string)input["weight"]); 
      return input;
    }
    private static int Chop(int value)
    {
      if(value < 0)
      {
        return 0;
      }
      else if(value > 255) 
      {
        return 255;
      }
      else
      {
        return value;
      }
    }
    public override int[][] TransformImage(Hashtable input)
    {
      int[][] image = (int[][])input["image"];
      int width = image.Length;
      int height = image[0].Length;
      int[][] resultant = new int[width][];
      //Blur the image
      int[][] blurred = Blur((int)input["mask"], image);
      decimal weight = (decimal)input["weight"];
      for(int x = 0; x < width; x++)
      {
        int[] iX = image[x];
        int[] bX = blurred[x];
        int[] rX = new int[height];
        for(int y = 0; y < height; y++)
        {
          Color originalValue = Color.FromArgb(iX[y]);
          Color blurValue = Color.FromArgb(bX[y]);
          int maskValueRed = (int)((decimal)(originalValue.R - blurValue.R) * weight);
          int maskValueGreen = (int)((decimal)(originalValue.G - blurValue.G) * weight);
          int maskValueBlue = (int)((decimal)(originalValue.B - blurValue.B) * weight);
          int finalRed = Chop(originalValue.R + maskValueRed);
          int finalBlue = Chop(originalValue.B + maskValueBlue);
          int finalGreen = Chop(originalValue.G + maskValueGreen);
          rX[y] = Color.FromArgb(255, finalRed, finalGreen, finalBlue).ToArgb();
        }
        resultant[x] = rX;
      }
      return resultant;
    }
    protected override int Operation(int a, int b, int c, int d, int[][] e, Hashtable f) { return (byte)0; }
    private int[][] Blur(int maskSize, int[][] input)
    {
      int width = input.Length;
      int height = input[0].Length;
      int[][] clone = new int[width][];
      int m = maskSize;	
      int a = (m - 1) >> 1;
      int b = a;
      for(int x = 0; x < width; x++)
      {
        int[] z = new int[height];
        for(int y = 0; y < height; y++) 
        {
          z[y] = Blur0(a, b, x, y, input);
        }
        clone[x] = z;
      }
      return clone;
    }
    private static int Blur0(int a, int b, int x, int y, int[][] input)
    {
      int width = input.Length;
      int height = input[0].Length;
      decimal denominatorRed = 0.0M,
              denominatorGreen = 0.0M,
              denominatorBlue = 0.0M,
              numeratorRed = 0.0M,
              numeratorGreen = 0.0M,
              numeratorBlue = 0.0M;
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
          //get rid of f because we can do the same thing with a memoized table
          //This removes a ton of potential multiplies in really large images
          //
          //This is possible because f was always equal to w and since w is
          //always between 0 and 255, we can save a lot of time by storing the
          //results in an array    
          if(c.R == c.B && c.B == c.G) 
          {
            int n = square[c.R];
            int m = c.R;
            numeratorRed += n;
            numeratorBlue += n;
            numeratorGreen += n;
            denominatorBlue += m;
            denominatorGreen += m;
            denominatorRed += m;
          }
          else 
          {
            numeratorRed += square[c.R];
            numeratorBlue += square[c.B];
            numeratorGreen += square[c.G];
            denominatorRed += c.R;
            denominatorGreen += c.G;
            denominatorBlue += c.B;
          }
        }
      }	
      return Color.FromArgb(255, (int)(numeratorRed / denominatorRed),
          (int)(numeratorGreen / denominatorGreen),
          (int)(numeratorBlue / denominatorBlue)).ToArgb();
    }
  }
}
