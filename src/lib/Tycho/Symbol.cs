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
	public class Symbol : Word, IComparable<Symbol>
	{        
		public new char TargetWord 
		{ 
			get 
			{ 
				return base.TargetWord[0]; 
			} 
			set 
			{ 
				base.TargetWord = value.ToString(); 
			} 
		}
		public string Name { get; protected set; }
		public Symbol(char input, string name, string type)
			: base(input.ToString(), type)
		{
			Name = name;
		}
		public Symbol(char input, string name)
			: this(input, name, input.ToString())
		{

		}
		public Symbol(char input) : this(input, string.Empty) { }

		public override ShakeCondition<string> AsShakeCondition()
		{
			var fn = LexicalExtensions.GenerateSingleCharacterCond(TargetWord);
			return (x) => fn(x);
		}

		public override TypedShakeCondition<string> AsTypedShakeCondition()
		{
			var fn = LexicalExtensions.GenerateSingleTypedCharacterCond(TargetWord, WordType);
			return (x) => fn(x);
		}

		public override object Clone()
		{
			return new Symbol(TargetWord);
		}

		public virtual int CompareTo(Symbol other)
		{
			return TargetWord.CompareTo(other.TargetWord);
		}

	}
}
