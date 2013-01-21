using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel;
using System.Data;

namespace ImageProcessingApplication 
{
	//we can do this to have a preview option
	//	public interface IUpdatable
	//	{
	//		void Update();
	//	}
	public class ImageDisplayPanel : Panel 
	{
		private int iWidth, iHeight;
		public int IdealWidth 
		{
			get
			{
				return iWidth;
			}
			set
			{
				iHeight = value;
				InitComponents(iWidth, iHeight, true);
			}
		}
		public int IdealHeight 
		{ 
			get 
			{
				return iHeight; 
			}
			set
			{
				iHeight = value;
				InitComponents(iWidth, iHeight, true);
			}
		}
		private PictureBox pBox;
		private Image targetImage;
		public Image TargetImage 
		{
			get
			{
				return targetImage;
			}
			set
			{
				targetImage = value;
				pBox.Image = targetImage;
				if(value != null)
					pBox.Size = new Size(targetImage.Width, targetImage.Height);
			}
		}


		public ImageDisplayPanel()
			: this(500, 768)
		{

		}
		public ImageDisplayPanel(int w, int h)
		{
			//this.title = title;
			Visible = false;
			InitComponents(w, h);
			iWidth = w;
			iHeight = h;
		}
		private void InitComponents() 
		{ 
			InitComponents(iWidth, iHeight); 
		}
		private void InitComponents(int width, int height) 
		{ 
			InitComponents(width, height, false); 
		}
		private void InitComponents(int width, int height, bool resizeOnly)
		{
			if(resizeOnly)
			{
				SuspendLayout();
				pBox.Size = new Size(width, height);
				Size = new Size(width + 11, height + 22);
				ResumeLayout(false);
			}
			else
			{
				pBox = new PictureBox();
				SuspendLayout();
				pBox.Location = new Point(4,4);
				pBox.Size = new Size(width,height);
				pBox.Name = "pBox";
				pBox.TabIndex  = 0;
				pBox.TabStop = false;
				Controls.Add(pBox);
				Location = new Point(0,0);
				Size = new Size(width + 12, height + 12);
				Visible = true;
				AutoScroll = true;
				ResumeLayout(false);
			}
		}
	}
}
