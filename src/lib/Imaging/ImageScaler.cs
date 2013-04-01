using System;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace Libraries.Imaging 
{
	public abstract class AbstractImage<T, C>
		where T : struct
		where C : AbstractImage<T,C>
	{
		private T[][] image;
		private int width, height;
		private long size;
		public T[][] Image { get { return image; } }
		public int Width { get { return width; } }
		public int Height { get { return height; } }
		public long PixelCount { get { return size; } }
		protected AbstractImage(T[][] image) 
		{
			this.image = image;
			this.width = image.Length;
			this.height = image[0].Length;
			this.size = (long)width * (long)height;
		}
		public T?[][] Distribute(int newWidth, int newHeight) 
		{
			return Distribute(new ScaleInfo(width, height, newWidth, newHeight));
		}
		public T?[][] Distribute(ScaleInfo si)
		{
			float wFac = si.WidthScalingFactor;
			float hFac = si.HeightScalingFactor;
			T?[][] result = new T?[si.ResultWidth][];
			for(int i = 0; i < si.ResultWidth; i++)
				result[i] = new T?[si.ResultHeight];
			if(si.IsZooming)
			{
				float iFactor = 0.0f;
				for(int i = 0; i < width; i++)
				{
					float jFactor = 0.0f;	
					for(int j = 0; j < height; j++)
					{
						result[(int)Math.Round(iFactor)][(int)Math.Round(jFactor)] = image[i][j];
						jFactor += hFac;
					}
					iFactor += wFac;
				}
				return result;
			}
			if(si.IsShrinking)
			{
				for(int i = 0; i < si.ResultWidth; i++)
				{
					for(int j = 0; j < si.ResultHeight; j++)
					{
						//overlay...its an interesting idea	
						int r1 = Math.Min(width - 1, 
								Math.Max(0, (int)Math.Floor(i / wFac)));
						int r2 = Math.Min(height - 1, 
								Math.Max(0, (int)Math.Floor(j / hFac)));
						result[i][j] = image[r1][r2];
					}
				}
				return result;
			}
			else 
			{
				return Convert(image);
			}
		}
		public C PadImage(int factor)
		{
			//check to see if the width and height are of the proper
			//size already
			int modW = width % factor;
			int modH = height % factor;
			if(modW == 0 && modH == 0)
				return (C)this;
			else
			{
				//get the difference between this and the new item
				int wDiff = width + (factor - modW);
				int hDiff = height + (factor - modH);
				return ReplicationScale(wDiff, hDiff);
			}
		}
		public C ReplicationScale(int newWidth, int newHeight)
		{
			ScaleInfo si = new ScaleInfo(width, height, newWidth, newHeight);
			T?[][] newImage = Distribute(si);	
			if(si.IsZooming)
			{
				T? curr = null;
				int rWidth = newImage.Length;
				int rHeight = newImage[0].Length;
				int i = 0, j = 0;
				for(j = 0; j < rHeight; j++)
				{
					curr = null;
					for(i = 0; i < rWidth; i++)
					{
						if(newImage[i][j] != null)
							//set that as the new color
							curr = newImage[i][j];
						else
							newImage[i][j] = curr;
					}
				}	
				for(i = 0; i < rWidth; i++)
				{
					curr = null;
					for(j = 0; j < rHeight; j++)
					{
						if(newImage[i][j] != null)
							//set that as the new color
							curr = newImage[i][j];
						else
							newImage[i][j] = curr;
					}
				}	
				return MakeInstance(Convert(newImage));
			}
			else if(si.IsShrinking)
			{
				return MakeInstance(Convert(newImage));
			}
			else
			{
				return (C)this;
			}
		}
		public T this[int x, int y] { get { return image[x][y]; } }
		public T[] this[int x] { get { return image[x]; } }
		protected abstract C MakeInstance(T[][] input);
		public static T?[][] Convert(T[][] input)
		{
			int width = input.Length;
			int height = input[0].Length;
			T?[][] result = new T?[width][];
			for(int i = 0; i < width; i++)
			{
				result[i] = new T?[height];	
				for(int j = 0; j < height; j++)
				{
					result[i][j] = (T?)input[i][j];
				}
			}
			return result;
		}
		public static T[][] Convert(T?[][] input)
		{
			int width = input.Length;
			int height = input[0].Length;
			T[][] result = new T[width][];
			for(int i = 0; i < width; i++)
			{
				result[i] = new T[height];	
				for(int j = 0; j < height; j++)
				{
					result[i][j] = (T)input[i][j];
				}
			}
			return result;
		}
	}
	public class ByteImage : AbstractImage<byte, ByteImage> 
	{
		public ByteImage(byte[][] image) : base(image) { }
		protected override ByteImage MakeInstance(byte[][] input) 
		{
			return new ByteImage(input);
		}
	}
	public class ArgbImage : AbstractImage<int, ArgbImage> 
	{
		public ArgbImage(int[][] image) : base(image) { }
		protected override ArgbImage MakeInstance(int[][] input)
		{
			return new ArgbImage(input);
		}
	}
	public class ColorImage : AbstractImage<Color, ColorImage>
	{
		public ColorImage(Color[][] image) : base(image) { }
		protected override ColorImage MakeInstance(Color[][] input) 
		{
			return new ColorImage(input);
		}
	}
}
