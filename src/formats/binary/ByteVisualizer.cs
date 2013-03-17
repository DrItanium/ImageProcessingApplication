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
  [FileFormat("Visualize binary file [bytes] (*.*)")]
  public class ByteVisualizer : BinaryVisualConverter
  {
    public override int DivisorFactor { get { return 1; } }
    public ByteVisualizer(string name) : base(name) { }
    protected override Color GetPixel(FileStream fs)
    {
      int result = fs.ReadByte();
      if(result == -1)
      {
        return Color.Black;
      }
      else
      {
        return Color.FromArgb(255, result, result, result);
      }
    }

  }
}
