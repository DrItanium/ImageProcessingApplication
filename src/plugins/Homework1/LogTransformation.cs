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
  [Filter("Log Transformation")]
  public class LogarithmTransformation : Filter
  {
    public override string InputForm { get { return "form new label new \"ConstantLabel\" \"Name\" imbue \"Constant\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"constant\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue return"; } }
    public LogarithmTransformation(string name) : base(name) { }

    public override byte[][] Transform(Hashtable source)
    {
      if(source == null)
        return null;
      else
      {
        byte[][] input = (byte[][])source["image"];
        double constant = (double)source["constant"];
        byte[][] clone = new byte[input.Length][];
        for(int x = 0; x < input.Length; x++)
        {
          clone[x] = new byte[input[x].Length];
          for(int y = 0; y < input[x].Length; y++)
            clone[x][y] = ComputeValue(constant, input[x][y]); 
        }
        return clone;
      }
    }
    protected static byte ComputeValue(double c, byte r)
    {
      return (byte)(c * Math.Log(1.0 + (double)r));
    }
    public override Hashtable TranslateData(Hashtable source)
    {
      Hashtable output = new Hashtable();
      output["image"] = source["image"];
      double target;
      bool result = double.TryParse((string)source["constant"], out target);
      if(!result)
      {
        MessageBox.Show("Invalid Bit Depth Provided");
        return null;
      }
      else
      {
        output["constant"] = target;
        return output;
      }
    }
  }

}
