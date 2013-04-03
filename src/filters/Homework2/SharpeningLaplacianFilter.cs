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
  [Filter("Sharpening Laplacian Filter")]
    public class SharpeningLaplacianFilter : ImageFilter
  {
    public override string InputForm { get { return null; } }
    public SharpeningLaplacianFilter(string name) : base(name) { }

    public override Hashtable TranslateData(Hashtable input) { return input; }
    public override int[][] TransformImage(Hashtable input)
    {
      int[][] image = (int[][])input["image"];
      int iWidth = image.Length;
      int iHeight = image[0].Length;
      int[][] clone = new int[iWidth][];      
      for(int x = 0; x < iWidth; x++)
      {
        int[] q = new int[iHeight];
        int[] iX = image[x];
        for(int y = 0; y < iHeight; y++)
        {
          Color c = Color.FromArgb(iX[y]);
          int red = c.R;
          int green = c.G;
          int blue = c.B;
          Color result = Laplacian(image, x, y, iWidth, iHeight);
          red += -1 * result.R;
          green += -1 * result.G;
          blue += -1 * result.B;
          q[y] = Color.FromArgb(red < 0 ? 0 : red,
              green < 0 ? 0 : green,
              blue < 0 ? 0 : blue).ToArgb();
        }
        clone[x] = q;
      }
      return clone;
    }
    private static Color Laplacian(int[][] b, int x, int y, int width, int height)
    {
      //fix this up...we don't need to do this every time
      int[] bX = b[x];
      int xM1 = x - 1;
      int xP1 = x + 1;
      int yM1 = y - 1;
      int yP1 = y + 1;
      Color f0 = Color.Black;
      Color f1 = Color.Black;
      Color f2 = Color.Black;
      Color f3 = Color.Black;
      Color f4 = Color.FromArgb(bX[y]);
      int f4Red = (int)(f4.R << 2);
      int f4Green = (int)(f4.G << 2);
      int f4Blue = (int)(f4.B << 2);
      if(xP1 < width) {
        f0Base = Color.FromArgb(b[xP1][y]);
      }
      if(xM1 >= 0) {
        f1Base = Color.FromArgb(b[xM1][y]);
      }
      if(yP1 < height) {
        f2Base = Color.FromArgb(bX[yP1]);
      }
      if(yM1 >= 0) {
        f3Base = Color.FromArgb(bX[yM1]);
      }
      return Color.FromArgb(f0.R + f1.R + f2.R + f3.R - f4Red,
          f0.G + f1.G + f2.G + f3.G - f4Green,
          f0.B + f1.B + f2.B + f3.B - f4Blue);
    }
  }
}
