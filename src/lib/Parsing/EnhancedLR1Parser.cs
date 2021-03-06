//#define STATS
//#define WALK
//#define CLOSURE_STATS
using System;
using System.Reflection;
using System.Text;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Libraries.Starlight;
using Libraries.LexicalAnalysis;
using Libraries.Tycho;
using Libraries.Extensions;
using Libraries.Collections;

namespace Libraries.Parsing
{
	public class EnhancedLR1Parser : AbstractMemoizedLR1Parser<Rule, ulong>
	{
		public const string DEFAULT_TERMINATE_SYMBOL = "$";
		private EnhancedParsingTable table;
		private IEncoder<uint,ulong> enc = new CellEncoder();
		public EnhancedParsingTable ActionTable { get { return table; } }

		public EnhancedLR1Parser(EnhancedGrammar g, string terminateSymbol, SemanticRule r, bool supressMessages)
			: base(g, terminateSymbol, r, supressMessages, true)
		{
		}
		public EnhancedLR1Parser(EnhancedGrammar g, string terminateSymbol, SemanticRule r) : this(g, terminateSymbol, r, false) { }
		public EnhancedLR1Parser(EnhancedGrammar g, string terminateSymbol) : this(g, terminateSymbol, (x) => x[0]) { }
		public EnhancedLR1Parser(EnhancedGrammar g) : this(g, DEFAULT_TERMINATE_SYMBOL) { }
		public EnhancedLR1Parser(EnhancedGrammar g, string terminateSymbol, EnhancedParsingTable table, SemanticRule r)
			: base(g, terminateSymbol, r, true, false)
		{
			this.table = table;
			baseToken = new Token<string>(TerminateSymbol, TerminateSymbol, TerminateSymbol.Length);
			stateStack = new Stack<object>();
			initial = new LookaheadRule(TerminateSymbol, TargetGrammar[0]);
		}
		public EnhancedLR1Parser(EnhancedGrammar g, EnhancedParsingTable table, SemanticRule r) : this(g, DEFAULT_TERMINATE_SYMBOL, table, r) { }
		public override string RetrieveTables(Dictionary<string, string> symbolTable)
		{
			return string.Empty;
		}
		protected override void PreTableConstruction()
		{
			base.PreTableConstruction();
			table = new EnhancedParsingTable(TargetGrammar.SymbolTable.Concat(
						new string[] { TerminateSymbol }), 0);
		}
		protected override void PostTableConstruction()
		{
			//cPrime isn't needed anymore
			cPrime = null;
		}
		protected override void AddToGotoTable(int index, string current, int state)
		{
			table[current][state] = enc.Encode(new uint[] { (uint)TableCellAction.Goto, 0, (uint)index });
		}
		protected override void MakeTable_SetAcceptState(int state, string symbol)
		{
			table[symbol][state] = enc.Encode(new uint[] { (uint)TableCellAction.Accept, 0, 0 });
		}
		protected override TableCellAction MakeTable_ExtractAction(int state, string symbol)
		{
			IEnumerable<uint> currRule = enc.Decode(table[symbol][state]);
			return (TableCellAction)currRule.ElementAt(0);
		}
		protected override bool MakeTable_Reduce_Condition(ulong revision, int state, string lookaheadSymbol)
		{
			IEnumerable<uint> currRule = enc.Decode(table[lookaheadSymbol][state]);
			IEnumerable<uint> enc0 = enc.Decode(revision);
			uint rulNum = currRule.ElementAt(1);
			uint prodNum = currRule.ElementAt(2);
			uint rulNum0 = enc0.ElementAt(1);
			uint prodNum0 = enc0.ElementAt(2);
			return (rulNum != rulNum0) &&
				(prodNum != prodNum0);
		}
		protected override void MakeTable_SetReduceState(int state, string symbol, ulong rev)
		{
			uint _rule = (uint) (rev >> 32);
			uint _prod = (uint) (rev);
			table[symbol][state] = enc.Encode( new uint[] { (uint)TableCellAction.Reduce, _rule, _prod });
		}
		protected override bool MakeTable_Shift_Condition(int targetValue, int state, string lookaheadSymbol)
		{
			IEnumerable<uint> currRule = enc.Decode(table[lookaheadSymbol][state]);
			uint prodNum = currRule.ElementAt(2);
			uint prodNum0 = (uint)targetValue;
			return (prodNum != prodNum0);
		}
		protected override void MakeTable_SetShiftState(int state, string symbol, int target)
		{
			table[symbol][state] = enc.Encode(new uint[] { (uint)TableCellAction.Shift, 0, (uint)target });
		}
		private ulong Encode(uint upper, uint lower)
		{
			ulong value = (ulong)lower;
			ulong value2 = (ulong)upper;
			return (value + (value2 << 32));
		}
		public override object Parse(IEnumerable<Token<string>> tokens, string input)
		{
			var _input = tokens.Concat( new [] { baseToken });
			stateStack.Clear();
			stateStack.Push(baseToken);
			stateStack.Push((uint)0);
			Queue<Token<string>> _i = new Queue<Token<string>>(_input);
			var a = _i.Dequeue();
			uint s = 0;
			var result = table[a.TokenType][(int)s];

			while(true)
			{
				//object __input = stateStack.Peek();
				//Console.WriteLine("typeof input is {0}", __input.GetType());
				s = (uint)stateStack.Peek();
				result = table[a.TokenType][(int)s];
				IEnumerable<uint> encoding = enc.Decode(result);
				TableCellAction action = (TableCellAction)encoding.ElementAt(0);
				uint targetRule = encoding.ElementAt(1);
				uint targetProduction = encoding.ElementAt(2);
				switch(action)
				{
					case TableCellAction.Shift:
						stateStack.Push(a);
						stateStack.Push(targetProduction);
						a = _i.Dequeue();
						break;
					case TableCellAction.Reduce:
						var rule = TargetGrammar.LookupRule(Encode(targetRule,targetProduction));
						var firstRule = rule[0];
						int count = firstRule.Count; //not the rule's length but the rule's first production's length
						object[] elements = new object[count];	
						for(int i = 0; i < count; i++)
						{
							stateStack.Pop(); //targetState
							elements[i] = stateStack.Pop();
						}
						var element = (uint)stateStack.Peek();
						//we just do the goto anyway
						var next = enc.Decode(table[rule.Name][(int)element]);
						var quantity = firstRule.Rule(elements.Reverse().ToArray());
						stateStack.Push(quantity);
						stateStack.Push(next.ElementAt(2));
						break;
					case TableCellAction.Accept:
						if(!SupressMessages)
						{
							Console.WriteLine("State Stack Contents before accept");
							foreach(var v in stateStack)
								Console.WriteLine(v);
						}
						stateStack.Pop();
						return onAccept(new object[] { stateStack.Pop() } );
					case TableCellAction.Error:
						Console.WriteLine("Error Occurred at position {0}, {1}", a.Start, a.Value);
						Console.WriteLine("{0}", input.Substring(0, a.Start + 1));
						return false;

				}
			}	
		}
		public override bool IsValid(IEnumerable<string> input)
		{
			return false;
		}
	}
}
