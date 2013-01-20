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
  [Filter("Contraharmonic Filter")]
    public class ContraharmonicMeanFilter : SpatialFilter
  {
    public ContraharmonicMeanFilter(string name) : base(name) { }
    protected override string InputFormAddition { get { return "label new \"weightLabel\" \"Name\" imbue \"Order\" \"Text\" imbue 13 62 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"q\" \"Name\" imbue 80 62 point \"Location\" imbue \"Controls.Add\" imbue"; } }
    protected override Hashtable TranslateData_Impl(Hashtable input)
    {
      input["q"] = double.Parse((string)input["q"]);
      return input;
    }
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
      double q = (double)values["q"];
      int width = input.Length;
      int height = input[0].Length;
      double totalNumerator = 0;	
      double totalDenominator = 0;
      //int size = 0;
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
          double value = input[wX][wY];
          totalDenominator += Math.Pow(value, (double)q);	
          totalNumerator += Math.Pow(value, (double)q + 1.0);
        }
      }
      int result = (int)(totalNumerator / totalDenominator);
      return (byte)result;
    }
  }
}
