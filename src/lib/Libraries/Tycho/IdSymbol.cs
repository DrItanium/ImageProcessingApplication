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
	public class IdSymbol : RegexSymbol, IComparable<IdSymbol>
	{
		public const string DEFAULT_IDENTIFIER = "[a-zA-Z_$]([a-zA-Z0-9_$])*";
		public IdSymbol(string input = DEFAULT_IDENTIFIER)
			: base(input, "identifier", "id")
		{

		}
		public override TypedShakeCondition<string> AsTypedShakeCondition()
		{
			var fn = base.AsTypedShakeCondition();
			return (x) => 
			{
				var result = fn(x);
				if(x.Length > 0 && result == null)
					throw new Exception(
							string.Format("Given area {0} is not valid", x));
				return result;
			};
		}
		public virtual int CompareTo(IdSymbol other)
		{
			return ((RegexSymbol)this).CompareTo(other);
		}	

	}
}
