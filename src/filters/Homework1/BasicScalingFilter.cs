using System;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Libraries.Imaging;
using Libraries.Filter;

namespace CS555.Homework1
{
	public abstract class BasicScalingFilter : Filter
	{
		public override string InputForm { get { return string.Format("form new \"{0} Filter\" \"Text\" imbue label new \"labelWidth\" \"Name\" imbue \"Width\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"labelHeight\" \"Name\" imbue \"Height\" \"Text\" imbue 63 13 size \"Size\" imbue 13 32 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"width\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"height\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue return", Name); } }
		
		public BasicScalingFilter(string name) : base(name) { }

		public override Hashtable TranslateData(Hashtable source)
		{
			if(source == null)
				return null;
			else
			{
				Hashtable result = new Hashtable();
				result["image"] = source["image"];
				string height = (string)source["height"];
				string width = (string)source["width"];
				try
				{
					result["height"] = int.Parse(height);
				}
				catch(Exception)
				{
					MessageBox.Show("Height is invalid");
					return null;
				}
				try
				{
					result["width"] = int.Parse(width);
				}
				catch(Exception)
				{
					MessageBox.Show("Width is invalid");
					return null;
				}
				return result;
			}
		}
		protected abstract byte[][] Interpolate(byte[][] srcImage, byte?[][] result,
				float wFac, float hFac);
		public override byte[][] Transform(Hashtable source)
		{
			ByteImage image = new ByteImage((byte[][])source["image"]);
			int nWidth = (int)source["width"];
			int nHeight = (int)source["height"];
			ScaleInfo si = new ScaleInfo(image.Width, image.Height, nWidth, nHeight);
			byte?[][] result = image.Distribute(si); //distribute it across the new image
			if(si.IsShrinking)
				return ByteImage.Convert(result);
			else if(si.IsZooming)
				return Interpolate(image.Image, result, si.WidthScalingFactor, si.HeightScalingFactor); 
			else
				return image.Image;
		//	byte[][] srcImage = (byte[][])source["image"];
		//	int srcWidth = srcImage.Length;
		//	int srcHeight = srcImage[0].Length;
		//	int rsltWidth = (int)source["width"];
		//	int rsltHeight = (int)source["height"];
		//	float srcSize = srcWidth * srcHeight;
		//	float rsltSize = rsltWidth * rsltHeight;
		//	float sWidth = srcWidth;
		//	float sHeight = srcHeight;
		//	float rWidth = rsltWidth;
		//	float rHeight = rsltHeight;
		//	float factor = rsltSize / srcSize;
		//	float wFac = rWidth / sWidth; //width difference factor
		//	float hFac = rHeight / sHeight; //height difference factor

		//	if(factor < 1.0f)
		//	{
		//		//TODO: Improve running time of this code using the
		//		//same method as the other interpolation filters
		//		//shrink the image
		//		//we keep track of the 
		//		//we need to shrink it down
		//	  byte[][] result2 = new byte[rsltWidth][];
		//		for(int i = 0; i < result2.Length; i++)
		//		{
		//			var tmp = new byte[rsltHeight];
		//			for(int j = 0; j < tmp.Length; j++)
		//			{
		//				//overlay...its an interesting idea	
		//				int r1 = Math.Min(srcWidth - 1, 
		//						Math.Max(0, (int)Math.Floor(i / wFac)));
		//				int r2 = Math.Min(srcHeight - 1, 
		//						Math.Max(0, (int)Math.Floor(j / hFac)));
		//				tmp[j] = srcImage[r1][r2];
		//			}
		//			result2[i] = tmp;
		//		}
		//		return result2;
		//	}
		//	else if(factor > 1.0f)
		//	{
		//	  byte?[][] rsltImage = new byte?[rsltWidth][];
		//		for(int i = 0; i < rsltImage.Length; i++)
		//			rsltImage[i] = new byte?[rsltHeight];
		//		//setup the image
		//		//zooming works by keeping track of i and j factors that
		//		//grab the factor that is closest to a whole number
		//		float iFactor = 0.0f;
		//		for(int i = 0; i < srcWidth; i++)
		//		{
		//			float jFactor = 0.0f;
		//			for(int j = 0; j < srcHeight; j++)
		//			{
		//				//first map it to the target points in the new image
		//				try
		//				{
		//					rsltImage[(int)Math.Round(iFactor)][(int)Math.Round(jFactor)] = srcImage[i][j];
		//				}
		//				catch(IndexOutOfRangeException)
		//				{
		//					Console.WriteLine("ERROR: iFactor = {0}, jFactor = {1}", iFactor, jFactor);
		//				}
		//				jFactor += hFac;
		//			}
		//			iFactor += wFac; 
		//		}
		//		try
		//		{
		//		return Interpolate(srcImage, rsltImage, wFac, hFac);
		//		}
		//		finally
		//		{
		//			srcImage = null;
		//			rsltImage = null; //clean these up
		//		}
		//	}
		//	else
		//		return srcImage;

		}
	}
}
