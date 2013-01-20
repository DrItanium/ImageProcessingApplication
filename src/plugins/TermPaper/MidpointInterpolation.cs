using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Filter;
using Libraries.Imaging;
using System.Collections;

namespace CS555.TermPaper
{
	[Filter("Inefficient Fractal Interpolation")]
		public class MidpointInterpolationFilter : Filter
	{
		private bool allowSawtooth, allowGauss, allowVariance;
		private float k;
		private Random rnd;
		//    public override string InputForm { get { return string.Format("form new \"{0} Filter\" \"Text\" imbue label new \"labelWidth\" \"Name\" imbue \"Width\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"labelHeight\" \"Name\" imbue \"Height\" \"Text\" imbue 63 13 size \"Size\" imbue 13 32 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"width\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"height\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"sawtooth\" \"Name\" imbue 80 62 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"Allow Sawtooth\" \"Text\" imbue 73 33 size \"Size\" imbue 13 52 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"gauss\" \"Name\" imbue 80 82 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"Disallow Gauss\" \"Text\" imbue 73 33 size \"Size\" imbue 13 82 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"variance\" \"Name\" imbue 80 102 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"Disallow Variance\" \"Text\" imbue 73 33 size \"Size\" imbue 13 102 point \"Location\" imbue \"Controls.Add\" imbue return", Name); } }
		public override string InputForm { get { return string.Format("form new \"{0} Filter\" \"Text\" imbue label new \"labelWidth\" \"Name\" imbue \"Width\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"labelHeight\" \"Name\" imbue \"Height\" \"Text\" imbue 63 13 size \"Size\" imbue 13 32 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"width\" \"Name\" imbue 80 12 point \"Location\" imbue \"Controls.Add\" imbue textbox new \"height\" \"Name\" imbue 80 32 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"sawtooth\" \"Name\" imbue 85 62 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"Sawtooth\" \"Text\" imbue 83 13 size \"Size\" imbue 13 52 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"gauss\" \"Name\" imbue 85 82 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"No Gauss\" \"Text\" imbue 83 13 size \"Size\" imbue 13 82 point \"Location\" imbue \"Controls.Add\" imbue checkbox new \"variance\" \"Name\" imbue 85 102 point \"Location\" imbue 16 16 size \"Size\" imbue \"Controls.Add\" imbue label new \"No Variance\" \"Text\" imbue 83 13 size \"Size\" imbue 13 102 point \"Location\" imbue \"Controls.Add\" imbue return", Name); } }
		public MidpointInterpolationFilter(string name) : base(name) 
		{
			rnd = new Random(Guid.NewGuid().ToString().GetHashCode());
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
		public float ComputeK(int n, int startX, int startY, byte[][] image)
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
			// for(int i = 0; i < (n - 2); i++)
			// {
			//   for(int j = 0; j < (n - 1); j++)
			//   {
			//     total1 += Math.Abs(image[startX + i][startY + (j + 1)] - image[startX + i][startY + j]);
			//   }
			// }	
			float numerator = total0 + total1;
			float denominator = 2.0f * (float)(Math.Pow(255, (float)n));
			return (numerator / denominator);
		}
		public float Variance(params float[] f)
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
		public float DeltaX(float x0, float x1, float y0, float y1)
		{
			return DeltaX((float)Math.Pow(x0 - x1, 2.0f), y0, y1);
		}
		public float DeltaX(float p0, float y0, float y1)
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
		public override byte[][] Transform(Hashtable input)
		{
			//reseed it 
			if(input == null)
				return null;
			rnd = new Random(Guid.NewGuid().ToString().GetHashCode());
			byte[][] b = (byte[][])input["image"];
			int width = (int)input["width"];
			int height = (int)input["height"];
			allowSawtooth = (bool)input["sawtooth"];
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
				Console.WriteLine("w x h: {0} x {1}", w, h);
				float?[][] newImage = new float?[nWidth][];
				float[][] displacementTable = new float[nWidth][];
				float[][] imageF = ToFloatTable(b);
				byte[][] resultant = new byte[si.ResultWidth][];
				for(int i = 0; i < nWidth; i++)
				{
					newImage[i] = new float?[nHeight];
					displacementTable[i] = new float[nHeight];
					resultant[i] = new byte[nHeight];
				}
				for(int i = 0; i < nWidth - 1; i++)
				{
					int x0 = (int)((float)i / w);
					int x1 = (int)((float)(i + 1) / w);
					float p0 = (float)Math.Pow(x0 - x1, 2.0f);
					float[] line0 = imageF[x0];
					float[] line1 = imageF[x1];
					for(int j = 0; j < nHeight - 1; j++)
					{

						int y0 = (int)((float)j / h);
						int y1 = (int)((float)(j + 1) / h);
						//order has been messed up for sometime
						//should go (x0,y0), (x0,y1), (x1,y1), (x1,y0)
						float f0 = line0[y0];
						float f1 = line0[y1];
						float f2 = line1[y1];
						float f3 = line1[y0];
						k = ComputeK(2,i,j,b);
						displacementTable[i][j] = k; //save the base value
						//if(k < 1.0f)
						//{
						float gauss = !allowGauss ? (float)rnd.NextDouble() : 1.0f;
						float var = !allowVariance ? Variance(f0,f1,f2,f3) : 1.0f;
						float tmp = (float)Math.Sqrt(1.0f - (float)Math.Pow(2.0f, 2.0f * k - 2f)) *
							DeltaX(p0,y0,y1) * gauss * var;
						k = tmp;
						//}
						DivideGrid(resultant, newImage, i, j, w, h, f0, f1, f2, f3);
					}
				}
				if(allowSawtooth)
				{
					List<float> temp = new List<float>();
					for(int x = 0; x < si.ResultWidth; x++)
					{
						int outCount = 0;
						int pX = x - 1;
						bool pXValid = (pX >=0);
						if(pXValid)
							outCount++;
						int fX = x + 1;
						bool fXValid = (fX < si.ResultWidth);
						if(fXValid)
							outCount++;
						for(int y = 0; y < si.ResultHeight; y++)
						{
							if(newImage[x][y] == null || newImage[x][y] > 1.0f)
							{
								temp.Clear();
								int pY = y - 1;
								int fY = y + 1;
								int count = outCount;
								float v0 = 0f, v1 = 0f, v2 = 0f, v3 = 0f;
								//f-type computation
								if(pXValid)
								{
									v0 = ((float)newImage[pX][y]);
									temp.Add(v0);
								}
								if(pY >= 0)
								{
									v1 = ((float)newImage[x][pY]);
									temp.Add(v1);
									count++;
								}
								if(fXValid)
								{
									v2 = ((float)newImage[fX][y]);
									temp.Add(v2);
								}
								if(fY < si.ResultHeight)
								{
									v3 = ((float)newImage[x][fY]);
									temp.Add(v3);
									count++;
								}
								//if count == 0 then we have bigger problems
								float total = (1.0f / (float)count);
								float p0 = total * (v0 + v1 + v2 + v3);

								float gauss = !allowGauss ? (float)rnd.NextDouble() : 1.0f;
								//get the k value 
								float dX = DeltaX(pX,fX,pY,fY);
								float cK = displacementTable[x][y];
								float var = !allowVariance ? Variance(temp.ToArray()) : 1.0f;
								float p1 = (float)Math.Pow(2.0f, -(cK / 2.0f));
								float p2 = (float)Math.Sqrt(1.0f - (float)Math.Pow(2.0, 2.0f * cK - 2.0f)); 
								float result = 255.0f * Normalize(p0 + p1 * p2 * dX * gauss * var);
								resultant[x][y] = (byte)result;
							}
						}
					}
					temp = null;
				}
				newImage = null;
				displacementTable = null;
				si = null;
				rnd = null;
				imageF = null;
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
			//we create the new 
		}
		public void DivideGrid(
				byte[][] result,
				float?[][] points,
				float x, float y, 
				float width, float height, 
				float c1, float c2, float c3, float c4)  
		{  
			if(x >= points.Length || y >= points[0].Length) return;
			float edge1, edge2, edge3, edge4, middle;  
			//don't floor this!!!!
			//it will infinite loop
			float newWidth = width / 2.0f;  
			float newHeight = height / 2.0f;
			if (width > 1.0f || height > 1.0f)  
			{  
				//insert the code for e and f types
				middle = Normalize(((c1 + c2 + c3 + c4) / 4.0f) + k);  //don't add displacement here
				edge1 = Normalize((c1 + c2) / 2.0f);    //Calculate the edges by averaging the two corners of each edge.  
				edge2 = Normalize((c2 + c3) / 2.0f);  
				edge3 = Normalize((c3 + c4) / 2.0f);  
				edge4 = Normalize((c4 + c1) / 2.0f);
				//Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!  
				//Do the operation over again for each of the four new grids.  
				DivideGrid(result, points, x, y, newWidth, newHeight, c1, edge1, middle, edge4);  
				DivideGrid(result, points, x + newWidth, y, width - newWidth, newHeight, edge1, c2, edge2, middle);  
				DivideGrid(result, points, x + newWidth, y + newHeight, width - newWidth, height - newHeight, middle, edge2, c3, edge3);  
				DivideGrid(result, points, x, y + newHeight, newWidth, height - newHeight, edge4, middle, edge3, c4);  
			}  
			else    //This is the "base case," where each grid piece is less than the size of a pixel.  
			{  
				int rX = (int)Math.Round(x);
				int rY = (int)Math.Round(y);
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
					if(allowSawtooth)
					{
						int q0 = rX + 1;
						int q1 = rX - 1;
						int q2 = rY + 1;
						int q3 = rY - 1;
						if(q0 < points.Length)
							points[q0][rY] = 2f;
						if(q2 < points[0].Length)
							points[rX][q2] = 2f;
						if(q1 >= 0)
							points[q1][rY] = 2f;
						if(q3 >= 0)
							points[rX][q3] = 2f;
					}
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

