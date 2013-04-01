using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using Frameworks.Plugin;
using Libraries.Messaging;

namespace Libraries.Filter
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
		public sealed class FilterAttribute : PluginAttribute
	{
		public FilterAttribute(string name) 
			: base(name)
		{
		}
	}
}
