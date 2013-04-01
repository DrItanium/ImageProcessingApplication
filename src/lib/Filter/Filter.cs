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
		public abstract class Filter : ImageFilter 
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
			int[][] image = (int[][])source["image"];
			int width = image.Length;
			int height = image[0].Length;
			byte[][] translatedImage = new byte[width][];
			for(int i = 0; i < width; i++)
			{
				int[] iLine = image[i];
				byte[] line = new byte[height];
				for(int j = 0; j < height; j++)
				{
					line[j] = Color.FromArgb(iLine[j]).R;
				}
				translatedImage[i] = line;
			}
			source["image"] = translatedImage;
			byte[][] result = Transform(source);
			//may be a larger image
			int newWidth = result.Length;
			int newHeight = result[0].Length;
			if(newWidth == width && newHeight == height) 
			{
				for(int i = 0; i < width; i++) 
				{
					byte[] rLine = result[i];
					int[] line = image[i];
					for(int j = 0; j < newHeight; j++)
					{
						line[j] = GreyScaleColors[rLine[j]].ToArgb();
					}
				}
			}
			else 
			{
				image = new int[newWidth][];
				for(int i = 0; i < newWidth; i++)
				{
					byte[] rLine = result[i];
					int[] line = new int[newHeight];
					for(int j = 0; j < newHeight; j++)
					{
						line[j] = GreyScaleColors[rLine[j]].ToArgb();
					}
					image[i] = line;
				}
			}
			source["image"] = image;
			//we are going to select the red channel for our byte image
			return source;
		}
		public abstract byte[][] Transform(Hashtable source);
	}

}
