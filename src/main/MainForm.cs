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
	public partial class MainForm : Form, IFilterCallback
	{
		private Dictionary<Guid, DynamicForm> dynamicForms = new Dictionary<Guid, DynamicForm>();
		private FormConstructionLanguage dynamicConstructor;
		private Guid id;
		private System.Drawing.Bitmap srcImage, resultImage;
		private AppDomain pluginDomain;
		private IPluginLoader container;
		private List<string> blackList = new List<string>(new string[]
				{
				"Libraries.Messaging.dll",
				"Libraries.Parsing.dll",
				"Libraries.Starlight.dll",
				"Libraries.Extensions.dll",
				"Libraries.LexicalAnalysis.dll",
				"Libraries.Tycho.dll",
				"Libraries.Imaging.dll",
				"Libraries.Filter.dll",
				"Frameworks.Plugin.dll",
				"Libraries.Collections.dll",
				});
		public MainForm()
		{
			//get list of files within the same directory
			dynamicConstructor = new FormConstructionLanguage();
			pluginDomain = AppDomain.CreateDomain("Plugin Domain");
			id = Guid.NewGuid();
			dynamicForms = new Dictionary<Guid, DynamicForm>();
			InitializeComponent();
			SetupFilters();
		}
		private static IEnumerable<string> GetPathsOfOtherAssemblies()
		{
			return GetPathsOfOtherAssemblies(Directory.GetCurrentDirectory());
		}
		private static IEnumerable<string> GetPathsOfOtherAssemblies(string path)
		{
			string[] items = Directory.GetFiles(path);	
			foreach(var v in items)
			{
				if(v.EndsWith(".dll"))
					yield return v;
			}
		}
		private void ApplyToImageBaseOperation(object sender, EventArgs e, Guid target)
		{
			if (srcImage != null)
			{
				Hashtable resultant = new Hashtable(); 
				if(dynamicForms.ContainsKey(target))
				{
					dynamicForms[target].ShowDialog();
					if(!dynamicForms[target].ShouldApply)
						return; //get out of here if they hit cancel
					resultant = dynamicForms[target].StorageCells;
				}
				msg.Message m = new msg.Message(Guid.NewGuid(), id, target, 
						msg.MessageOperationType.Execute, resultant);
				byte[][] elements = new byte[srcImage.Width][];
				resultant["image"] = elements;
				Func<int,int,Color> getPixelBase = (x,y) => srcImage.GetPixel(x,y);
				for(int i =0 ; i < srcImage.Width; i++)
				{
					Func<int,Color> getPixel = (x) => getPixelBase(i,x);
					byte[] line = new byte[srcImage.Height];
					for(int j = 0; j < srcImage.Height; j++)
					{
						line[j] = getPixel(j).R; 
					}
					elements[i] = line;
				}
				try 
				{
					var result = container.Invoke(m);
					var array = (byte[][])result.Value;
					resultImage = new Bitmap(array.Length, array[0].Length);
					Action<int,int,byte> setColorBase = (x,y,c) => resultImage.SetPixel(x,y,colorConversion[c]);
					for(int i =0 ; i < array.Length; i++)
					{
						byte[] aX = array[i];
						Action<int,byte> setColor = (y,c) => setColorBase(i,y,c);
						for(int j=0; j < aX.Length; j++)
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
		private Dictionary<string,FilterToolStripMenuItem> addedFilters = new Dictionary<string,FilterToolStripMenuItem>();
		private void LoadFilters()
		{
			foreach(var c in container.DesiredPluginInformation)
			{
				string name = c.Item1;
				string form = c.Item2;
				Guid targetGUID = c.Item3;

				FilterToolStripMenuItem curr = new FilterToolStripMenuItem(targetGUID, this);
				//make the form
				if(form != null && !form.Equals(string.Empty)) 
					dynamicForms.Add(targetGUID, dynamicConstructor.ConstructForm(form));
				curr.Text = name;
				curr.Name = string.Format("{0}FilterToolStripMenuItem", curr.Text.Replace(" ", "_").ToLower());
				addedFilters.Add(curr.Name, curr);
				curr.Size = new Size(183, 22);
				filtersToolStripMenuItem.DropDownItems.Add(curr);
			}
		}
		private void SetupFilters()
		{
			Assembly full = Assembly.LoadFile(Path.GetFullPath("Libraries.Filter.dll"));
			IEnumerable<string> assemblies = GetPathsOfOtherAssemblies();
			List<string> paths = new List<string>(); 
			foreach(var v in assemblies)
				if(!blackList.Exists((x) => v.Contains(x)))
					paths.Add(v);
			container = (IPluginLoader)pluginDomain.CreateInstanceAndUnwrap(
					full.FullName, "Libraries.Filter.Initiator",
					true, 0, null, new object[] { paths.ToArray() }, CultureInfo.CurrentCulture, null);
			LoadFilters();
		}
		private void ReloadFilters()
		{
			UnloadFilters();
			SetupFilters();
		}
		private void UnloadFilters()
		{
			dynamicForms = new Dictionary<Guid, DynamicForm>();
			container = null;
			menuStrip1.SuspendLayout();
			foreach(var v in addedFilters)
				filtersToolStripMenuItem.DropDownItems.Remove(v.Value);
			addedFilters = new Dictionary<string, FilterToolStripMenuItem>();
			menuStrip1.ResumeLayout(false);
			AppDomain.Unload(pluginDomain);
			pluginDomain = AppDomain.CreateDomain("Plugin Domain");
		}
		private void OpenImage(object sender, EventArgs e)
		{
			var result = openFileDialog1.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
				this.source.TargetImage = srcImage;
		}
		private void toolStripSeparator1_Click(object sender, EventArgs e)
		{
			OpenImage(sender, e);
		}

		private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
		{
			try
			{
				string path = openFileDialog1.FileName;
				srcImage = new Bitmap(Image.FromFile(path));
				source.Visible = true;
			}
			catch (Exception)
			{
				MessageBox.Show("Error: Given file isn't valid");
			}
		}

		private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void resultImageOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			resultImage = null;
			RedrawPictures(false, true);
		}

		private void bothSourceAndResultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			srcImage = null;
			resultImage = null;
			RedrawPictures(true, true);
		}
		private void transposeImagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tmp = srcImage;
			srcImage = resultImage;
			resultImage = tmp;
			RedrawPictures(true, true);
		}
		private void onReloadClicked(object sender, EventArgs e)
		{
			ReloadFilters(); ///Wheee!!!
		}
		private void RedrawPictures(bool src, bool dest)
		{
			if (src)
				this.source.TargetImage = srcImage;
			if (dest)
				this.result.TargetImage = resultImage;
		}
		private void SaveResultantImage(object sender, EventArgs e)
		{
			saveFileDialog1.ShowDialog();
		}
		private void SaveFile(object sender, CancelEventArgs e)
		{
			try
			{
				string path = saveFileDialog1.FileName;
				if(result != null)
					result.TargetImage.Save(path);
				else
					MessageBox.Show("Error: Can't Save Resultant Image. None Exists!");
			}
			catch (Exception)
			{
				MessageBox.Show("Error: There was a problem saving the file. Check your permissions");
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
		private static double[] redConversion = new double[256];
		private static double[] blueConversion = new double[256];
		private static double[] greenConversion = new double[256];
		private static Color[] colorConversion = new Color[256];
		static MainForm()
		{
			for(int i = 0; i < 256; i++)
			{
				byte b = (byte)i;
				double value = i / 255.0;
				redConversion[i] = value * 0.3;
				blueConversion[i] = value * 0.11;
				greenConversion[i] = value * 0.59;
				colorConversion[i] = Color.FromArgb(255, b, b, b);
			}
		}
		private static byte DesaturateColor(Color c) 
		{
			return (byte)(255.0 * (redConversion[c.R] +
						blueConversion[c.B] +
						greenConversion[c.G]));
		}
		private void Desaturate(object sender, EventArgs e)
		{
			//go through the image and perform desaturation
			Bitmap clone = srcImage.Clone() as Bitmap;
			Func<int,int,Color> getPixelBase = (x,y) => clone.GetPixel(x,y);
			Action<int,int,byte> setPixelBase = (x,y,c) => clone.SetPixel(x,y, 
					colorConversion[c]);
			for(int i = 0; i < clone.Width; i++)
			{
				Func<int,Color> getPixel = (x) => getPixelBase(i,x);
				Action<int,byte> setPixel = (x,c) => setPixelBase(i,x,c);
				for(int j = 0; j < clone.Height; j++)
				{
					setPixel(j, DesaturateColor(getPixel(j)));
				}
			}
			this.resultImage = clone;
			RedrawPictures(false, true);
		}
	}
}
