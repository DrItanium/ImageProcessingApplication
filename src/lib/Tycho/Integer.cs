using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Collections;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
	public class GenericInteger : RegexSymbol, IComparable<GenericInteger>
	{
		public const string BASE_NUMERIC_STRING = "((?<=[^0-9a-zA-Z])(\\+|-)?)((?<=[^\\w])\\d+{0})";
		//public const string BASE_NUMERIC_STRING = "((?<=[^0-9a-zA-Z])(\\+|-)?)((?<=[^\\w])\\d+{0})";
		public string Addendum { get; set; }
		public GenericInteger(string name, string type, string typeAddendum)
			: base(string.Format(BASE_NUMERIC_STRING, typeAddendum), 
					name, type)
		{
			Addendum = typeAddendum;
		}
		public GenericInteger(string name, string type)
			: this(name, type, string.Empty)
		{

		}
		public virtual int CompareTo(GenericInteger gi)
		{
			return base.CompareTo((RegexSymbol)gi);
		}
		public override object Clone()
		{
			return new GenericInteger(Addendum, Name);
		}
	}
}
