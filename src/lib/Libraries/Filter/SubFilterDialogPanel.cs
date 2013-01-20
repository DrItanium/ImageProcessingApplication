using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Libraries.Filter
{
  public class SubFilterDialogPanel : GroupBox
  {
    private string title;
    public string Title 
    {
      get
      {
        return title;
      }
      set
      {
        Text = value;
        this.title = value;
      }
    }
    public SubFilterDialogPanel() : this(string.Empty) { }
    public SubFilterDialogPanel(string title)
    {
      Title = title;
      Visible = false;
      SetupElements();
    }

    protected virtual void SetupElements()
    {
      //do nothing at this point
      //set the size
      SuspendLayout();
      Size = new Size(294, 172);
      Visible = false;
      Location = new Point(13, 40);
      ResumeLayout(false);
    }

		public virtual bool ValidateInput(object sender, EventArgs e)
		{
			return true;
		}
  }
}
