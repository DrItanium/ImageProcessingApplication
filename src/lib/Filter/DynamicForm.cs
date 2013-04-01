using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace Libraries.Filter
{
  public class DynamicForm : FilterDialogForm
  {
    private OpenFileDialog openFile;
		//Uncomment this when save support is implemented
    //private SaveFileDialog saveFile;
    private Guid uid;
    private Hashtable storageCells;
    public Hashtable StorageCells { get { return storageCells; } }
    public Guid UniqueID { get { return uid; } }
    public DynamicForm()
    {
      uid = Guid.NewGuid();
      storageCells = new Hashtable();
      openFile = new OpenFileDialog();
      openFile.FileName = "";
      openFile.FileOk += new System.ComponentModel.CancelEventHandler(OpenImageHandler);

    }
    private void OpenImageHandler(object sender, System.ComponentModel.CancelEventArgs e)
    {
      try
      {
        //load an image
        string path = openFile.FileName;
        var image = new Bitmap(Image.FromFile(path));
        byte[][] newImage = new byte[image.Width][];
        for(int x = 0; x < image.Width; x++)
        {
          byte[] line = new byte[image.Height];
          for(int y = 0; y < image.Height; y++)
            line[y] = image.GetPixel(x,y).R; 
          newImage[x] = line;
        }
        storageCells["otherImage"] = newImage;
        shouldApply = true;
      }
      catch(Exception)
      {
        MessageBox.Show("Invalid File Type Given");
        shouldApply = false;
      }
    }
    public void ButtonOpensFileDialog(Button b, bool addControl)
    {
      b.Click += new EventHandler(OnClick);
      if(addControl)
      {
        Controls.Add(b);
      }
    }
    private void OnClick(object sender, EventArgs e)
    {
      var result = openFile.ShowDialog();
      shouldApply &= (result == DialogResult.OK || result == DialogResult.Yes);
    }
    protected override bool OnOk(object sender, EventArgs e)
    {
      if(!shouldApply)
      {
        MessageBox.Show("Insufficient (or Invalid) Information Provided for Filter to be able to continue");
        shouldApply = true;
        return false;
      }
      else
      {
        foreach(var o in Controls)
        {
          Control ctrl = (Control)o;
          if(ctrl is CheckBox)
          {
            CheckBox cc = (CheckBox)ctrl;
            storageCells[cc.Name] = cc.Checked;
          }
          else if(!(ctrl is Label))
          {
            if(storageCells.ContainsKey(ctrl.Name))
              storageCells[ctrl.Name] = ctrl.Text; 
          }
        }
        shouldApply = true;
        return true;
      }
    }
  }
}
