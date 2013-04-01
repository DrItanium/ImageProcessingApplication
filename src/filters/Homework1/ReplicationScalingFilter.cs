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
	[Filter("Replication Scaling")]
		public class ReplicationScalingFilter : BasicScalingFilter
	{
		public ReplicationScalingFilter(string name) : base(name) { }

		protected override int[][] Interpolate(int[][] srcImage, int?[][] elements,
				float wFac, float hFac)
		{
			//go back in time and grab the previous non-null x-pixel
			int? curr = null;
			int width = elements.Length;
			int height = elements[0].Length;
			int[][] result = new int[width][];
			int i = 0, j = 0;
			for(i = 0; i < width; i++)
			{
				result[i] = new int[height];
			}
			for(j = 0; j < height; j++)
			{
				curr = null;
				for(i = 0; i < width; i++)
				{
					if(elements[i][j] != null)
						//set that as the new color
						curr = elements[i][j];
					else
						elements[i][j] = curr;
				}
			}	
			for(i = 0; i < width; i++)
			{
				curr = null;
				for(j = 0; j < height; j++)
				{
					if(elements[i][j] != null)
						//set that as the new color
						curr = elements[i][j];
					else
						elements[i][j] = curr;
				}
			}	
			for(i = 0; i < width; i++)
			{
				int[] rsltRow = result[i];
				int?[] nRow = elements[i];
				for(j = 0; j < height; j++)
				{
					rsltRow[j] = (int)nRow[j];	
				}
			}
			return result;
		}
	}
}
