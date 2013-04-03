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

/* This filter performs a logarithm transformation on a given image.
 *
 */
namespace CS555.Homework1
{
  [Filter("Log Transformation")]
  public class LogarithmTransformation : ImageFilter
  {
    public override string InputForm { get { return "form new label new \"ConstantLabel\" \"Name\" imbue \"Constant\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"constant\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue return"; } }
    public LogarithmTransformation(string name) : base(name) { }

    public override int[][] TransformImage(Hashtable source)
    {
      if(source == null)
        return null;
      else
      {
        int[][] input = (int[][])source["image"];
        double constant = (double)source["constant"];
				byte[] valueTable = new byte[256];
				int ixLength = input[0].Length;
				for(int i = 0; i < 256; i++) 
				{
					valueTable[i] = ComputeValue(constant, (byte)i);
				}
        for(int x = 0; x < input.Length; x++)
        {
					int[] iX = input[x];
          for(int y = 0; y < ixLength; y++) 
					{
            Color c = Color.FromArgb(iX[y]);
            iX[y] = Color.FromArgb(255, valueTable[c.R],
                valueTable[c.G], valueTable[c.B]).ToArgb();
					}
        }
        return input;
      }
    }
    protected static byte ComputeValue(double c, byte r)
    {
      return (byte)(c * Math.Log(1.0 + (double)r));
    }
    public override Hashtable TranslateData(Hashtable source)
    {
      double target;
      bool result = double.TryParse((string)source["constant"], out target);
      if(!result)
      {
        MessageBox.Show("Invalid Bit Depth Provided");
        return null;
      }
      else
      {
        source["constant"] = target;
        return source;
      }
    }
  }

}
