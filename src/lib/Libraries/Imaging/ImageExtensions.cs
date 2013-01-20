using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;


namespace Libraries.Imaging
{
	public struct Pixel1 
	{
		public byte Intensity { get; set; }
		public Pixel1(byte intensity) : this()
		{
			Intensity = intensity;
		}

		public static explicit operator Pixel3(Pixel1 pix)
		{
			return new Pixel3(pix.Intensity, pix.Intensity, pix.Intensity);
		}
		public static explicit operator Pixel4(Pixel1 pix)
		{
			return new Pixel4(pix.Intensity, pix.Intensity, pix.Intensity, (byte)255);
		}
	}
	public unsafe struct Pixel4
	{
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }
		public byte Alpha { get; set; }
		public Pixel4(byte* b)
			: this(b[0], b[1], b[2],b[3])
		{

		}
		public Pixel4(byte red, byte green, byte blue, byte alpha)
			: this()
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		public static explicit operator Pixel1(Pixel4 pix)
		{
			float f0 = 0.3f * (float)pix.Red;
			float f1 = 0.59f * (float)pix.Green;
			float f2 = 0.11f * (float)pix.Blue;
			return new Pixel1((byte)(f0 + f1 + f2));
			//how do I select the b
		}

		public static explicit operator Pixel3(Pixel4 pix)
		{
			return new Pixel3(pix.Red, pix.Green, pix.Blue);
		}
	}
	public unsafe struct Pixel3
	{
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }
		public Pixel3(byte* input)
			: this(input[0], input[1], input[2])
		{
		}
		public Pixel3(byte red, byte green, byte blue)
			: this()
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
		public static explicit operator Pixel1(Pixel3 pix)
		{
			float f0 = 0.3f * (float)pix.Red;
			float f1 = 0.59f * (float)pix.Green;
			float f2 = 0.11f * (float)pix.Blue;
			return new Pixel1((byte)(f0 + f1 + f2));
			//how do I select the b
		}
		public static explicit operator Pixel4(Pixel3 pix)
		{
			return new Pixel4(pix.Red, pix.Green, pix.Blue, (byte)255);
		}
	}

	public unsafe delegate void UnsafeImageTransformation(BitmapData data, byte* b);
	public static partial class ImageExtensions
	{
		public unsafe static void ApplyTranslationTableToBitmap(this Bitmap image, byte[] translationTable)
		{
			image.UnsafeTransformation((data, pointer) => ApplyTranslationTable(data, pointer, translationTable));
		}
		public unsafe static void ApplyTranslationTable(BitmapData data, byte* pointer, byte[] translationTable)
		{
			for(int i = 0; i < data.Width; i++)
			{
				for(int j = 0; j < data.Height; j++)
				{
					byte tmp;
					if(TryGetGrayScaleValue(pointer, out tmp))
						SetGrayScaleValue(pointer, translationTable[tmp]);
					else
					{
						//color
					}
					pointer += 4;
				}
			}
		}
		public static IEnumerable<byte> GrabNeighborhood(this byte[][] image, int centerX, int centerY, int width, int height, int squareSize)
		{
			//generate a starting location that would define this properly
			//since we know centerX and centerY 
			//compute startPoint, the factor that determines how to grab values from
			//this neighborhood
			int startPoint = (squareSize - 1) / 2; //this is the factor
			int endPoint = startPoint + 1; //this is the end condition
			for(int x = centerX - startPoint; x < centerX + endPoint; x++)
			{
				if(x >= 0 && x < width)
				{
					for(int y = centerY - startPoint; y < centerY + endPoint; y++)
					{
						if(y >= 0 && y < height)
							yield return image[x][y];
					}
				}
			}
		}
		public static unsafe byte[,] ToGreyScaleImage(byte* input, int width, int height)
		{
			byte[,] elements = new byte[width, height];
			for(int i = 0; i < width; i++)
			{
				for(int j = 0; j < height; j++)
				{
					elements[i, j] = input[0];
					input += 4;
				}
			}
			return elements;
			//assume 32bit rgb
		}
		public static unsafe byte[,] ToGreyScaleImage(byte* input, BitmapData data)
		{
			return ToGreyScaleImage(input, data.Width, data.Height);
		}
		public static unsafe byte[,] ToGreyScaleImage(this Bitmap b)
		{
			byte[,] result = null;
			b.UnsafeTransformation((x,y) => result = ToGreyScaleImage(y, x));
			return result;
		}
		private static void Empty(int input) { }
		public static unsafe void ApplyByteArrayToImage(this byte[,] b, int width, int height, byte* data)
		{
			TraverseAcrossImage(width, height, (x, y) => {
					unsafe{
					SetGrayScaleValue(data, b[x,y]);
					data += 4;
					}});
		}
		public static void TraverseAcrossImage(this BitmapData data, Action<int, int> inner)
		{
			TraverseAcrossImage(data, inner, Empty);
		}
		public static void TraverseAcrossImage(this BitmapData data, Action<int, int> inner, Action<int> outer)
		{
			TraverseAcrossImage(data.Width, data.Height, inner, outer);
		}
		public static void TraverseAcrossImage(int width, int height, Action<int, int> inner)
		{
			TraverseAcrossImage(width, height, inner, Empty);
		}
		public static void TraverseAcrossImage(int width, int height, Action<int,int> inner, Action<int> outer)
		{
			for(int i = 0; i < width; i++)
			{
				outer(i);
				for(int j = 0; j < height; j++)
					inner(i,j);
			}
		}
		public static unsafe void UnsafeTransformation(this Bitmap b, UnsafeImageTransformation transformer, PixelFormat format, ImageLockMode m)
		{
			BitmapData bits = b.LockBits(b.GetImageSizeRectangle(), m, format);
			unsafe
			{
				byte* input = (byte*)bits.Scan0;
				transformer(bits, input);
			}
			b.UnlockBits(bits);
		}
		public static unsafe void UnsafeTransformation(this Bitmap b, UnsafeImageTransformation transformer, PixelFormat format)
		{
			UnsafeTransformation(b, transformer, format, ImageLockMode.ReadWrite);
		}
		public static unsafe void UnsafeTransformation(this Bitmap b, UnsafeImageTransformation transformer)
		{
			UnsafeTransformation(b, transformer, b.PixelFormat);
		}
		public static Rectangle GetImageSizeRectangle(this Bitmap b)
		{
			return new Rectangle(0, 0, b.Width, b.Height);
		}
		public static unsafe void SetColorValue(byte* data, byte r, byte g, byte b)
		{
			data[0] = r;
			data[1] = g;
			data[2] = b;
		}
		public static unsafe bool TryGetGrayScaleValue(byte* data, out byte value)
		{
			byte r = data[0];
			byte g = data[1];
			byte b = data[2];
			bool gotValue = r == g && g == b;
			if(gotValue)
				value = r;
			else
				value = (byte)0;
			return gotValue;
		}
		public static unsafe void SetGrayScaleValue(byte* data, byte value)
		{
			data[0] = value;
			data[1] = value;
			data[2] = value;
			data[3] = (byte)255;
		}
	}
}
