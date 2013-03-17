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
    private Dictionary<Guid, DynamicForm> dynamicForms;
    private Dictionary<string,FilterToolStripMenuItem> addedFilters;
    private List<Guid> fileFormatIndexConversion;
    private FormConstructionLanguage dynamicConstructor;
    private Guid id;
    private System.Drawing.Bitmap srcImage, resultImage;
    private AppDomain pluginDomain, fileFormatDomain;
    private IPluginLoader<Tuple<string,string,Guid>> filterContainer;
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
      addedFilters = new Dictionary<string, FilterToolStripMenuItem>();
      //setup the file format conversion tools
      fileFormatDomain = AppDomain.CreateDomain("File Format Conversion Domain");
      fileFormatIndexConversion = new List<Guid>();
      InitializeComponent();
      SetupFilters();
    }
    private void OpenImage(object sender, EventArgs e)
    {
      var result = openFileDialog1.ShowDialog();
      if (result == System.Windows.Forms.DialogResult.OK || 
          result == System.Windows.Forms.DialogResult.Yes) {
        this.source.TargetImage = srcImage;
      }
      this.openFileDialog1.FileName = string.Empty;
    }
    private void toolStripSeparator1_Click(object sender, EventArgs e)
    {
      OpenImage(sender, e);
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      //we need to define a new format to make it possible to define custom
      //file format parsers. Obviously, these file parsers have to be in a
      //separate application domain to make it all worthwhile
      try
      {
        //get the path of the item and send it over to the application domain
        //for processing. If possible, I would like to set it up so that we can
        //use the filter features of openFileDialog to make it seamless where
        //this should go. 

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
        if(result != null) {
          result.TargetImage.Save(path);
        } else {
          MessageBox.Show("Error: Can't Save Resultant Image. None Exists!");
        }
      }
      catch (Exception)
      {
        MessageBox.Show("Error: There was a problem saving the file. Check your permissions");
      } 
      finally 
      {
        //clear out the file name dialog
        saveFileDialog1.FileName = string.Empty;
      }
    }
  }
}
