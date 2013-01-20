using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.LexicalAnalysis;
using Libraries.Extensions;
using Libraries.Collections;

namespace Libraries.Tycho.Metadata
{
	public class DefineRegexSymbolAttribute : DefineWordAttribute
	{
		public DefineRegexSymbolAttribute(string name, string type, string expression)
			: base(expression, type)
		{
			Name = name;
            
		}	
		public override Word DefineWord()
		{
			return new RegexSymbol(TargetWord, Name, TargetWord);
		}
	}
	public abstract class DefineGenericRegexAttribute : DefineRegexSymbolAttribute
	{
		public string Addendum { get; private set; }
		public DefineGenericRegexAttribute(string name, string type, string addendum)
			: base(name, type, string.Empty)
		{
			Addendum = addendum;
			Group = "Literals";
		}
		public DefineGenericRegexAttribute(string name, string type)
			: this(name, type, string.Empty)
		{
		}
		public sealed override Word DefineWord()
		{
			return DefineWord_Impl();
		}
		protected abstract Word DefineWord_Impl();
	}

	public class DefineGenericFloatingPointNumberAttribute : DefineGenericRegexAttribute 
	{
		public DefineGenericFloatingPointNumberAttribute(string name, string type, string addendum) : base(name, type, addendum) { }
		public DefineGenericFloatingPointNumberAttribute(string name, string type) : base(name, type) { }
		protected override Word DefineWord_Impl()
		{
			return new GenericFloatingPointNumber(Name, WordType, Addendum);
		}
	}
	public class DefineGenericIntegerAttribute : DefineGenericRegexAttribute 
	{
		public DefineGenericIntegerAttribute(string name, string type, string addendum) : base(name, type, addendum) { }
		public DefineGenericIntegerAttribute(string name, string type) : base(name, type) { }

		protected override Word DefineWord_Impl()
		{
			return new GenericInteger(Name, WordType, Addendum);
		}
	}
	public class DefineStringLiteralAttribute : DefineRegexSymbolAttribute 
	{
		public string Before { get; private set; }
		public DefineStringLiteralAttribute(string name, string type, string expression, string before)
			: base(name, type, expression)
		{
			Before = before;
		//	Expression = string.Format("{0}{1}", before, expression);
		}
		public DefineStringLiteralAttribute(string name, string type, string expression)
			: this(name, type, expression, string.Empty)
		{
		}
		public DefineStringLiteralAttribute(string name, string type)
			: this(name, type, string.Empty)
		{
		}
		public DefineStringLiteralAttribute(string name)
			: this(name, "string-literal")
		{
		}
		public DefineStringLiteralAttribute()
			: this("String Literal")
		{
		}
		public override Word DefineWord()
		{
			if(TargetWord.Equals(string.Empty))
				TargetWord = ModifiableStringLiteral.MODIFIABLE_IDENTIFIER;
			return new ModifiableStringLiteral(TargetWord, Name, WordType, Before);
		}
	}
	public class DefineCharacterLiteralAttribute : DefineRegexSymbolAttribute 
	{
		public DefineCharacterLiteralAttribute(string name, 
				string type, string expression)
			: base(name, type, expression)
		{
		}
		public DefineCharacterLiteralAttribute(string name,
				string type) 
			: this(name, type, string.Empty)
		{
		}
		public DefineCharacterLiteralAttribute(string name)
			: this(name, "char-literal")
		{
		}
		public DefineCharacterLiteralAttribute()
			: this("Character Literal")
		{

		}

		public override Word DefineWord()
		{
			if(TargetWord.Equals(string.Empty))
				TargetWord = CharacterSymbol.DEFAULT_EXPRESSION;
			return new CharacterSymbol(TargetWord, Name, WordType);
		}
	}

}
