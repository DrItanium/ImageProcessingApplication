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
    private void LoadFileFormats() 
    {
      StringBuilder filterString = new StringBuilder();
      foreach(var c in fileFormatContainer.DesiredPluginInformation)
      {
        string name = c.Item1;
        string filter = c.Item2;
        string form = c.Item3;
        Guid targetGuid = c.Item4;

        if(form != null && !form.Equals(string.Empty)) 
        {
          dynamicForms.Add(targetGuid, dynamicConstructor.ConstructForm(form));
        }
        //build the filter for the target element 
        filterString.Append(string.Format("{0}|{1}|", name, filter));
        fileFormatIndexConversion.Add(targetGuid);
      }
      //the all files is always the last item
      fileFormatIndexConversion.Add(id);
      sb.Append("All Files (*.*)|*.*");
    }
    private void SetupFileFilters() 
    {
      fileFormatIndexConversion.Clear();
      Assembly full = Assembly.LoadFile(Path.GetFullPath("Libraries.FileFormat.dll"));
      IEnumerable<string> assemblies = GetPathsOfOtherAssemblies();
      List<string> paths = new List<string>();
      foreach(var v in assemblies) 
      {
        if(!blackList.Exists((x) => v.Contains(x))) 
        {
          paths.Add(v);
        }
      }
      fileFormatContainer = (IPluginLoader<Tuple<string,string,string,Guid>>)
        fileFormatDomain.CreateInstanceAndUnwrap(full.FullName, 
            "Libraries.FileFormat.FileFormatInitiator", true, 0, null,
            new object[] { paths.ToArray() }, CultureInfo.CurrentCulture, null);
      LoadFileFormats();

    }
    private void ReloadFileFilters() 
    {
      UnloadFileFilters();
      SetupFileFilters();

    }
    private void UnloadFileFilters() 
    {
      dynamicFileFormatForms = new Dictionary<Guid, DynamicForm>();
      fileFormatContainer = null; 
      AppDomain.Unload(fileFormatDomain);
      fileFormatDomain = AppDomain.CreateDomain("File Format Conversion Domain");
    }
	}
}
