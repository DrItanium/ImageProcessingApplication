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
		private void LoadFilters()
		{
			foreach(var c in filterContainer.DesiredPluginInformation)
			{
				string name = c.Item1;
				string form = c.Item2;
				Guid targetGUID = c.Item3;

				FilterToolStripMenuItem curr = new FilterToolStripMenuItem(targetGUID, this);
				//make the form
				if(form != null && !form.Equals(string.Empty)) 
					dynamicFilterForms.Add(targetGUID, dynamicConstructor.ConstructForm(form));
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
			filterContainer = (IPluginLoader<Tuple<string,string,Guid>>)pluginDomain.CreateInstanceAndUnwrap(
					full.FullName, "Libraries.Filter.FilterInitiator",
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
			dynamicFilterForms = new Dictionary<Guid, DynamicForm>();
			filterContainer = null;
			menuStrip1.SuspendLayout();
			foreach(var v in addedFilters)
				filtersToolStripMenuItem.DropDownItems.Remove(v.Value);
			addedFilters = new Dictionary<string, FilterToolStripMenuItem>();
			menuStrip1.ResumeLayout(false);
			AppDomain.Unload(pluginDomain);
			pluginDomain = AppDomain.CreateDomain("Plugin Domain");
		}
	}
}
