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
		private msg.Message QueryInfo(Guid target, string action) 
		{
			Hashtable ht = new Hashtable();
			ht["action"] = action;
			msg.Message m = new msg.Message(Guid.NewGuid(), id, target,
					msg.MessageOperationType.Execute, ht);
			return fileFormatContainer.Invoke(m);
		}
		private bool CanLoad(Guid target) 
		{
			return (bool)QueryInfo(target, "supports-loading").Value;
		}
		private bool CanSave(Guid target)
		{
			return (bool)QueryInfo(target, "supports-saving").Value;
		}
		private bool CanPerformOperation(Guid target, string action) 
		{
			switch(action.ToLower()) 
			{
				case "load":
					return CanLoad(target);
				case "save":
					return CanSave(target);
				default:
					return false;
			}
		}
		private void ApplyToFileFormatOperation(object sender, EventArgs e, Guid target, string action, string path) 
		{
			if(CanPerformOperation(target, action)) 
			{
				Hashtable resultant = new Hashtable();
				if(dynamicFileFormatForms.ContainsKey(target)) 
				{
					dynamicFileFormatForms[target].ShowDialog();
					if(!dynamicFileFormatForms[target].ShouldApply)
					{
						return;
					}
					resultant = dynamicFileFormatForms[target].StorageCells;
				}
				resultant["action"] = action;
				resultant["path"] = path;
				try
				{
					if(action.Equals("save")) 
					{
						int[][] elements = new int[srcImage.Width][];
						resultant["image"] = elements;
						Func<int,int,Color> getPixelBase = (x,y) => srcImage.GetPixel(x,y);
						for(int i =0 ; i < srcImage.Width; i++)
						{
							Func<int,Color> getPixel = (x) => getPixelBase(i,x);
							int[] line = new int[srcImage.Height];
							for(int j = 0; j < srcImage.Height; j++)
							{
								line[j] = getPixel(j).ToArgb(); 
							}
							elements[i] = line;
						}
					}
					msg.Message m = new msg.Message(Guid.NewGuid(), id, target, 
							msg.MessageOperationType.Execute, resultant);
					var result = fileFormatContainer.Invoke(m);
					if(action.Equals("load")) 
					{
						var array = (int[][])result.Value;
						//we could always just return the RGBA value across the boundaries
						int width = array.Length;
						int height = array[0].Length;
						srcImage = new Bitmap(width, height);
						Action<int,int,int> setColorBase = (x,y,c) => srcImage.SetPixel(x,y,Color.FromArgb(c));
						for(int i = 0; i < width; i++)
						{
							int[] line = array[i];
							Action<int,int> setColor = (y,c) => setColorBase(i,y,c);
							for(int j = 0; j < height; j++)
							{
								setColor(j,line[j]);
							}
						}
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
					MessageBox.Show(string.Format("An error occured during a {0}-file operation.\nMessage: {1}", action, ex.Message));
				}
			}
			else
			{
				MessageBox.Show(string.Format("Operation {0} is not supported", action));
			}
		}
	}
}
