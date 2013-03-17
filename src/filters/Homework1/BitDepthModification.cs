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
/* This filter is responsible for changing the bit depth of a given byte image
 * to a specified bitwidth.
 *
 * For instance, if we have an 8-bit and we want a 5-bit image then we convert
 * all 8-bit values to 5-bit values. The value still remains within a byte.
 * However, the values stored have been transformed to fit within the specified
 * bit width. 
 *
 * This is done through the use of a translation matrix that consists of 8*256
 * bytes. It is a multidimensional array instead of a jagged one. Currently,
 * each instance of this filter has its own copy. 
 *
 * The way this translation matrix works is quite simple. Each bit depth
 * contains 256 entries in the translation matrix. The original intensity is
 * the index into the set of entries with the value stored at that index being
 * the 8-bit intensity for the corresponding value at the target bit depth. We
 * use the concept of duplication to simulate the reduction in representable
 * values. 
 *
 */
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
				//compute the largest value at the corresponding bit depth +1
				//So at seven bits we return 256 as that's the next largest bit depth
        int power = (int)Math.Pow(2, i + 1);
				//figure out reduction factor by dividing 256.0 (largest for 8-bits)
				//by the largest value expressible at the target bit depth (power - 1)
				//The floor of this computation will be the conversion factor to use 
        int sd = (int)Math.Floor((256.0f / (float)(power - 1)));
				//figure out how many elements each intensity in the lower bit-depth
				//will take up in 256 entries. At eight bits there is a one to one
				//correspondence between values and entries. At seven bits, two eight
				//bit intensities correspond to the same seven-bit intensity
        int sep = 256 / power;
				//we populate wider sections with a distributed intensity in the target
				//bit depth but represented as an 8 bit value.
				//So at two bits the values are 0, 85, 170, 255 
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
