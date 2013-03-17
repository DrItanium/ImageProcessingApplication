using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
    public class Language : IEnumerable<TypedShakeCondition<string>>, IEnumerable<Word>
    {
        private List<Comment> comments;
        private List<Symbol> symbols;
        private List<RegexSymbol> regexSymbols;
        private StringOrganizer keywords;
        private List<Word> customActions;
				private IdSymbol id;
        public string Name { get; protected set;  }
        public string Version { get; protected set; }
        public string Description { get; protected set; }
        public Language(string name, string version, string idType,
						IEnumerable<Comment> comments, 
						IEnumerable<Symbol> symbols, 
						IEnumerable<RegexSymbol> regexSymbols, 
						IEnumerable<Keyword> keywords, 
						IEnumerable<Word> rest)
        {
            this.comments = new List<Comment>(comments);
            this.symbols = new List<Symbol>(symbols);
            this.regexSymbols = new List<RegexSymbol>(regexSymbols);
            this.keywords = new StringOrganizer(keywords);
            this.customActions = new List<Word>(rest);
						id = new IdSymbol(idType);
            Name = name;
            Version = version;
        }
        public Language(string name, string version, string idType, IEnumerable<Comment> comments, IEnumerable<Symbol> symbols, IEnumerable<RegexSymbol> regexSymbols, IEnumerable<Keyword> keywords)
            : this(name, version, idType, comments, symbols, regexSymbols, keywords, new Word[0])
        {

        }

        public void AddCustomAction(Word customAction)
        {
            customActions.Add(customAction);
        }
        private static IEnumerable<TypedShakeCondition<string>> Convert(IEnumerable<Word> words)
        {
            return (from x in words
                    select x.AsTypedShakeCondition());
        }

        public IEnumerator<TypedShakeCondition<string>> GetEnumerator()
        {
            var a = Convert(comments);
            var b = Convert(symbols);
            var c = Convert(regexSymbols);
            var d = keywords.GetTypedShakeConditions();
            var e = Convert(customActions);
						var idy = new[] { id.AsTypedShakeCondition() };
            return (a.Concat(c).Concat(b).Concat(d).Concat(e).Concat(idy)).GetEnumerator();
        }
        IEnumerator<Word> IEnumerable<Word>.GetEnumerator()
        {
            
            IEnumerable<Word> a = (comments);
            IEnumerable<Word> b = (symbols);
            IEnumerable<Word> c = (regexSymbols);
            IEnumerable<Word> d = keywords;
            IEnumerable<Word> e = (customActions);
            return a.Concat(c).Concat(b).Concat(d).Concat(e).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
