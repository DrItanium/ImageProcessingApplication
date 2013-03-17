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
  public static class Conversion
  {
    public static byte As8Bits(byte value, int bitWidth)
    {
      int factor = ((int)Math.Pow(2,bitWidth)) - 1;
      if(factor == 0)
      {
        return (byte)0;
      } 
      else if(factor > 255)
      {
        return value;
      }
      else
      {
        double divisor = 255.0 / (double)factor;
        return (byte)(value * divisor);
      }
    }
    public static byte TwoBitsAs8Bits(byte value)
    {
      return As8Bits(value, 2);
    }
    public static byte ThreeBitsAs8Bits(byte value) 
    {
      return As8Bits(value, 3);
    }
    public static Color ByteToColor(byte value)
    {
        byte r = (byte)(value >> 5); 
        byte g = (byte)(((byte)(value << 3)) >> 5);
        byte b = (byte)(((byte)(value << 6)) >> 6);
        return Color.FromArgb(255, 
            ThreeBitsAs8Bits(r),
            ThreeBitsAs8Bits(g),
            TwoBitsAs8Bits(b));
    }
    public static Color UshortTo565(ushort value)
    {
      byte r = (byte)(value >> 11);
      byte g = (byte)(((ushort)(value << 5)) >> 10);
      byte b = (byte)(((ushort)(value << 11)) >> 11);
      return Color.FromArgb(255,
          As8Bits(r, 5),
          As8Bits(g, 6),
          As8Bits(b, 5));
    }
  }
  [FileFormat("Visualize binary file [16-bit LE] (*.*)")]
  public class LittleEndian16BitConverter : BinaryVisualConverter
  {
    public override int DivisorFactor { get { return 2; } }
    public LittleEndian16BitConverter(string name) : base(name) { }
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
      ushort lower = (ushort)result;
      ushort upper = (ushort)result2;
      upper = (ushort)(upper * (ushort)256);
      return Conversion.UshortTo565((ushort)(upper + lower));
    }

  }
  [FileFormat("Visualize binary file [16-bit BE] (*.*)")]
  public class BigEndian16BitConverter : BinaryVisualConverter
  {
    public override int DivisorFactor { get { return 2; } }
    public BigEndian16BitConverter(string name) : base(name) { }
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
      ushort lower = (ushort)result2;
      ushort upper = (ushort)result;
      upper = (ushort)(upper * (ushort)256);
      return Conversion.UshortTo565((ushort)(upper + lower));
    }

  }
}
