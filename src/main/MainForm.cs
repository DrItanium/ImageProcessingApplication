﻿using System;
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
    //this list handles both app domains
    private Dictionary<Guid, DynamicForm> dynamicFilterForms;
    private Dictionary<Guid, DynamicForm> dynamicFileFormatForms;
    private Dictionary<string,FilterToolStripMenuItem> addedFilters;
    private List<Guid> fileFormatIndexConversion;
    private FormConstructionLanguage dynamicConstructor;
    private Guid id;
    private System.Drawing.Bitmap srcImage, resultImage;
    private AppDomain pluginDomain;
    private AppDomain fileFormatDomain;
    private IPluginLoader<Tuple<string,string,Guid>> filterContainer;
    private IPluginLoader<Tuple<string,string,string,Guid,Tuple<bool,bool>>> fileFormatContainer;
    private List<string> blackList = new List<string>(new string[] { "Cortex.dll" });
    public MainForm()
    {
      //get list of files within the same directory
      dynamicConstructor = new FormConstructionLanguage();
      pluginDomain = AppDomain.CreateDomain("Plugin Domain");
      id = Guid.NewGuid();
      dynamicFilterForms = new Dictionary<Guid, DynamicForm>();
      addedFilters = new Dictionary<string, FilterToolStripMenuItem>();
      //setup the file format conversion tools
      fileFormatDomain = AppDomain.CreateDomain("File Format Conversion Domain");
      fileFormatIndexConversion = new List<Guid>();
      dynamicFileFormatForms = new Dictionary<Guid, DynamicForm>();
      InitializeComponent();
      SetupFilters();
      SetupFileFormats();
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
        //get the path of the item and send it over to the application domain
        //for processing. If possible, I would like to set it up so that we can
        //use the filter features of openFileDialog to make it seamless where
        //this should go. 
      string path = openFileDialog1.FileName; 
      //starts at 1....I'm used to that in CLIPS, not C#
      int index = openFileDialog1.FilterIndex - 1;
      
      if(index == 0)
      {
        LoadFile(path); 
      }
      else
      {
        ApplyToFileFormatOperation(sender, e, fileFormatIndexConversion[index],
            "load", path);
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
      ReloadFileFormats();
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
        string path = saveFileDialog1.FileName;
      //starts at 1....I'm used to that in CLIPS, not C#
        int index = saveFileDialog1.FilterIndex - 1;
        if(index == 0)
        {
          SaveFile(path);
        }
        else
        {
          ApplyToFileFormatOperation(sender, e, fileFormatIndexConversion[index],
              "save", path);
        }
        //clear out the file name dialog
        saveFileDialog1.FileName = string.Empty;
    }
  }
}
