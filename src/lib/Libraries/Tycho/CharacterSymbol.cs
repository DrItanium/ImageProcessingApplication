using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Extensions;
using Libraries.Collections;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
	///<summary>
	///Used to capture chars from C-like languages.
	///Use Symbol if you want to use 
	///</summary>
	public class CharacterSymbol : RegexSymbol, IComparable<CharacterSymbol>
	{

		//new RegexSymbol("'(\\\\n|\\\\t|\\\\r|\\\\b|.)'", "Character","char-literal"),
		public const string DEFAULT_EXPRESSION = "'(\\\\n|\\\\t|\\\\r|\\\\b|.)'";

		public CharacterSymbol(string expression, string name, string type)
			: base(expression, name, type)
		{
		}
		public CharacterSymbol(string expression, string name)
			: this(expression, name, "char-literal")
		{
		}
		public CharacterSymbol(string expression)
			: this(expression, "Character Literal")
		{
		}
		public CharacterSymbol()
			: this(DEFAULT_EXPRESSION)
		{
		}

		public virtual int CompareTo(CharacterSymbol other)
		{
			return this.CompareTo((RegexSymbol)other);
		}
	}
}
