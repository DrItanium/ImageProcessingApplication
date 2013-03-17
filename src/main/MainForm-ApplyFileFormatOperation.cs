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
      return (bool)QueryInfo(target, "supports-load").Value;
    }
    private bool CanSave(Guid target)
    {
      return (bool)QueryInfo(target, "supports-save").Value;
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
        msg.Message m = new msg.Message(Guid.NewGuid(), id, target, 
            msg.MessageOperationType.Execute, resultant);
        try
        {
          var result = fileFormatContainer.Invoke(m);
          if(action.Equals("load")) 
          {
            var array = (Color[][])result.Value;
            srcImage = new Bitmap(array.Length, array[0].Length);
            for(int i = 0; i < array.Length; i++)
            {
              Color[] line = array[i];
              for(int j = 0; j < line.Length; j++)
              {
                srcImage.SetPixel(i, j, line[j]);
              }
            }
            source.Visible = true;
          }
        }
        catch(Exception ex)
        {
          Console.WriteLine(ex.StackTrace);
          MessageBox.Show(string.Format("An error occured during a {0}-file operation.\n See console for stack dump", action));
        }
      }
      else
      {
        MessageBox.Show(string.Format("Operation {0} is not supported", action));
      }
    }
  }
}