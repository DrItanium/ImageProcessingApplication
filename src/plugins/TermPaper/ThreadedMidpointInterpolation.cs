using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading;
using Libraries.Filter;
using Libraries.Imaging;
using System.Collections;

namespace CS555.TermPaper
{
	[Filter("Threaded Fractal Interpolation")]
		public class ThreadedMidpointInterpolationFilter : Filter
	{
		private bool allowGauss, allowVariance;
		public override string InputForm { get { return string.Format("form new \"{0} Filter\" \"Text\" imbue label new \"labelWidth\" \"Name\" imbue \"Width\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"labelHeight\" \"Name\" imbue \"Height\" \"Text\" imbue 63 13 size \"Size\" imbue 13 32 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"width\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"height\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"gauss\" \"Name\" imbue 85 82 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"No Gauss\" \"Text\" imbue 83 13 size \"Size\" imbue 13 82 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"variance\" \"Name\" imbue 85 102 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"No Variance\" \"Text\" imbue 83 13 size \"Size\" imbue 13 102 point \"Location\" imbue \"Controls.Add\" imbue return", Name); } }
		public ThreadedMidpointInterpolationFilter(string name) : base(name) 
		{
		}

		public override Hashtable TranslateData(Hashtable input) 
		{
			try
			{
				input["width"] = int.Parse((string)input["width"]);
				input["height"] = int.Parse((string)input["height"]);
			}
			catch(Exception)
			{
				return null;
			}
			return input; 
		}
		public static float ComputeK(int n, int startX, int startY, byte[][] image)
		{
			return ComputeK(n, startX, startY, image, 2.0f * (float)(Math.Pow(255, (float)n)));
		//	int total0 = 0;
		//	int total1 = 0;
		//	//alright this works!
		//	for(int j = 0; j < (n - 1); j++)
		//	{
		//		for(int i = 0; i < (n - 2); i++)
		//		{
		//			total0 += Math.Abs(image[startX + (i + 1)][startY + j] - image[startX + i][startY + j]);
		//			total1 += Math.Abs(image[startX + i][startY + (j + 1)] - image[startX + i][startY + j]);
		//		}
		//	}
		//	float numerator = total0 + total1;
		//	float denominator = 2.0f * (float)(Math.Pow(255, (float)n));
		//	return (numerator / denominator);
		}
		public static float ComputeK(int n, int startX, int startY, byte[][] image, float denominator)
		{
			int total0 = 0;
			int total1 = 0;
			//alright this works!
			for(int j = 0; j < (n - 1); j++)
			{
				for(int i = 0; i < (n - 2); i++)
				{
					total0 += Math.Abs(image[startX + (i + 1)][startY + j] - image[startX + i][startY + j]);
					total1 += Math.Abs(image[startX + i][startY + (j + 1)] - image[startX + i][startY + j]);
				}
			}
			float numerator = total0 + total1;
			return numerator / denominator;
		}
		public static float Variance(params float[] f)
		{
			float n = 0f;
			float mean = 0f;
			float m2 = 0f;
			foreach(var x in f)
			{
				n = n + 1f;	
				float delta = x - mean;
				mean = mean + delta/n;
				m2 = m2 + delta*(x - mean);
			}
			return m2 / (n - 1);	
		}
		public static float DeltaX(float x0, float x1, float y0, float y1)
		{
			return DeltaX((float)Math.Pow(x0 - x1, 2.0f), y0, y1);
		}
		public static float DeltaX(float p0, float y0, float y1)
		{
			return (float)Math.Abs((float)Math.Sqrt(p0 + (float)Math.Pow(y0 - y1, 2.0f)));
		}
		public static float[][] ToFloatTable(byte[][] items)
		{
			float[][] resultTable = new float[items.Length][];
			for(int i = 0; i < resultTable.Length; i++)
			{
				resultTable[i] = new float[items[i].Length];
				for(int j = 0; j < resultTable[i].Length; j++)
					resultTable[i][j] = ((float)items[i][j]) / 255.0f;
			}
			return resultTable;
		}
		public static void UpdateDisplacementTable(
				byte[][] image, 
				float[][] imageF,
				float[][] preComputedTable,
				int width, int height, int size, 
				bool allowGauss,
				bool allowVariance,
				float facW, float facH,
				int[] scaleX, int[] scaleY,
				float[] divX)
		{
			Random rnd = new Random(Guid.NewGuid().ToString().GetHashCode());
			int realXEnd = width - (size);
			int realYEnd = height - (size);
			float denominator = 2.0f * (float)(Math.Pow(255, (float)size));
			//lets define this as a series of quads or hexes whatever
			float[][] subK = new float[image.Length][];
			for(int i = 0; i < image.Length; i++)
				subK[i] = new float[image[i].Length];
			for(int i = 0; i < image.Length - (size - 1); i++)
			{
				for(int j = 0; j < image[i].Length - (size - 1); j++)
				{
					subK[i][j] = ComputeK(size, i, j, image, denominator);
				}
			}
			for(int i = 0; i < realXEnd; i++)
			{
				int x0 = scaleX[i];
				int x1 = scaleX[i + 1];
				float p0 = divX[i];
				float[] line0 = imageF[x0];
				float[] line1 = imageF[x1];
				for(int j = 0; j < realYEnd; j++)
				{
					int y0 = scaleY[j];
					int y1 = scaleY[j + 1];
					float f0 = line0[y0];
					float f1 = line0[y1];
					float f2 = line1[y1];
					float f3 = line1[y0];
					float k = subK[x0][y0];
					float gauss = !allowGauss ? (float)rnd.NextDouble() : 1.0f;
					float var = !allowVariance ? (float)Variance(f0,f1,f2,f3) : 1.0f;
					preComputedTable[i][j] = (float)Math.Sqrt(1.0f - (float)Math.Pow(2.0f, 2.0f * k - 2f)) 
						* gauss * var * DeltaX(p0,y0,y1);
				}
				rnd = new Random(Guid.NewGuid().ToString().GetHashCode());
			}
		}
		public override byte[][] Transform(Hashtable input)
		{
			DateTime start = DateTime.Now;
			//reseed it 
			if(input == null)
				return null;
			byte[][] b = (byte[][])input["image"];
			int width = (int)input["width"];
			int height = (int)input["height"];
			//allowSawtooth = (bool)input["sawtooth"];
			allowVariance = (bool)input["variance"];
			allowGauss = (bool)input["gauss"];
			ScaleInfo si = new ScaleInfo(b.Length, b[0].Length, width, height);
			if(si.IsZooming)
			{
				//compute k across the entire image
				int nWidth = si.ResultWidth;
				int nHeight = si.ResultHeight;
				//apply the original image values to the given points	
				float w = si.WidthScalingFactor;
				float h = si.HeightScalingFactor;
				int w0 = (int)Math.Ceiling(w);
				int h0 = (int)Math.Ceiling(h);
			//	int w0 = 1;
			//	int h0 = 1;
				float?[][] newImage = new float?[nWidth][];
				float[][] imageF = ToFloatTable(b);
				byte[][] resultant = new byte[si.ResultWidth][];
				float[][] preComputedTable = new float[nWidth][];
				int[] scaleX = new int[nWidth];
				int[] scaleY = new int[nHeight];
				float[] xDiv = new float[nWidth - 1];
				Thread t = new Thread(() => UpdateDisplacementTable(b, imageF, preComputedTable, 
							nWidth, nHeight, 4, allowGauss, allowVariance, w, h, scaleX, scaleY, xDiv));

				for(int i = 0; i < nWidth; i++)
				{
					newImage[i] = new float?[nHeight];
					resultant[i] = new byte[nHeight];
					preComputedTable[i] = new float[nHeight];
					scaleX[i] = (int)((float)i / w);
				}
				for(int i = 0; i < nWidth - 1; i++)
					xDiv[i] = (float)Math.Pow(scaleX[i] - scaleX[i + 1], 2.0f);
				for(int i = 0; i < nHeight; i++)
					scaleY[i] = (int)((float)i / h);
				t.Start();

				for(int i = 0; i < nWidth - 3; i+=w0)
				{
					int x0 = scaleX[i];
					int x1 = scaleX[i + 1];
					for(int j = 0; j < nHeight - 3; j+=h0)
					{
						int y0 = scaleY[j];
						int y1 = scaleY[j + 1];
						//order has been messed up for sometime
						//should go (x0,y0), (x0,y1), (x1,y1), (x1,y0)
						float f0 = imageF[x0][y0];
						float f1 = imageF[x1][y0];
						float f2 = imageF[x1][y1];
						float f3 = imageF[x0][y1];
						float k = preComputedTable[i][j];
						DivideGrid(resultant, newImage, i, j, w, h, f0, f1, f2, f3, k);
					}

				}
				newImage = null;
				si = null;
				imageF = null;
				Console.WriteLine("Took {0} seconds", DateTime.Now - start);
				return resultant;
			}
			else
			{
				ByteImage image = new ByteImage(b);
				if(si.IsShrinking)
				{
					var _i = image.ReplicationScale(si.ResultWidth, si.ResultHeight);
					byte[][] target = new byte[_i.Width][];
					for(int i = 0; i < _i.Width; i++)
					{
						byte[] q = new byte[_i.Height];
						for(int j = 0; j < _i.Height; j++)
							target[i][j] = _i[i,j];
						target[i] = q;
					}
					image = null;
					return target;
				}
				else
					return b;
			}
		}
		public void DivideGrid(
				byte[][] result, float?[][] points,
				float x, float y, 
				float width, float height,
				float c1, float c2, 
				float c3, float c4, float k)
		{  
			if(x >= points.Length || y >= points[0].Length)
				return;
			float edge1, edge2, edge3, edge4, middle;  
			//don't floor this!!!!
			//it will infinite loop
			if (width > 1.0f || height > 1.0f)  
			{  
				float newWidth = width / 2.0f;
				float newHeight = height / 2.0f;
				//insert the code for e and f types
				middle = Normalize(((c1 + c2 + c3 + c4) / 4.0f) + k);  //don't add displacement here
				edge1 = Normalize((c1 + c2) / 2.0f);    //Calculate the edges by averaging the two corners of each edge.  
				edge2 = Normalize((c2 + c3) / 2.0f);  
				edge3 = Normalize((c3 + c4) / 2.0f);  
				edge4 = Normalize((c4 + c1) / 2.0f);
				//Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!  
				//Do the operation over again for each of the four new grids.  
				float xW = x + newWidth;
				float yH = y + newHeight;
				float wNW = width - newWidth;
				float hNH = height - newHeight;
				DivideGrid(result, points, x, y, newWidth, newHeight, c1, edge1, middle, edge4,k);
				DivideGrid(result, points, xW, y, wNW, newHeight, edge1, c2, edge2, middle,k);  
				DivideGrid(result, points, xW, yH, wNW, hNH, middle, edge2, c3, edge3,k);
				DivideGrid(result, points, x, yH, newWidth, hNH, edge4, middle, edge3, c4,k);  
			}  
			else    //This is the "base case," where each grid piece is less than the size of a pixel.  
			{  
				int rX = (int)Math.Ceiling(x);
				int rY = (int)Math.Ceiling(y);
				if(rX >= points.Length || rY >= points[0].Length)
					return;
				if(points[rX][rY] == null) //either they're null or f type
				{
					//The four corners of the grid piece will be averaged and drawn as a single pixel.  
					//the thing that we also need to keep in mind that f pixels will be computed at the end
					//ideally we should only need to compute 

					var v = Normalize(((c1 + c2 + c3 + c4) / 4.0f) + k);
					points[rX][rY] = v;
					result[rX][rY] = (byte)(255.0f * v);
				}
			}  
		}  
		private static float Normalize(float iNum)  
		{  
			if(iNum < 0)
				return 0.0f;
			else if(iNum > 1.0f)
				return 1.0f;
			return iNum;
		}  
	}
}

