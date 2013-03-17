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
	public class Grammar : RuleBasedGrammar<int>
	{
		public Grammar(IEnumerable<Rule> rules)
			: base(rules)
		{
		}
		public Grammar() : base() { }
		public override int ReverseLookup(string ruleName, Production target)
		{
			ushort targetRule = (ushort)IndexOf(ruleName); //
			ushort targetProduction = (ushort)this[(int)targetRule].IndexOf(target);
			return (int)((uint)(targetProduction << 16) + (uint)targetRule);
		}
		public override IFixedRule LookupRule(int index)
		{
			uint contents = (uint)index;
			ushort ruleNum = (ushort)contents;
			ushort prodNum = (ushort)(contents >> 16);
			return new FixedRule(this[(int)ruleNum], (int)prodNum);
		}
	}
}
