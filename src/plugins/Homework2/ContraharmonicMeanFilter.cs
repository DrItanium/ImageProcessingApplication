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
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
			double[] numerator = (double[])values["numerator"];
			double[] denominator = (double[])values["denominator"];
      int width = input.Length;
      int height = input[0].Length;
      double totalNumerator = 0.0;	
      double totalDenominator = 0.0;
      for(int s = -a; s < a; s++)
      {
        int wX = x + s;
        if(wX < 0 || wX >= width)
          continue;
				byte[] iX = input[wX];
        for(int t = -b; t < b; t++)
        {
          int wY = y + t;
          if(wY < 0 || wY >= height)
            continue;
					byte index = iX[wY];
					totalDenominator += denominator[index];
					totalNumerator += numerator[index];
        }
      }
      return (byte)(totalNumerator / totalDenominator);
    }
  }
}
