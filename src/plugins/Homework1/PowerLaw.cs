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
      double gamma = (double)source["gamma"];
      double r = (double)source["constant"];
      byte[][] clone = new byte[input.Length][];
      for(int x = 0; x < input.Length; x++)
      {
        clone[x] = new byte[input[x].Length];
        for(int y = 0; y < input[x].Length; y++)
        {
          clone[x][y] = (byte)(input[x][y] * Math.Pow((double)r, gamma));
        }
      }
      return clone;
    }
    public override Hashtable TranslateData(Hashtable source)
    {
      Hashtable output = new Hashtable();
      output["image"] = source["image"];
      double nX, nY;
      bool result0 = double.TryParse(source["gamma"].ToString(), out nX);
      bool result1 = double.TryParse(source["constant"].ToString(), out nY);
      if(result0 && result1)
      {

        if(nX >= 0.0 && nY >= 0.0)
        {
          output["gamma"] = nX;
          output["constant"] = nY;
          return output;
        }
        else
        {
          if(nX < 0.0)
            MessageBox.Show("Gamma is Less than zero");
          if(nY < 0.0)
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
