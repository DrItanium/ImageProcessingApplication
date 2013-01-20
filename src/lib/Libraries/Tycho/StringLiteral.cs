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
	public class StringLiteral : RegexSymbol, IComparable<StringLiteral>
	{
		public const string DEFAULT_IDENTIFIER = "\"([^\"]|((?<=\\\\)((?<!\\\\\\\\)\"{1})))*\"";
		public StringLiteral(string expression, string name, string type)
			: base(expression, name, type)
		{

		}
		public StringLiteral(string expression,
				string name)
			: this(expression, name, "string-literal")
		{
		}
		public StringLiteral(string expression)
			: this(expression, "String Literal")
		{
		}
		public StringLiteral() : this(DEFAULT_IDENTIFIER) { }


		public virtual int CompareTo(StringLiteral other)
		{
			return ((RegexSymbol)this).CompareTo((RegexSymbol)other);
		}

	}

}
