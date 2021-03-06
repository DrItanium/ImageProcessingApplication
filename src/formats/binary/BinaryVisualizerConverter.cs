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
  public abstract class BinaryVisualConverter : FileFormatConverter
  {
    public abstract int DivisorFactor { get; }
    public override bool SupportsLoading { get { return true; } }
    public override bool SupportsSaving { get { return false; } }
    /* -- Omnicron script for binary visual converter
     * form new
     * "{0}" "Text" imbue
     * label new
     * "widthLabel" "Name" imbue
     * "Image Width" "Text" imbue
     * 13 12 point "Location" imbue
     * 63 13 size "Size" imbue
     * "Controls.Add" imbue
     * textbox new
     * "width" "Name" imbue
     * 80 12 point "Location" imbue
     * "Controls.Add" imbue
     * return
     */
    public override string FormCode
    { 
      get 
      { 
        return string.Format("form new \"{0}\" \"Text\" imbue label new \"widthLabel\" \"Name\" imbue \"Image Width\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"width\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue return", Name);
      } 
    }
    public override string FilterString { get { return "*.*"; } }
    protected BinaryVisualConverter(string name) : base(name) { }

    public override void Save(Hashtable input) { }
    private static int GetFileLength(string path)
    {
      return File.ReadAllBytes(path).Length;
    }
    public override int[][] Load(Hashtable input) 
    {
      string path = (string)input["path"];
      int length = GetFileLength(path);
      using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) 
      {
        int[][] rawImage;
        int width = int.Parse((string)input["width"]);
        int height = (width < length) ? (int)Math.Floor(
            ((double)length / (double)DivisorFactor) /
            (double)width) : 
          (int)Math.Floor((double)width / 
              ((double)length / (double)DivisorFactor));
        rawImage = new int[width][];
        for(int i = 0; i < width; i++)
        {
          int[] line = new int[height];
          for(int j = 0; j < height; j++)
          {
						line[j] = GetPixel(fs).ToArgb();
          }
          rawImage[i] = line;
        }
        return rawImage;
        //need to determine the image size 
      }
    }
    protected abstract Color GetPixel(FileStream fs);
  }
}
