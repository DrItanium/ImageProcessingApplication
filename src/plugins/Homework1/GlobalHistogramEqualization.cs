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
using System.Collections;


namespace CS555.Homework1
{
  [Filter("Global Histogram Equalization")]
    public class GlobalHistogramEqualization : Filter
  {
    public override string InputForm { get { return null; } }
    public GlobalHistogramEqualization(string name) : base(name) { }

    public override Hashtable TranslateData(Hashtable source)
    {
      return source;
    }
    public override byte[][] Transform(Hashtable source)
    {
      if(source == null)
        return null; 
      else
      {
        byte[][] input = (byte[][])source["image"];
        Histogram h = new Histogram(input);
        for(int x = 0; x < input.Length; x++)
        {
          for(int y = 0; y < input[y].Length; y++)
          {
            input[x][y] = Translate(input[x][y], h);
          }
        }
        return input;
      }
    }
    private static byte Translate(byte data, Histogram histogram)
    {
      return histogram.GlobalEqualizedIntensity[data];
    }
  }
}
