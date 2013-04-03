using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Libraries.Imaging;
using Libraries.Filter;
using System.Threading;
using System.ComponentModel;
using System.Collections;


namespace CS555.Homework2
{
	[Filter("Median Filter")]
		public class MedianFilter : SpatialFilter
	{
		private List<byte> elementsRed, elementsGreen, elementsBlue;
		public MedianFilter(string name) : base(name) 
		{
			elementsRed = new List<byte>();
			elementsGreen = new List<byte>();
      elementsBlue = new List<byte>();
		}
		protected override int Operation(int a, int b, int x, int y, int[][] input, Hashtable values)
		{
			int width = input.Length;
			int height = input[0].Length;
      elementsRed.Clear();
      elementsGreen.Clear();
      elementsBlue.Clear();
			for(int s = -a; s < a; s++)
			{
				int wX = x + s;
				if(wX < 0 || wX >= width)
					continue;
				byte[] iX = input[wX];
				for(int t = -b; t < b; t++)
				{
					int wY = y + t;
					if(wY < 0 || wY >= height)
						continue;
          Color c = Color.FromArgb(iX[wY]);
          elementsRed.Add(c.R);
          elementsGreen.Add(c.G);
          elementsBlue.Add(c.B);
				}
			}
      elementsRed.Sort();
      elementsGreen.Sort();
      elementsBlue.Sort();
      return Color.FromArgb(255, 
          elementsRed[elementsRed.Count >> 1],
          elementsGreen[elementsGreen.Count >> 1],
          elementsBlue[elementsBlue.Count >> 1]).ToArgb();
		}
	}
}
