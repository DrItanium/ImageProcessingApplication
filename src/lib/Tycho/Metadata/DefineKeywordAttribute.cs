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
	public class DefineKeywordAttribute : DefineWordAttribute
	{
		public bool RequiresEqualityCheck { get; set; }
		public DefineKeywordAttribute(string keyword)
			: base(keyword, keyword)
		{
			Group = "Keywords";			
		}
		public override Word DefineWord()
		{
			return new Keyword(TargetWord, RequiresEqualityCheck);
		}
	}
}
