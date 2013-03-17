
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
	public abstract class RuleBasedGrammar<Encoding> : AbstractGrammar<Rule,Encoding>
		where Encoding : struct
	{
		private Dictionary<string, int> lookup;
		private HashSet<string> terminalSymbols, nonTerminalSymbols;
		private int numProductions;
		public override IEnumerable<string> SymbolTable { get { return terminalSymbols.Concat(nonTerminalSymbols); } }
		public override IEnumerable<string> TerminalSymbols { get { return terminalSymbols; } }
		public override IEnumerable<string> NonTerminalSymbols { get { return nonTerminalSymbols; } }
		public override int NumberOfProductions { get { return numProductions; } }

		protected RuleBasedGrammar(IEnumerable<Rule> rules)
			: this()
		{
			AddRange(rules);
		}
		protected RuleBasedGrammar() 
		{
			terminalSymbols = new HashSet<string>();
			nonTerminalSymbols = new HashSet<string>();
			lookup = new Dictionary<string, int>();
	 	}
		public override void UpdateSymbolTable()
		{
			foreach(Rule r in this)
			{
				if(terminalSymbols.Contains(r.Name))
					terminalSymbols.Remove(r.Name);
				nonTerminalSymbols.Add(r.Name);
			}
			foreach(Rule r in this)
			{
				foreach(Production p in r)
				{
					foreach(string str in p)
					{
						if(!nonTerminalSymbols.Contains(str))
							terminalSymbols.Add(str);
					}
				}
			}
		}


		public override IProduction LookupProduction(Encoding index)
		{
			return LookupRule(index)[0];
		}
		public override bool Exists(string rule)
		{
			return lookup.ContainsKey(rule);
		}
		public override Rule this[string name] { get { return this[lookup[name]]; } }
		public override int IndexOf(string name) { return lookup[name]; }
		protected override void Add_Impl(Rule r, bool delayUpdate)
		{

			if(lookup.ContainsKey(r.Name))
			{
				//add the production rules instead
				Rule curr = this[lookup[r.Name]];
				bool shouldUpdate = false;
				foreach(var v in r)
				{
					if(!curr.Contains(v)) //prevent empty from being duplicated
					{
						shouldUpdate = true;
						curr.Add(v);
						numProductions++;
					}
				}
				if(shouldUpdate && !delayUpdate) //prevent a case where no new information is given
					UpdateSymbolTable();
			}
			else
			{
				int oldCount = Count;
				//HACK HACK!
				BaseAdd(r); //prevents a cyclic loop
				numProductions += r.Count;
				lookup.Add(r.Name, oldCount);
				if(!delayUpdate)
					UpdateSymbolTable();
			}
		}
		protected override bool Remove_Impl(Rule r)
		{
			bool result = base.Remove(r);
			if(result)
				numProductions -= r.Count;
			return result;
		}
		protected override void RemoveAt_Impl(int index)
		{
			var result = this[index];
			base.RemoveAt(index);
			numProductions -= result.Count;
		}
		protected override Predicate<Rule> MakeRemoveAllFunction(Predicate<Rule> pred)
		{
			//YAY Closures!
			return (x) => 
			{
				bool result = pred(x);
				if(result)
					numProductions -= x.Count;
				return result;
			};
		}
		protected override void Clear_Impl()
		{
			numProductions = 0;
			lookup.Clear();
			terminalSymbols.Clear();
			nonTerminalSymbols.Clear();
		}
	}
}
