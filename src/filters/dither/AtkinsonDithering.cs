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


namespace Filters.Dithering 
{
  ///<summary>
  /// The dithering algorithm invented by Bill Atkinson for displaying images
  /// on the original Apple Macintosh. This filter will make the image able to
  /// be shown on an original 128k Mac. 
  ///</summary>
  [Filter("Atkinson Dithering")]
    public class AtkinsonDithering : Filter 
  {
    private static byte[] thresholdTable;
    static AtkinsonDithering() 
    {
      thresholdTable = new byte[256];
      for(int i = 128; i < 256; i++) 
      {
        thresholdTable[i] = (byte)255;
      }
    }
    public override string InputForm { get { return null; } }
    public AtkinsonDithering(string name) : base(name) { }

    public override Hashtable TranslateData(Hashtable input)
    {
      return input;
    }
    private static bool InRange(int x, int y, int width, int height)
    {
      return (x >= 0 && x < width) && (y >= 0 && y < height);
    }
    public override byte[][] Transform(Hashtable input) 
    {
      //if the intensity is less than 128 then return 0
      //else return black
      byte[][] image = (byte[][])input["image"];
      int width = image.Length;
      int height = image[0].Length;
      Func<int,int,bool> check = (a,b) => InRange(a,b,width,height);
      Action<int,int,byte> checkSet = (a,b,v) => {
        if(check(a,b)) {
          image[a][b] = image[a][b] + v;
        }
      };
      for(int j = 0; j < height; j++)
      {
        int y0 = j;
        int y1 = y0 + 1;
        int y2 = y0 + 2;
        for(int i = 0; i < width; i++)
        {
          byte oldIntensity = image[i][j];
          byte newIntensity = threshold[oldIntensity];
          byte error = (byte)((oldIntensity - newIntensity) >> 3);
          Action<int,int> cs = (a,b) => checkSet(a,b,error);
          image[i][j] = newIntensity;
          //compute the offsets
          int x0 = i;
          int x1 = x0 + 1;
          cs(x1,y0);
          cs(x0 + 2,y0);
          cs(x0 - 1,y1);
          cs(x0,y1);
          cs(x1,y1);
          cs(x0,y2);
        }
      }
      return image;
    }
  }
}
