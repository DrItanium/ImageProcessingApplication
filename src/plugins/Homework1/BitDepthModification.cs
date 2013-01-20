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
using Frameworks.Plugin;
using System.Collections;

namespace CS555.Homework1
{
  [Filter("Change Bit Depth")]
  public class BitDepthTransformation : Filter
  {
    private byte[,] translationMatrix;
    public BitDepthTransformation(string name) : base(name) 
    {
      translationMatrix = new byte[8,256];
      SetupMatrix();
    }
    public override string InputForm { get { return "form new \"Bit Depth Modification\" \"Text\" imbue label new \"bitDepthLabel\" \"Name\" imbue \"Bit Depth\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"depth\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue return"; } }
    private void SetupMatrix()
    {
      //I manually perform this for 1 bit because the formula I use doesn't
      //handle 1-bit at all....it produces a pure black image.
      SetSection(0, 0, 128, (byte)0);
      SetSection(0, 128, 256, (byte)255);
      for(int i = 1; i < 8; i++)
      {
        int power = (int)Math.Pow(2, i + 1);
        int sd = (int)Math.Floor((256.0f / (float)(power - 1)));
        int sep = 256 / power;
        for(int j = 0; j < power; j++) //separation areas
          SetSection(i, j * sep, (j + 1) * sep, (byte)(sd * j));
      }
    }
    private void SetSection(int depth, int from, int to, byte value)
    {
      for(int i = from; i < to; i++)
      {
        translationMatrix[depth, i] = value; 
      }
    }
    //TODO: Make it so that the source coming in is already checked.
    //      This will require another language
    public override Hashtable TranslateData(Hashtable source)
    {
      Hashtable output = new Hashtable();
      output["image"] = source["image"];
      int target;
      bool result = int.TryParse((string)source["depth"], out target);
      if(!result || (target < 0 && target > 8))
      {
        MessageBox.Show("Invalid Bit Depth Provided");
        return null;
      }
      else
      {
        output["depth"] = target;
        return output;
      }
    }
    public override byte[][] Transform(Hashtable source)
    {
      if(source == null)
        return null;
      byte[][] input = (byte[][])source["image"];
      int depth = (int)source["depth"];
      byte[][] clone = new byte[input.Length][];
      for(int x = 0; x < input.Length; x++)
      {
        clone[x] = new byte[input[x].Length];
        for(int y = 0; y < input[x].Length; y++)
        {
          clone[x][y] = translationMatrix[depth - 1, input[x][y]];
        }
      }
      return clone;
    }
  }
}
