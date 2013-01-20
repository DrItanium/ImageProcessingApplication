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
	public abstract class DefineWordAttribute : BaseLexicalAnalysisMetadataTagAttribute
	{
		public string TargetWord { get; protected set; }
        public string WordType { get; protected set; }
		public DefineWordAttribute(string targetWord, string type)
		{
			TargetWord = targetWord;
            WordType = type;
		}

		public abstract Word DefineWord();
	}
}
