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
	public class GenericFloatingPointNumber : RegexSymbol, IComparable<GenericFloatingPointNumber>
	{
			public const string BASE_FLOATING_POINT_STRING = "((?<=[^0-9a-zA-Z])(\\+|-)?)\\d+\\.\\d+{0}";
			public string Addendum { get; set; }
			public GenericFloatingPointNumber(string name, string type, string addendum)
				: base(string.Format(BASE_FLOATING_POINT_STRING, addendum), name, type)
			{
				Addendum = addendum;
			}
			public GenericFloatingPointNumber(string name, string type)
				: this(name, type, string.Empty)
			{

			}
			public virtual int CompareTo(GenericFloatingPointNumber g)
			{
				return base.CompareTo((RegexSymbol)g);
			}
			public override object Clone()
			{
				return new GenericFloatingPointNumber(Addendum, Name);
			}
	}
}
