using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.Starlight
{
	public class EnhancedGrammar : RuleBasedGrammar<ulong>
	{
		public EnhancedGrammar(IEnumerable<Rule> rules)
			: base()
		{
			AddRange(rules);
		}
		public EnhancedGrammar() : base() { }

		public override ulong ReverseLookup(string ruleName, Production target)
		{
			uint targetRule = (uint)IndexOf(ruleName);
			uint targetProduction = (uint)this[(int)targetRule].IndexOf(target);
			ulong value = (ulong)targetProduction;
			value += ((ulong)targetRule << 32);
			return value;
		}

		public override IFixedRule LookupRule(ulong index)
		{
			uint prodNum = (uint)index;
			uint ruleNum = (uint)(index >> 32);
			return new FixedRule(this[(int)ruleNum], (int)prodNum);
		}
	}
}
