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
	public abstract class Word : IComparable<Word>, ICloneable, IComparable<string>
	{
		public string TargetWord { get; protected set; }
		public string WordType { get; protected set; }
		public Word(string input, string type)
		{
			TargetWord = input;
			WordType = type;
		}

		public Word(string input)
			: this(input, string.Empty)
		{
		}

		public abstract ShakeCondition<string> AsShakeCondition();
		public abstract TypedShakeCondition<string> AsTypedShakeCondition();

		public virtual int CompareTo(Word other)
		{
			return TargetWord.CompareTo(other.TargetWord) + WordType.CompareTo(other.WordType);
		}

		public virtual int CompareTo(string other)
		{
			return TargetWord.CompareTo(other);
		}

		public abstract object Clone();

		public override int GetHashCode()
		{
			return TargetWord.GetHashCode() + WordType.GetHashCode();
		}

		public override bool Equals(object other)
		{
			Word w = (Word)other;
			return TargetWord.Equals(w.TargetWord) && WordType.Equals(w.WordType);
		}

		public override string ToString()
		{
			return string.Format("[{0},{1}]", TargetWord, WordType);
		}

		public static implicit operator ShakeCondition<string>(Word k)
		{
			return k.AsShakeCondition();
		}
	}
}
