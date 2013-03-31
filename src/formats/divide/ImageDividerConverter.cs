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

namespace Formats.Divide 
{
	///<summary>
	///Base class for all Image division format converters
	///</summary>
	public abstract class ImageDividerConverter : FileFormatConverter 
	{
		public abstract int DivideWidth { get; }
		public abstract int DivideHeight { get; }
		public override bool SupportsLoading { get { return false; } }
		public override bool SupportsSaving { get { return true; } }
		/* -- Omnicron script for image division
		 * form new
		 * "{0}" "Text" imbue
		 * -- Prefix Section
		 * label new
		 * "prefixLabel" "Name" imbue
		 * "Image Prefix" "Text" imbue
		 * 13 12 point "Location" imbue
		 * 63 13 size "Size" imbue
		 * "Controls.Add" imbue
		 * textbox new
		 * "prefix" "Name" imbue
		 * 80 12 point "Location" imbue
		 * "Controls.Add" imbue
		 * -- Extension Section
		 * label new
		 * "extensionLabel" "Name" imbue
		 * "Extension" "Text" imbue
		 * 13 32 point "Location" imbue
		 * 63 13 size "Size" imbue
		 * "Controls.Add" imbue
		 * textbox new
		 * "extension" "Name" imbue
		 * 80 32 point "Location" imbue
		 * "Controls.Add" imbue
		 * return
		 */
		public override string FormCode 
		{ 
			get
			{ 
				return "";
        //return "form new \"{0}\" \"Text\" imbue label new \"prefixLabel\" \"Name\" imbue \"Image Prefix\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"prefix\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue label new \"extensionLabel\" \"Name\" imbue \"Extension\" \"Text\" imbue 13 32 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue textbox new \"extension\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue return";
			}
		}

		protected ImageDividerConverter(string name) : base(name) { }
		public override string FilterString { get { return "*.*"; } }
		public override byte[][] Load(Hashtable input) 
		{
			throw new NotImplementedException("Load is not supported in this filter");
		}
		private static readonly string SaveFormatString = "{0}/{1}-tile{2}{3}";
		public override void Save(Hashtable input) 
		{
			string fullPath = (string)input["path"];
			string fileName = Path.GetFileNameWithoutExtension(fullPath);
			string extension = Path.GetExtension(fullPath);
			string path = Path.GetDirectoryName(fullPath); 
			int[][] image = (int[][])input["image"];
			//determine how many blocks the target image is made up of
			int width = image.Length;
			int height = image[0].Length;
			int tileWidth = width / DivideWidth;
			int tileHeight = height / DivideHeight;
			if(width % DivideWidth != 0 || 
				 height % DivideHeight != 0) 
			{ 
				throw new ArgumentException("Target image can't be cleanly broken up into tiles");
			}
      int total = tileWidth * tileHeight;
			int[][] block = new int[DivideWidth][];
			string partial = string.Format(SaveFormatString, path, fileName, "{0}", extension);
			//get the file name from the provided base name
			for(int i = 0; i < tileWidth; i++) 
			{
				for(int j = 0; j < tileHeight; j++)
				{
				   SaveTile(string.Format(partial,i * j), CreateTile(image, i, j));
				}
			}

		}
		private static void SaveTile(string path, int[][] tile) 
		{
		  int width = tile.Length;
			int height = tile[0].Length;
			Bitmap outTile = new Bitmap(width, height);
			Action<int,int,int> setColorBase = (x,y,c) => outTile.SetPixel(x,y, Color.FromArgb(c));
			for(int i = 0; i < width; i++)
			{
			  Action<int,int> setColor = (y,c) => setColorBase(i,y,c); 
				int[] line = tile[i];
				for(int j = 0; j < height; j++)
				{
					 setColor(j,line[j]);	
				}
			}
			outTile.Save(path);
		}
		private int[][] CreateTile(int[][] image, int x, int y)
		{
			int convX = x * DivideWidth;
			int convY = y * DivideHeight;
			int convXEnd = convX + DivideWidth;
			int convYEnd = convY + DivideHeight; 
			int[][] outputTile = new int[DivideWidth][];
			for(int i = convX, r = 0 ; i < convXEnd; i++, r++)
			{
				int[] inLine = image[i];
				int[] outLine = new int[DivideHeight];
				for(int j = convY, s = 0; j < convYEnd; j++, s++)
				{
					outLine[s] = inLine[j];	
				}
				outputTile[r] = outLine;
			}
			return outputTile;
		}
	}

}
