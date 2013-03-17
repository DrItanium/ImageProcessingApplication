using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Extensions;
using Libraries.Collections;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
	public class RegexSymbol : Word, IComparable<RegexSymbol>
	{
		public string Name { get; set; }
		//	public bool CachingIsAllowed { get; protected set; }
		public RegexSymbol(string input, string name, string type)
			: base(input, type)
		{
			Name = name;
		}
		public override ShakeCondition<string> AsShakeCondition()
		{
			Regex currentRegex = new Regex(TargetWord);
			return LexicalExtensions.GenerateRegexCond<string>(
					(x,y,z) => 
					{
					Match m = currentRegex.Match(x,y,z - y);	
					Segment output = m.Success ? new Segment(m.Length, m.Index + y) : null;
					return new Tuple<bool, Segment>(m.Success, output);
					});
		}
		public override TypedShakeCondition<string> AsTypedShakeCondition()
		{
			Regex currentRegex = new Regex(TargetWord);
			return LexicalExtensions.GenerateTypedRegexCond<string>(
					(x,y,z) => 
					{
					//						Console.WriteLine("x = {0}, y = {1}, z = {2}", x, y, z);
					Match m = currentRegex.Match(x,y,z - y);	
					//						Console.WriteLine("m.Length = {0},m.Index = {1}", m.Length, m.Index);
					TypedSegment output = m.Success ? new TypedSegment(m.Length, WordType, m.Index + y) : null;
					//						if(m.Success)
					//						  Console.WriteLine("output = {0}", output);
					return new Tuple<bool, TypedSegment>(m.Success, output);
					});
		}

		public override object Clone()
		{
			return new RegexSymbol(TargetWord, Name, TargetWord);
		}
		public virtual int CompareTo(RegexSymbol other)
		{
			return Name.CompareTo(other.Name) + base.CompareTo((Word)other);
		}

	}
}
