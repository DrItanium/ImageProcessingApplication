using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Libraries.Filter
{
	public class Mask<T>
	{
		private T[,] cells;
		private int pointX, pointY;	
		private int width, height;
		public int Width { get { return width; } }
		public int Height { get { return height; } }

		public Mask(int width, int height)
		{
			cells = new T[width,height];
			this.width = width;
			this.height = height;
		}
//		public IEnumerable<T> NeighborValues(int w, int s)
//		{
//				if(w >= 0 && s >= 0)
//					yield return cells[w - 1, s - 1];
//				if(
//
//		}
	}
}
