using System;
using System.Collections.Generic;
using System.Linq;
using Libraries.Collections;

namespace Libraries.Parsing
{
	/*
		 Lets define an encoding scheme of 64-bits
		 Highest 8-bits for the action
		 56-bits left
		 Depending on that value different encoding occurs
		 For Accept nothing is parsed
		 For Error, 2^56 max error states are allowed
		 For Reduce, divided the remaining bits into two sections (each 28-bits each)
		 upper portion is for the rule number
		 lower portion is for the production number
		 For Goto, the remaining 56 bits are used to denote the state to go to
		 For Shift, the remaining 56 bits are used to denote the state to go to
	 */
	public enum TableCellAction : byte
	{
		Error = 0,
					Accept = 1,
					Reduce = 2,
					Shift = 3,
					Goto = 4,
	}
	public class EnhancedParsingTable : Dictionary<string, CompressionList<ulong>>
	{
		public EnhancedParsingTable(IEnumerable<string> titles, ulong defaultValue)
		{
			foreach(var v in titles)
				this[v] = new CompressionList<ulong>(defaultValue);

		}
		public EnhancedParsingTable(IEnumerable<string> titles)
			: this(titles, 0L)
		{

		}
	}
}
