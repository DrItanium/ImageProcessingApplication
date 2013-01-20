using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace Libraries.Filter
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class SubFilterAttribute : Attribute
	{
		private string name, parentFilterName;
		public string Name { get { return name; } }
		public string ParentFilterName { get { return parentFilterName; } }

		public SubFilterAttribute(string name, string parentFilterName)
		{
			this.name = name;
			this.parentFilterName = parentFilterName;
		}
	}
}
