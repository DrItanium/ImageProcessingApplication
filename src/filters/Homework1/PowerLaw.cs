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
  public class PowerTransformation : ImageFilter 
  {
    public PowerTransformation(string name) : base(name) { }
    public override string InputForm { get { return "form new \"Gamma Transformation\" \"Text\" imbue label new \"xLabel\" \"Name\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Gamma\" \"Text\" imbue \"Controls.Add\" imbue label new \"yLabel\" \"Name\" imbue 13 32 point \"Location\" imbue 63 33 size \"Size\" imbue \"Constant\" \"Text\" imbue \"Controls.Add\" imbue textbox new \"gamma\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"constant\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue return"; } }
    public override int[][] TransformImage(Hashtable source)
    {
      if(source == null)
        return null;
      int[][] input = (int[][])source["image"];
			byte[] conversion = (byte[])source["conversion-table"];
      int width = input.Length;
      int height = input[0].Length;
      for(int x = 0; x < width; x++)
      {
				int[] iX = input[x];
        for(int y = 0; y < height; y++)
        {
          Color c = Color.FromArgb(iX[y]);
          iX[y] = Color.FromArgb(255, conversion[c.R],
              conversion[c.G], conversion[c.B]).ToArgb();
        }
      }
      return input;
    }
    public override Hashtable TranslateData(Hashtable source)
    {
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
					source["conversion-table"] = precomputeTable;
          return source;
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
