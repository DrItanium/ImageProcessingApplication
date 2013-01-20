using System;
using System.Reflection;
using System.Text;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Libraries.Starlight;
using Libraries.LexicalAnalysis;
using Libraries.Extensions;

namespace Libraries.Parsing
{
	public abstract class AbstractLR1Parser<R,Lookahead,Encoding> : Parser<R,Encoding>
		where R : Rule
		where Encoding : struct
		where Lookahead : ILookaheadRule
	{
		protected List<List<Lookahead>> cPrime;
		public List<List<Lookahead>> CPrime { get { return cPrime; } }
		protected SemanticRule onAccept;
		public SemanticRule OnAccept { get { return onAccept; } }

		protected AbstractLR1Parser(AbstractGrammar<R,Encoding> g, string terminateSymbol,
			 SemanticRule r, bool suppressMessages, bool setupRequired) 
			: base(g, terminateSymbol, suppressMessages, setupRequired)
		{
			this.onAccept = r;
		}
		public abstract string RetrieveTables(Dictionary<string, string> symbolTable);
		protected override void SetupParser()
		{
			cPrime = new List<List<Lookahead>>();
			SetupExtraParserElements();
			foreach(var v in Items())
				cPrime.Add(new List<Lookahead>(v));	
			PreTableConstruction();
			MakeTable();
			MakeGotoTable();
			PostTableConstruction();
		}
		protected abstract void MakeGotoTable();
		protected abstract void MakeTable();
		protected abstract void SetupExtraParserElements();
		protected abstract void PreTableConstruction();
		protected abstract void PostTableConstruction();
		public abstract IEnumerable<IEnumerable<Lookahead>> Items();
		public abstract IEnumerable<Lookahead> ComputeGoto(IEnumerable<Lookahead> i, string x);
		public IEnumerable<Lookahead> Closure(IEnumerable<Lookahead> rules)
		{
			return Closure(rules, TerminateSymbol);
		}
		public abstract IEnumerable<Lookahead> Closure(IEnumerable<Lookahead> rules,
				string terminateSymbol);
		public abstract IEnumerable<string> First(LookaheadRule rule, int lookahead);
	}	
}
