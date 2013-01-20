using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Extensions;
using Libraries.LexicalAnalysis;
using Libraries.Tycho;
using Libraries.Collections;
using Libraries.Starlight;

namespace Libraries.Parsing
{
	public class ParsableLanguage<R,Encoding>
		where R : Rule
		where Encoding : struct
	{
		private Language lang;
		private Parser<R,Encoding> parser;
		private TokenShakerContainer<string> tok;
		private Func<Token<string>, bool> shouldKeep;
		public bool SuppressMessages { get; set; }
		public Language Lang { get { return lang; } protected set { lang = value; } }
		public Parser<R,Encoding> Parser { get { return parser; } protected set { parser = value; } }
		public TokenShakerContainer<string> Tokenizer { get { return tok; } protected set { tok = value; } }
		public ParsableLanguage(
				Language lang, 
				TypedShakeSelector<string> selector,
			 	Parser<R,Encoding> parser, 
				Func<Token<string>, bool> shouldKeep)
		{
			this.shouldKeep = shouldKeep;
			this.lang = lang;
			this.parser = parser;
			this.tok = new TokenShakerContainer<string>(selector);
		}

		public IEnumerable<Token<string>> GetTokens(string input)
		{
			Hunk<string> data = new Hunk<string>(input, input.Length);
			foreach(var v in tok.TypedShake(data, lang))
				if(shouldKeep(v))
					yield return v;
		}
		private object Parse(IEnumerable<Token<string>> toks, string input)
		{
			return parser.Parse(toks, input);
		}
		public object Parse(string input)
		{
			return Parse(GetTokens(input), input);
		}
		public bool InputIsValid(string input)
		{
			var tags = from x in GetTokens(input)
				         select x.TokenType;
			return parser.IsValid(tags);
		}
		public bool FileContentsAreValid(string fileName)
		{
			return InputIsValid(File.ReadAllText(fileName));
		}
		public object ParseFileContents(string fileName)
		{
			return Parse(File.ReadAllText(fileName));
		}
	}
	public class LR1ParsableLanguage : ParsableLanguage<Rule,int>
	{
		public LR1ParsableLanguage(
				Language lang, 
				TypedShakeSelector<string> selector,
			 	Parser<Rule,int> parser, 
				Func<Token<string>, bool> shouldKeep)
			: base(lang, selector, parser, shouldKeep)
		{
		}

		public LR1ParsableLanguage(
				Language l,
				TypedShakeSelector<string> selector,
				Grammar g,
				string terminateSymbol,
				SemanticRule onAccept,
				bool supressMessages,
				Func<Token<string>, bool> shouldKeep)
			: this(l, selector, 
					new LR1Parser(g, terminateSymbol, onAccept, supressMessages),
					 shouldKeep)
		{
			
		}
		public LR1ParsableLanguage(
				Language l,
				TypedShakeSelector<string> selector,
				Grammar g,
				string terminateSymbol,
				LR1ParsingTable table,
				LR1GotoTable gotoTable,
				SemanticRule onAccept,
				Func<Token<string>, bool> shouldKeep)
			: this(l, selector, 
					new LR1Parser(g, terminateSymbol, table, gotoTable, onAccept),
					 shouldKeep)
		{
			
		}
		
		public LR1ParsableLanguage(
				string name,
			 	string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
				IEnumerable<Word> rest,
			 	TypedShakeSelector<string> selector,
				Grammar g,
				string terminateSymbol,
				SemanticRule onAccept,
				bool supressMessages,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords, rest), selector, g, terminateSymbol, onAccept, supressMessages,
					shouldKeep)
		{
			
		}
		public LR1ParsableLanguage(
				string name,
				string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
				IEnumerable<Word> rest,
			 	TypedShakeSelector<string> selector,
				Grammar g,
				string terminateSymbol,
				LR1ParsingTable table,
				LR1GotoTable gotoTable,
				SemanticRule onAccept,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords, rest), selector, g, terminateSymbol, table, gotoTable, onAccept,
					shouldKeep)
		{

		}
		public LR1ParsableLanguage(
				string name,
				string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
			 	TypedShakeSelector<string> selector,
				Grammar g,
				string terminateSymbol,
				LR1ParsingTable table,
				LR1GotoTable gotoTable,
				SemanticRule onAccept,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords), selector, g, terminateSymbol, table, gotoTable, onAccept, 
					shouldKeep)
		{

		}
				
		public LR1ParsableLanguage(
				string name,
			 	string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
			 	TypedShakeSelector<string> selector,
				Grammar g,
				string terminateSymbol,
				SemanticRule onAccept,
				bool supressMessages,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords), selector, g, terminateSymbol, onAccept, supressMessages,
					shouldKeep)
		{
			
		}
	}
	public class EnhancedLR1ParsableLanguage : ParsableLanguage<Rule,ulong>
	{
		public EnhancedLR1ParsableLanguage(
				Language lang, 
				TypedShakeSelector<string> selector,
			 	Parser<Rule,ulong> parser, 
				Func<Token<string>, bool> shouldKeep)
			: base(lang, selector, parser, shouldKeep)
		{
		}

		public EnhancedLR1ParsableLanguage(
				Language l,
				TypedShakeSelector<string> selector,
				EnhancedGrammar g,
				string terminateSymbol,
				SemanticRule onAccept,
				bool supressMessages,
				Func<Token<string>, bool> shouldKeep)
			: this(l, selector, 
					new EnhancedLR1Parser(g, terminateSymbol, onAccept, supressMessages),
					 shouldKeep)
		{
			
		}
		public EnhancedLR1ParsableLanguage(
				Language l,
				TypedShakeSelector<string> selector,
				EnhancedGrammar g,
				string terminateSymbol,
				EnhancedParsingTable table,
				SemanticRule onAccept,
				Func<Token<string>, bool> shouldKeep)
			: this(l, selector, 
					new EnhancedLR1Parser(g, terminateSymbol, table, onAccept),
					 shouldKeep)
		{
			
		}
		
		public EnhancedLR1ParsableLanguage(
				string name,
			 	string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
				IEnumerable<Word> rest,
			 	TypedShakeSelector<string> selector,
				EnhancedGrammar g,
				string terminateSymbol,
				SemanticRule onAccept,
				bool supressMessages,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords, rest), selector, g, terminateSymbol, onAccept, supressMessages,
					shouldKeep)
		{
			
		}
		public EnhancedLR1ParsableLanguage(
				string name,
				string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
				IEnumerable<Word> rest,
			 	TypedShakeSelector<string> selector,
				EnhancedGrammar g,
				string terminateSymbol,
				EnhancedParsingTable table,
				SemanticRule onAccept,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords, rest), selector, g, terminateSymbol, table, onAccept,
					shouldKeep)
		{

		}
		public EnhancedLR1ParsableLanguage(
				string name,
				string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
			 	TypedShakeSelector<string> selector,
				EnhancedGrammar g,
				string terminateSymbol,
				EnhancedParsingTable table,
				SemanticRule onAccept,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords), selector, g, terminateSymbol, table, onAccept, 
					shouldKeep)
		{

		}
				
		public EnhancedLR1ParsableLanguage(
				string name,
			 	string version,
				string idType,
				IEnumerable<Comment> comments,
				IEnumerable<Symbol> symbols,
				IEnumerable<RegexSymbol> regexSymbols,
				IEnumerable<Keyword> keywords,
			 	TypedShakeSelector<string> selector,
				EnhancedGrammar g,
				string terminateSymbol,
				SemanticRule onAccept,
				bool supressMessages,
				Func<Token<string>, bool> shouldKeep)
			: this(new Language(name, version, idType,
						comments, symbols, regexSymbols,
						keywords), selector, g, terminateSymbol, onAccept, supressMessages,
					shouldKeep)
		{
			
		}
	}
}

