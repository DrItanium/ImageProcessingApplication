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
  [Filter("Alpha Trimmed Mean Filter")]
    public class AlphaTrimmedMeanFilter : SpatialFilter
  {
    private List<byte> elements;
    protected override string InputFormAddition
    {
      get
      {
        return "label new 13 60 point \"Location\" imbue 63 13 size \"Size\" imbue \"dLabel\" \"Name\" imbue \"D\" \"Text\" imbue \"Controls.Add\" imbue textbox new 80 60 point \"Location\" imbue \"d\" \"Name\" imbue \"Controls.Add\" imbue";
      }
    }
    public AlphaTrimmedMeanFilter(string name) : base(name)
    {
      elements = new List<byte>();
    }
    protected override Hashtable TranslateData_Impl(Hashtable input)
    {
      //use the same hashtable
      input["d"] = decimal.Parse((string)input["d"]);
      return input;
    }
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
      //TODO: Add another stack language for type checking...
      // or just extend off the one we currently have
      int width = input.Length;
      int height = input[0].Length;
      decimal d = (decimal)values["d"];
      elements.Clear();
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
          elements.Add(input[wX][wY]);
        }
      }
      elements.Sort();
      //get d from the input
      try
      {
        int removalFactor = (int)(d / 2.0M);
        //crude but effective
        return (byte)Average(elements.Skip(removalFactor).Reverse().Skip(removalFactor).Reverse());
      }
      catch(DivideByZeroException)
      {
        return input[x][y];
      }
      catch(Exception)
      {
        return input[x][y];
      }
    }
    private static int Average(IEnumerable<byte> b)
    {
      int total = 0;
      int count = 0;
      foreach(var v in b)
      {
        total += v;
        count++;
      }
      return total / count;
    }

  }
}
