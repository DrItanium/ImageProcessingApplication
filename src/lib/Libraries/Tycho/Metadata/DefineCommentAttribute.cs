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
	public class DefineCommentAttribute : DefineWordAttribute 
	{
		public string EndingSymbol { get; private set; }
		public string CantHaveBefore { get; private set; }
		public DefineCommentAttribute(string type, string startSymbol, string endingSymbol, string cantHaveBefore)
			: base(startSymbol, type)
		{
			EndingSymbol = endingSymbol;
			CantHaveBefore = cantHaveBefore;
		}

		public override Word DefineWord()
		{
			return new Comment(WordType, TargetWord, EndingSymbol, CantHaveBefore);
		}
	}
}
