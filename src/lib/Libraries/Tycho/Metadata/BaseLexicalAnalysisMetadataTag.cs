using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.LexicalAnalysis;
using Libraries.Extensions;
using Libraries.Collections;

namespace Libraries.Tycho.Metadata
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class BaseLexicalAnalysisMetadataTagAttribute : Attribute
	{
		public string Group { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		protected BaseLexicalAnalysisMetadataTagAttribute() : base() { }
	}
}
