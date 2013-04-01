using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using Frameworks.Plugin;
using Libraries.Messaging;

namespace Libraries.Filter
{
	[Obsolete("The filter class is obsolete. Use ImageFilter instead")]
		public abstract class Filter : ImageFilter, IFilter
	{
		public static Color[] GreyScaleColors = new Color[256];
		static Filter()
		{
			for(int i = 0; i < 256; i++)
				GreyScaleColors[i] = Color.FromArgb(255, i, i, i);
		}
		protected Filter(string name) : base(name) { }
		public sealed override int[][] TransformImage(Hashtable source) 
		{
			//break image into component channels (red, green, and blue).
			//Call the intensity filter three times (once for each color channel)
			//This is super inefficient but it does maintain backwards compatibility
			int[][] image = (int[][])source["image"];
			int width = image.Length;
			int height = image[0].Length;
			byte[][] translatedImageRed = new byte[width][];
			byte[][] translatedImageGreen = new byte[width][];
			byte[][] translatedImageBlue = new byte[width][];
			for(int i = 0; i < width; i++)
			{
				int[] iLine = image[i];
				byte[] lineR = new byte[height];
				byte[] lineG = new byte[height];
				byte[] lineB = new byte[height];
				for(int j = 0; j < height; j++)
				{
					lineR[j] = Color.FromArgb(iLine[j]).R;
					lineG[j] = Color.FromArgb(iLine[j]).G;
					lineB[j] = Color.FromArgb(iLine[j]).B;
				}
				translatedImageRed[i] = lineR;
				translatedImageGreen[i] = lineG;
				translatedImageBlue[i] = lineB;
			}
			source["image"] = translatedImageRed;
			byte[][] resultRed = Transform(source);
			source["image"] = translatedImageGreen;
			byte[][] resultGreen = Transform(source);
			source["image"] = translatedImageBlue;
			byte[][] resultBlue = Transform(source);
			//may be a larger image
			int newWidth = resultRed.Length;
			int newHeight = resultRed[0].Length;
			if(newWidth == width && newHeight == height) 
			{
				for(int i = 0; i < width; i++) 
				{
					byte[] rLine = resultRed[i];
					byte[] gLine = resultGreen[i];
					byte[] bLine = resultBlue[i];
					int[] line = image[i];
					for(int j = 0; j < newHeight; j++)
					{
						line[j] = Color.FromArgb(255, rLine[j],
							  gLine[j], bLine[j]).ToArgb();
					}
				}
			}
			else 
			{
				image = new int[newWidth][];
				for(int i = 0; i < newWidth; i++)
				{
					byte[] rLine = resultRed[i];
					byte[] gLine = resultGreen[i];
					byte[] bLine = resultBlue[i];
					int[] line = new int[newHeight];
					for(int j = 0; j < newHeight; j++)
					{
						line[j] = Color.FromArgb(255, rLine[j],
								gLine[j], bLine[j]).ToArgb();
					}
					image[i] = line;
				}
			}
			return image;
		}
		public abstract byte[][] Transform(Hashtable source);
	}

}
