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
	public class Comment : Word, IComparable<Comment>
	{
		public string CantHaveBefore { get; set; }
		public string TerminateSymbol { get; set; }
		public Comment(string type, string startSymbol, string terminateSymbol, string cantHaveBefore)
			: base(startSymbol, type)
		{
			TerminateSymbol = terminateSymbol;	
			CantHaveBefore = cantHaveBefore;	
		}
		public virtual int CompareTo(Comment cmt)
		{
			return TerminateSymbol.CompareTo(cmt.TerminateSymbol) +
				base.CompareTo((Word)cmt);
		}
		public override object Clone()
		{
			return new Comment(WordType, TargetWord, TerminateSymbol, CantHaveBefore);
		}
		public override ShakeCondition<string> AsShakeCondition()
		{
			return LexicalExtensions.GenerateCond<string>(GenerateSingularDualMatch);
		}

		public override TypedShakeCondition<string> AsTypedShakeCondition()
		{
			return LexicalExtensions.GenerateTypedCond<string>(GenerateSingularTypedDualMatch);
		}
		public Tuple<bool, Segment> GenerateSingularDualMatch(string x, int start, int length)
		{
			Segment result = GenerateSingularDualMatch0(x,start, length);
			return new Tuple<bool, Segment>(result != null, result);
		}
		public Tuple<bool, TypedSegment> GenerateSingularTypedDualMatch(string x, int start, int length)
		{
			TypedSegment result = (TypedSegment)GenerateSingularDualMatch0(x, start, length, true);
			return new Tuple<bool, TypedSegment>(result != null, result);
		}
		private Segment GenerateSingularDualMatch0(string x, int start, int length)
		{
			return GenerateSingularDualMatch0(x, start, length, false);
		}
		private Segment GenerateSingularDualMatch0(string x, int start, int length, bool makeTyped)
		{
			//Console.WriteLine("Called GenerateSingularDualMatch0 with arguments ({0}, {1}, {2})", x, start, length);
			string t0 = TargetWord;
			string t1 = TerminateSymbol;
			string sec = x.Substring(new Segment(length - start, start));
			int index = sec.IndexOf(t0);
			if (index == -1)
				return null;
			//Console.WriteLine("\tindex = {0}", index);
			string before = string.Empty;
			if(index == 0 && start != 0)
				before = x.Substring(0,start);
			else 
				before = sec.Substring(0,index);
			//Console.WriteLine("\tBefore = {0}", before);
			if(before.EndsWith(CantHaveBefore))
				return null;
			int indMod = index + t0.Length;
			//Console.WriteLine("\tindex + t0.Length = {0}", indMod);
			//Console.WriteLine("\tsec.Length = {0}", sec.Length);
			Segment ss = new Segment(sec.Length - indMod, indMod);
			//Console.WriteLine("\tss = {0}", ss);
			string sec2 = sec.Substring(ss);
			int index2 = sec2.IndexOf(t1);
			if (index2 == -1)
				return null; //legal
			if (makeTyped)            
				return new TypedSegment(index2 + t1.Length + t0.Length, WordType, index + start);
			else
				return new Segment(index2 + t1.Length + t0.Length, index + start);

		}
	}
}

