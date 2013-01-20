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
    public HighBoostingFilter(string name) : base(name) { }
    protected override string InputFormAddition { get { return "label new \"weightLabel\" \"Name\" imbue \"Weight\" \"Text\" imbue 13 72 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"weight\" \"Name\" imbue 80 72 point \"Location\" imbue \"Controls.Add\" imbue"; } }
    protected override Hashtable TranslateData_Impl(Hashtable input)
    {
      input["weight"] = decimal.Parse((string)input["weight"]); 
      return input;
    }
    public override byte[][] Transform(Hashtable input)
    {
      byte[][] image = (byte[][])input["image"];
      int width = image.Length;
      int height = image[0].Length;
      byte[][] resultant = new byte[width][];
      for(int i = 0; i < width; i++)
        resultant[i] = new byte[height];
      //Blur the image
      byte[][] blurred = Blur((int)input["mask"], image);
      decimal weight = (decimal)input["weight"];
      for(int x = 0; x < width; x++)
      {
        for(int y = 0; y < height; y++)
        {
          int originalValue = image[x][y];
          int blurValue = blurred[x][y];

          int maskVal = (int)((decimal)(originalValue - blurValue) * weight);
          int final = originalValue + maskVal;
          if(final < 0)
            final = 0;
          else if(final > 255)
            final = 255;
          resultant[x][y] = (byte)final;
        }
      }
      return resultant;
    }
    protected override byte Operation(int a, int b, int c, int d, byte[][] e, Hashtable f) { return (byte)0; }
    private byte[][] Blur(int maskSize, byte[][] input)
    {
      int width = input.Length;
      int height = input[0].Length;
      byte[][] clone = new byte[width][];
      for(int i = 0; i < width; i++)
      {
        clone[i] = new byte[height];
        for(int j = 0; j < height; j++)
          clone[i][j] = input[i][j];
      }
      int m = maskSize;	
      int a = (m - 1) >> 1;
      int b = a;
      for(int x = 0; x < width; x++)
        for(int y = 0; y < height; y++)
          clone[x][y] = Blur0(a, b, x, y, input);
      return clone;
    }
    private static byte Blur0(int a, int b, int x, int y, byte[][] input)
    {
      int width = input.Length;
      int height = input[0].Length;
      decimal denominator = 0.0M;
      decimal numerator = 0.0M;
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

          int w = input[wX][wY];
          int f = w;
          numerator += (w * f);
          denominator += w;
        }
      }	
      return (byte)((int)(numerator / denominator));
    }
  }
}
