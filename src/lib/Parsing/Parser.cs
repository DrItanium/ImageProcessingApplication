using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.LexicalAnalysis;
using Libraries.Starlight;

namespace Libraries.Parsing
{
	public abstract class Parser<R,Encoding>
		where Encoding : struct
		where R : Rule
	{
		private AbstractGrammar<R,Encoding> g;
		public AbstractGrammar<R,Encoding> TargetGrammar { get { return g; } set { g = value; } }
		private string terminateSymbol;
		public string TerminateSymbol { get { return terminateSymbol; } set { terminateSymbol = value; } }
		public bool SupressMessages { get; set; }
		public bool SetupRequired { get; set; }
		public Parser(AbstractGrammar<R,Encoding> g, string terminateSymbol, bool supressMessages, bool setupRequired)
		{
			this.g = g;
			this.terminateSymbol = terminateSymbol;
			SupressMessages = supressMessages;
			SetupRequired = setupRequired;
			if(SetupRequired)
			  SetupParser();
		}
		public Parser(AbstractGrammar<R,Encoding> g, string terminateSymbol, bool supressMessages)
			: this(g, terminateSymbol, supressMessages, true)
		{

		}
		public Parser(AbstractGrammar<R,Encoding> g, string terminateSymbol)
			: this(g, terminateSymbol, false)
		{

		}
		protected abstract void SetupParser();
		public virtual object Parse(IEnumerable<Token<string>> tokens)
		{
			return Parse(tokens, string.Empty);
		}
		public abstract object Parse(IEnumerable<Token<string>> tokens, string input);
		public abstract bool IsValid(IEnumerable<string> input);
		
	}
}
