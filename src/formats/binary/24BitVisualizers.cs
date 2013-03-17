using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.FileFormat;
using System.Drawing;

namespace Formats.Binary
{
  public abstract class Base24BitConverter : BinaryVisualConverter
  {
    public override int DivisorFactor { get { return 3; } }
    protected Base24BitConverter(string name) : base(name) { }
    protected override Color GetPixel(FileStream fs)
    {
      int result = fs.ReadByte();
      if(result == -1)
      {
        return Color.Black;
      }
      int result2 = fs.ReadByte();
      if(result2 == -1)
      {
        //do a palette conversion action on result
        return Conversion.ByteToColor((byte)result);
      }
      int result3 = fs.ReadByte();
      if(result3 == -1)
      {
        //do a conversion for 16-bits
        return Conversion.UshortTo565(Convert((byte)result, 
              (byte)result2));
      }
      return GetPixelImpl((byte)result, (byte)result2, (byte)result3);
    }
    protected abstract ushort Convert16(byte first, byte second);
    protected abstract Color GetPixelImpl(byte first, byte second, byte third);
  }
  [FileFormat("Visualize binary file [24-bit LE] (*.*)")]
    public class LittleEndian24BitConverter : Base24BitConverter 
  {
    public LittleEndian24BitConverter(string name) : base(name) { }
    protected override Color GetPixelImpl(byte first, byte second, byte third)
    {
        return Color.FromArgb(255, third, second, first);
    }
    protected override ushort Convert16(byte first, byte second)
    {
        ushort lower = (ushort)first;
        ushort upper = (ushort)second;
        upper = (ushort)(upper * (ushort)256);
        return (ushort)(lower + upper);
    }

  }
  [FileFormat("Visualize binary file [24-bit BE] (*.*)")]
    public class BigEndian24BitConverter : Base24BitConverter 
  {
    public BigEndian24BitConverter(string name) : base(name) { }
    protected override Color GetPixelImpl(byte first, byte second, byte third)
    {
        return Color.FromArgb(255, first, second, third);
    }
    protected override ushort Convert16(byte first, byte second)
    {
        ushort lower = (ushort)second;
        ushort upper = (ushort)first;
        upper = (ushort)(upper * (ushort)256);
        return (ushort)(lower + upper);
    }

  }
}
