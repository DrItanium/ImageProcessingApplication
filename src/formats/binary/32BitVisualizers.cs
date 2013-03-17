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
  [FileFormat("Visualize binary file [32-bit LE] (*.*)")]
    public class LittleEndian32BitConverter : BinaryVisualConverter
  {
    public override int DivisorFactor { get { return 2; } }
    public LittleEndian32BitConverter(string name) : base(name) { }
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
        ushort lower = (ushort)result;
        ushort upper = (ushort)result2;
        upper = (ushort)(upper * (ushort)256);
        return Conversion.UshortTo565((ushort)(upper + lower));
      }
      int result4 = fs.ReadByte();
      if(result4 == -1)
      {
        //Little Endian goes backwards
        return Color.FromArgb(255, result3, result2, result);
      }
      //otherwise we have all four bytes...I don't want to do alpha
      //so the format is going to be ARGB which allows me to use .NET built in
      //functions
      uint v0 = (uint)result;
      uint v1 = (uint)(result2 << 8);
      uint v2 = (uint)(result3 << 16);
      uint v3 = (uint)(result4 << 24);
      uint value = v0 + v1 + v2 + v3;
      return Color.FromArgb((int)value);
    }

  }
  [FileFormat("Visualize binary file [32-bit BE] (*.*)")]
    public class BigEndian32BitConverter : BinaryVisualConverter
  {
    public override int DivisorFactor { get { return 2; } }
    public BigEndian32BitConverter(string name) : base(name) { }
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
        ushort lower = (ushort)result2;
        ushort upper = (ushort)result;
        upper = (ushort)(upper * (ushort)256);
        return Conversion.UshortTo565((ushort)(upper + lower));
      }
      int result4 = fs.ReadByte();
      if(result4 == -1)
      {
        return Color.FromArgb(255, result, result2, result3);
      }
      //otherwise we have all four bytes...I don't want to do alpha
      //so the format is going to be ARGB which allows me to use .NET built in
      //functions
      uint v0 = (uint)result4;
      uint v1 = (uint)(result3 << 8);
      uint v2 = (uint)(result2 << 16);
      uint v3 = (uint)(result << 24);
      uint value = v0 + v1 + v2 + v3;
      return Color.FromArgb((int)value);
    }

  }
}
