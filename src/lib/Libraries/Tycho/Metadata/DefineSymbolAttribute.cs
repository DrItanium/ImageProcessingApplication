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
	public class DefineSymbolAttribute : DefineWordAttribute
	{
		public new char TargetWord { get { return base.TargetWord[0]; } 
			protected set { base.TargetWord = value.ToString(); } }
		public DefineSymbolAttribute(char value, string name, string type)
			: base(value.ToString(), type)
		{
			Name = name;	
		}

		public override Word DefineWord()
		{
			return new Symbol(TargetWord, Name);
		}
	}
	public class DefineWhitespaceAttribute : DefineSymbolAttribute
	{
		public DefineWhitespaceAttribute(char value, string name, string type)
		: base(value, name, type)
		{
			Group = "Whitespace";
		}
	}
}
