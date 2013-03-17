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
	public abstract class AbstractGrammar<R,Encoding> : List<R>
		where R : Rule
		where Encoding : struct
	{
		private string name;
		public string Name { get { return name; } set { name = value; } }
		public abstract IEnumerable<string> SymbolTable { get; }
		public abstract IEnumerable<string> TerminalSymbols { get; } 
		public abstract IEnumerable<string> NonTerminalSymbols { get; }
		public abstract int NumberOfProductions { get; }
		protected AbstractGrammar() { }
		protected AbstractGrammar(IEnumerable<R> rules)
			: this()
		{
			AddRange(rules);
		}
		public bool IsTerminalSymbol(string symbol)
		{
			return !Exists(symbol);
		}
		public abstract void UpdateSymbolTable();

		public abstract Encoding ReverseLookup(string ruleName, Production target);
		public abstract IFixedRule LookupRule(Encoding index);
		public abstract IProduction LookupProduction(Encoding index);
		public abstract bool Exists(string rule);
		public abstract R this[string name] { get; }
		public abstract int IndexOf(string name);

		public new void AddRange(IEnumerable<R> rules)
		{
			foreach(var v in rules)
				Add_Impl(v, true);
			UpdateSymbolTable();
		}
		protected void BaseAdd(R r)
		{
			base.Add(r);
		}
		public new void Add(R r)
		{
			Add_Impl(r, false);
		}
		protected abstract void Add_Impl(R rule, bool delayUpdate);
		public new bool Remove(R rule)
		{
			return Remove_Impl(rule);	
		}
		protected abstract bool Remove_Impl(R rule);
		protected abstract void RemoveAt_Impl(int index);
		public new void RemoveAt(int index)
		{
			RemoveAt_Impl(index);
		}
		public new int RemoveAll(Predicate<R> pred)
		{
			//YAY Closures!
			return base.RemoveAll(MakeRemoveAllFunction(pred));
		}
		protected abstract Predicate<R> MakeRemoveAllFunction(Predicate<R> pred);

		public new void Clear()
		{
			base.Clear();
			Clear_Impl();
		}
		protected abstract void Clear_Impl();
	}
}
