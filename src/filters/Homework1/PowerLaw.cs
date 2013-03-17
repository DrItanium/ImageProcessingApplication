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
using System.Collections;

namespace CS555.Homework1
{
  [Filter("Gamma Transformation")]
  public class PowerTransformation : Filter 
  {
    public PowerTransformation(string name) : base(name) { }
    public override string InputForm { get { return "form new \"Gamma Transformation\" \"Text\" imbue label new \"xLabel\" \"Name\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Gamma\" \"Text\" imbue \"Controls.Add\" imbue label new \"yLabel\" \"Name\" imbue 13 32 point \"Location\" imbue 63 33 size \"Size\" imbue \"Constant\" \"Text\" imbue \"Controls.Add\" imbue textbox new \"gamma\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"constant\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue return"; } }
    public override byte[][] Transform(Hashtable source)
    {
      if(source == null)
        return null;
      byte[][] input = (byte[][])source["image"];
			byte[] conversion = (byte[])source["conversion-table"];
      byte[][] clone = new byte[input.Length][];
			int ixl = input[0].Length;
      for(int x = 0; x < input.Length; x++)
      {
				byte[] iX = input[x];
				byte[] q = new byte[ixl];
        for(int y = 0; y < ixl; y++)
        {
					q[y] = conversion[iX[y]];
        }
				clone[x] = q;
      }
      return clone;
    }
    public override Hashtable TranslateData(Hashtable source)
    {
      Hashtable output = new Hashtable();
      output["image"] = source["image"];
      double gamma, constant;
      bool result0 = double.TryParse(source["gamma"].ToString(), out gamma);
      bool result1 = double.TryParse(source["constant"].ToString(), out constant);
      if(result0 && result1)
      {
        if(gamma >= 0.0 && constant >= 0.0)
        {
					//precompute the results
					byte[] precomputeTable = new byte[256];
					double factor = Math.Pow(constant, gamma);
					precomputeTable[0] = (byte)0;
					precomputeTable[1] = (byte)factor;
					for(int i = 2; i < 256; i++) 
					{
						precomputeTable[i] = (byte)(i * factor);
					}
					output["conversion-table"] = precomputeTable;
          return output;
        }
        else
        {
          if(gamma < 0.0)
            MessageBox.Show("Gamma is Less than zero");
          if(constant < 0.0)
            MessageBox.Show("Constant is Less Than Zero");
          return null;
        }
      }
      else
      {
        if(!result0)
          MessageBox.Show("Invalid Input for Gamma");
        if(!result1)
          MessageBox.Show("Invalid Input for Constant");
        return null;;
      }
    }
  }

}
