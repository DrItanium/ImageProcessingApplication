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
    private void LoadFile(string path)
    {
      try
      {
      srcImage = new Bitmap(Image.FromFile(path));
      source.Visible = true;
      } 
      catch(Exception)
      {
        MessageBox.Show("Error: Given file isn't valid");
      }
    }
    private void SaveFile(string path) 
    {
      try
      {
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
    }
	}
}
