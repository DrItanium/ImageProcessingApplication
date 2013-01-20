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
    public class SharpeningLaplacianFilter : Filter
  {
    public override string InputForm { get { return null; } }
    public SharpeningLaplacianFilter(string name) : base(name) { }

    public override Hashtable TranslateData(Hashtable input) { return input; }
    public override byte[][] Transform(Hashtable input)
    {
      byte[][] image = (byte[][])input["image"];
      int iWidth = image.Length;
      int iHeight = image[0].Length;
      byte[][] clone = new byte[iWidth][];      
      for(int x = 0; x < iWidth; x++)
      {
        clone[x] = new byte[iHeight];
        for(int y = 0; y < iHeight; y++)
        {
          int result = image[x][y];
          result += -1 * Laplacian(image, x, y, iWidth, iHeight);
          clone[x][y] = result < 0 ? (byte)0 : (byte)result;
        }
      }
      return clone;
    }
		 	private static int Laplacian(byte[][] b, int x, int y, int width, int height)
			{
				//fix this up...we don't need to do this every time
				int xM1 = x - 1;
				int xP1 = x + 1;
				int yM1 = y - 1;
				int yP1 = y + 1;
				int f0 = 0; 
				int f1 = 0;
				int f2 = 0;
				int f3 = 0;
				int f4 = (int)(b[x][y]) << 2; //times 4
				if(xP1 < width)
					f0 = b[xP1][y];
				if(xM1 >= 0)
					f1 = b[xM1][y];
				if(yP1 < height)
					f2 = b[x][yP1];
				if(yM1 >= 0)
					f3 = b[x][yM1];
				return f0 + f1 + f2 + f3 - f4;
			}
  }
}
