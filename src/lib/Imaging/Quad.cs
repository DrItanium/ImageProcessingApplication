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
	///<summary>
	///Represents a 4x4 section of an image
	///</summary>
	public class Quad
	{
		private byte[][] points;
		private int startX, startY;
		public int StartX { get { return startX; } }
		public int StartY { get { return startY; } }

		public Quad(ByteImage image, int startX, int startY)
		{
			byte[][] original = image.Image;
			this.startX = startX;
			this.startY = startY;
			points = new byte[4][];
			for(int x = startX, i = 0; x < (startX + 4); x++, i++) //it will go four steps
			{
				byte[] line = new byte[4];
				for(int y = startY, j = 0; y < (startY + 4); y++, j++)
					line[j] = original[x][y];	
				points[i] = line;
			}
		}
	}
}
