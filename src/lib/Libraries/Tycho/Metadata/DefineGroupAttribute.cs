using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.LexicalAnalysis;
using Libraries.Collections;
using Libraries.Extensions;

namespace Libraries.Tycho.Metadata
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class DefineGroupAttribute : Attribute
	{
		public string Name { get; private set; }
		public int Order { get; set; }
		public DefineGroupAttribute(string name)
		{
			Name = name;
		}
	}
}
