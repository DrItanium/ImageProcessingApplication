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

namespace FileFormats.StarTrek
{
  [FileFormat("Star Trek Judgment Rites Bitmap [Default Palette] (*.BMP)")]
    public class StarTrekJudgementRitesBMPDefaultPaletteConverter : FileFormatConverter
  {
    //let's create a standard palette RGB => 332 layout
    static Color[] palette = new Color[256];
    static byte[] translation3bit = new byte[] 
    {
      0,
      36,
      73,
      110,
      146,
      183,
      219,
      255
    };
    static byte[] translation2bit = new byte[]
    {
      0,
      85,
      171,
      255,
    };

    static StarTrekJudgementRitesBMPDefaultPaletteConverter()
    {
      
      for(int i = 0; i < 256; i++)
      {
        //recompute this relative to 8-bits
        int r = i >> 5; 
        int g = ((byte)(i << 3)) >> 5;
        int b = ((byte)(i << 6)) >> 6;
        palette[i] = Color.FromArgb(255, 
            translation3bit[r],
            translation3bit[g],
            translation2bit[b]);
      }
    }
    public override bool SupportsSaving { get { return false; } }
    public override bool SupportsLoading { get { return true; } }
    public override string FilterString { get { return "*.BMP"; } }
    public override string FormCode { get { return null; } }
    public StarTrekJudgementRitesBMPDefaultPaletteConverter(string name) : base(name)  { }
    public override void Save(Hashtable input) { }
    public override int[][] Load(Hashtable input) 
    {
      string path = (string)input["path"];
      //translate the pallette setup to the color pallette
      using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) 
      {
        int[][] rawImage;

        //translation code base came from
        //https://code.google.com/p/scummvm-startrek/source/browse/trunk/graphics.cpp
        //
        //The original source code was written in C++ and I've translated it to
        //C#

        //palette creation
        int xoffset = ReadLittleEndianUShort(fs);
        int yoffset = ReadLittleEndianUShort(fs);
        int width = ReadLittleEndianUShort(fs);
        int height = ReadLittleEndianUShort(fs);
        //we can use some heuristics to prevent some really wierd resolutions
        if(width >= 1024 || height >= 1024)
        {
          throw new ArgumentException(string.Format("Given resolution is too large to be a stjr BMP file, {0}x{1}", width, height));
        }
        rawImage = new int[width][];
        byte[] tmpLine = new byte[height];
        for(int i = 0; i < width; i++)
        {
          //reuse the tmpLine
          int[] line = new int[height];
          fs.Read(tmpLine,0,height);
          //now we play the conversion game
          for(int j = 0, k = 0; j < height; j++,k++)
          {
						line[k] = palette[tmpLine[j]].ToArgb();
          }
          rawImage[i] = line;
        }
        return rawImage;
      }
    }
    private static ushort ReadLittleEndianUShort(FileStream fs)
    {
      byte[] tempStorage = new byte[2];
      fs.Read(tempStorage,0,2);
      return (ushort)((tempStorage[1] * 256) + tempStorage[0]);
    }

  }
}
