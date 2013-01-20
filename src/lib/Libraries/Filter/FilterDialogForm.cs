using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Libraries.Filter
{
	//TODO: Insert code to handle on the fly application
	//      of given filters
	public partial class FilterDialogForm : Form
	{
		protected bool shouldApply = true;
		public bool ShouldApply { get { return shouldApply; } }
		public FilterDialogForm()
		{
			InitializeComponent();
		}
		protected virtual bool OnOk(object sender, EventArgs e)
		{
			return true;
		}
		protected virtual void CleanUpFields(object sender, EventArgs e)
		{

		}
		private void ok_Click(object sender, EventArgs e)
		{
			shouldApply = true;
			if(OnOk(sender, e))
				Close();
		}
		private void Cancel_Click(object sender, EventArgs e)
		{
			shouldApply = false;
			CleanUpFields(sender, e);
			Close();
		}

	}
}
