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
	public class ScaleInfo
	{
		private float factor, wFac, hFac;
		private int srcWidth, srcHeight, rsltWidth, rsltHeight;
		private long srcSize, rsltSize;
		public long SourceImagePixelCount { get { return srcSize; } }
		public long ResultImagePixelCount { get { return rsltSize; } }
		public int SourceWidth { get { return srcWidth; } }
		public int SourceHeight { get { return srcHeight; } }
		public int ResultWidth { get { return rsltWidth; } }
		public int ResultHeight { get { return rsltHeight; } }
		public float WidthScalingFactor { get { return wFac; } }
		public float HeightScalingFactor { get { return hFac; } }
		public float ScalingFactor { get { return factor; } }
		public bool IsZooming { get { return factor > 1.0f; } }
		public bool IsShrinking { get { return factor < 1.0f; } }
		public ScaleInfo(int sWidth, int sHeight, int rWidth, int rHeight)
		{
			srcWidth = sWidth;
			srcHeight = sHeight;
			rsltWidth = rWidth;
			rsltHeight = rHeight;
			srcSize = (long)sWidth * (long)sHeight;
			rsltSize = (long)rWidth * (long)rHeight;
			float fsWidth = sWidth;
			float fsHeight = sHeight;
			float frWidth = rWidth;
			float frHeight = rHeight;
			factor = (float)rsltSize / (float)srcSize;
			wFac = (float)frWidth / (float)fsWidth;
			hFac = (float)frHeight / (float)fsHeight;
		}
	}
}
