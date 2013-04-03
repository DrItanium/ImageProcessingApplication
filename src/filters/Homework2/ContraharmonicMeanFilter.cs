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
      double q = double.Parse((string)input["q"]);
      double[] numerator = new double[256];
      double[] denominator = new double[256];
      for(int i = 0; i < 256; i++) 
      {
        denominator[i] = Math.Pow((double)i, q);
        numerator[i] = Math.Pow((double)i, q + 1.0);
      }
      input["numerator"] = numerator;
      input["denominator"] = denominator;
      input["q"] = q;
      return input;
    }
    protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable values)
    {
      double[] numerator = (double[])values["numerator"];
      double[] denominator = (double[])values["denominator"];
      int width = input.Length;
      int height = input[0].Length;
      double totalNumeratorRed = 0.0, 
             totalNumeratorGreen = 0.0, 
             totalNumeratorBlue = 0.0, 
             totalDenominatorRed = 0.0, 
             totalDenominatorGreen = 0.0, 
             totalDenominatorBlue = 0.0;
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
          byte red = c.R;
          byte green = c.G;
          byte blue = c.B;
          totalDenominatorRed += denominator[red];
          totalDenominatorGreen += denominator[green];
          totalDenominatorBlue += denominator[blue];
          totalNumeratorRed += numerator[red];
          totalNumeratorGreen += numerator[green];
          totalNumeratorBlue += numerator[blue];
        }
      }
      return Color.FromArgb((byte)(totalNumeratorRed / totalDenominatorRed),
          (byte)(totalNumeratorGreen / totalDenominatorGreen),
          (byte)(totalNumeratorBlue / totalDenominatorBlue));
    }
  }
}
