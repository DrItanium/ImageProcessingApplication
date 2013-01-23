using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Collections;
using Libraries.Imaging;
using Libraries.Filter;
using Frameworks.Plugin;
using Libraries.LexicalAnalysis;
using Libraries.Extensions;
using Libraries.Parsing;
using Libraries.Starlight;
using Libraries.Tycho;
using msg = Libraries.Messaging;

namespace ImageProcessingApplication 
{
	public partial class MainForm 
	{
		private static double[] redConversion = new double[256];
		private static double[] blueConversion = new double[256];
		private static double[] greenConversion = new double[256];
		private static Color[] colorConversion = new Color[256];
		static MainForm()
		{
			for(int i = 0; i < 256; i++)
			{
				byte b = (byte)i;
				double value = i / 255.0;
				redConversion[i] = value * 0.3;
				blueConversion[i] = value * 0.11;
				greenConversion[i] = value * 0.59;
				colorConversion[i] = Color.FromArgb(255, b, b, b);
			}
		}
		private static byte DesaturateColor(Color c) 
		{
			return (byte)(255.0 * (redConversion[c.R] +
						blueConversion[c.B] +
						greenConversion[c.G]));
		}
		private void Desaturate(object sender, EventArgs e)
		{
			//go through the image and perform desaturation
			Bitmap clone = srcImage.Clone() as Bitmap;
			Func<int,int,Color> getPixelBase = (x,y) => clone.GetPixel(x,y);
			Action<int,int,byte> setPixelBase = (x,y,c) => clone.SetPixel(x,y, 
					colorConversion[c]);
			for(int i = 0; i < clone.Width; i++)
			{
				Func<int,Color> getPixel = (x) => getPixelBase(i,x);
				Action<int,byte> setPixel = (x,c) => setPixelBase(i,x,c);
				for(int j = 0; j < clone.Height; j++)
				{
					setPixel(j, DesaturateColor(getPixel(j)));
				}
			}
			this.resultImage = clone;
			RedrawPictures(false, true);
		}
	}
}
