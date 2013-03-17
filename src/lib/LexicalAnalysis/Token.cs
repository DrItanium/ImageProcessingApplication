using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Collections;

namespace Libraries.LexicalAnalysis
{
    public class Token<T> : Hunk<T>
    {
        public string TokenType { get; set; }
        public Token(T value, string tokenType, int size, int offset, bool isBig)
            : base(value, size, offset, isBig)
        {
					//Console.WriteLine("TokenType = {0}", tokenType);
            TokenType = tokenType;
        }
        public Token(T value, string tokenType, int size, int offset)
            : this(value, tokenType, size, offset, false)
        {

        }
        public Token(T value, string tokenType, int size)
            : this(value, tokenType, size, 0)
        {

        }
        public Token(Hunk<T> hunk)
            : this(hunk.Value, string.Empty, hunk.Length, hunk.Start, hunk.IsBig)
        {
        }
        public override bool Equals(object other)
        {
            Token<T> tok = (Token<T>)other;
            return (tok.TokenType.Equals(TokenType)) && base.Equals(tok);
        }
        public bool Equals(Hunk<T> other)
        {
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + TokenType.GetHashCode();
        }
    }
}
