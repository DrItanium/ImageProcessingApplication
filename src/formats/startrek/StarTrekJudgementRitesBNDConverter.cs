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
  [FileFormat("Star Trek JudgmentRites BND (*.BGD)")]
    public class StarTrekJudgementRitesBNDConverter : FileFormatConverter
  {
    public override bool SupportsSaving { get { return false; } }
    public override bool SupportsLoading { get { return true; } }
    public override string FilterString { get { return "*.BGD"; } }
    public override string FormCode { get { return null; } }
    public StarTrekJudgementRitesBNDConverter(string name) : base(name)  { }
    public override void Save(Hashtable input) { }
		public override int[][] Load(Hashtable input) 
    {
      string path = (string)input["path"];
      //translate the pallette setup to the color pallette
      using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) 
      {
        byte[] buildp = new byte[1024];
        int[][] rawImage;
        Color[] palette = new Color[256];

        //translation code base came from
        //https://code.google.com/p/scummvm-startrek/source/browse/trunk/graphics.cpp
        //
        //The original source code was written in C++ and I've translated it to
        //C#

        //palette creation
        for(int i = 0; i < 256; i++) 
        {
          //RGBA
          int r = fs.ReadByte();
          int g = fs.ReadByte();
          int b = fs.ReadByte();
          if(r == -1 || g == -1 || b == -1) 
          {
            throw new ArgumentException("ERROR: Target file is not a valid BND file");
          }
          buildp[i * 4] = (byte)r;
          buildp[i * 4 + 1] = (byte)g;
          buildp[i * 4 + 2] = (byte)b;
          buildp[i * 4 + 3] = (byte)0;
        }

        for(int i = 0; i < 256; i++)
        {
          for(int j = 0; j < 3; j++)
          {
            buildp[i * 4 + j] = (byte)(buildp[i * 4 + j] << 2);
          }
        }
        //update the palette
        for(int i = 0, j = 0; i < 256; i++, j+=4) 
        {
            palette[i] = Color.FromArgb(buildp[j+3],
                buildp[j],
                buildp[j+1],
                buildp[j+2]);
        }
        int xoffset = ReadLittleEndianUShort(fs);
        int yoffset = ReadLittleEndianUShort(fs);
        int width = ReadLittleEndianUShort(fs);
        int height = ReadLittleEndianUShort(fs);
        //we can use some heuristics to prevent some really wierd resolutions
        if(width >= 1024 || height >= 1024)
        {
          throw new ArgumentException("Given resolution is too large to be a stjr BGD file");
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
