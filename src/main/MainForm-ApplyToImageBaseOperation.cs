using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Collections;
using Libraries.Imaging;
using Libraries.Filter;
using Frameworks.Plugin;
using Libraries.LexicalAnalysis;
using Libraries.Extensions;
using Libraries.Parsing;
using Libraries.Starlight;
using Libraries.Tycho;
using msg = Libraries.Messaging;

namespace ImageProcessingApplication 
{
	public partial class MainForm 
	{
		private void ApplyToImageBaseOperation(object sender, EventArgs e, Guid target)
		{
			if (srcImage != null)
			{
				Hashtable resultant = new Hashtable(); 
				if(dynamicFilterForms.ContainsKey(target))
				{
					dynamicFilterForms[target].ShowDialog();
					if(!dynamicFilterForms[target].ShouldApply)
						return; //get out of here if they hit cancel
					resultant = dynamicFilterForms[target].StorageCells;
				}
				msg.Message m = new msg.Message(Guid.NewGuid(), id, target, 
						msg.MessageOperationType.Execute, resultant);
				int[][] elements = new int[srcImage.Width][];
				resultant["image"] = elements;
				Func<int,int,int> getPixelBase = (x,y) => srcImage.GetPixel(x,y).ToArgb();
				for(int i =0 ; i < srcImage.Width; i++)
				{
					Func<int,int> getPixel = (x) => getPixelBase(i,x);
					int[] line = new int[srcImage.Height];
					for(int j = 0; j < srcImage.Height; j++)
					{
						line[j] = getPixel(j);
					}
					elements[i] = line;
				}
				try 
				{
					var result = filterContainer.Invoke(m);
					var array = (int[][])result.Value;
					int width = array.Length;
					int height = array[0].Length;
					resultImage = new Bitmap(width, height);
					Action<int,int,int> setColorBase = (x,y,c) => resultImage.SetPixel(x,y,Color.FromArgb(c));
					for(int i = 0; i < width; i++)
					{
						int[] aX = array[i];
						Action<int,byte> setColor = (y,c) => setColorBase(i,y,c);
						for(int j=0; j < height; j++)
						{
							setColor(j, aX[j]);
						}
					}
				} 
				catch (Exception exception) 
				{
					Console.WriteLine(exception.StackTrace);
					MessageBox.Show("An error occured during filter execution.\n See console for stack dump");
				}
			}
			else
			{
				OpenImage(this, new EventArgs());
			}
		}

		Guid IFilterCallback.CurrentFilter
		{
			set 
			{
				ApplyToImageBaseOperation(this, new EventArgs(), value);
				RedrawPictures(false, true);
				if(!result.Visible)
					result.Visible = true;
			}
		}
		string IFilterCallback.Name { set { } } 
	}
}
