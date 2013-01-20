using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Libraries.Filter
{
	public class FilterToolStripMenuItem : ToolStripMenuItem
	{
		public Guid TargetFilter { get; set; }
		public IFilterCallback Callback { get; set; }
		public FilterToolStripMenuItem(Guid targetFilter, IFilterCallback callback)
		{
			TargetFilter = targetFilter;
			Callback = callback;
		}
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			Callback.CurrentFilter = TargetFilter; //set this
		}
	}
}
