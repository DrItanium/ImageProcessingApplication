
//#define DEBUG
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.Collections
{
	public class Segment : ICloneable
	{
		private int start, length, computedStart;
		public int Start
		{
			get
			{
				return start;
			}
			set
			{
				if(start != value)
				  computedStart = value + length;
				start = value;

			}
		}
		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				if(value != length)
					computedStart = value + start;
				length = value;
			}
		}
		public int ComputedStart { get { return computedStart; } }

			/*
		public int Start { get { return start; } set { start = value; } }
		public int Length { get { return length; } set { length = value; } }
		*/
		public Segment(int length, int start)
		{
			this.start = start;
			this.length = length;
		}
		public Segment(int length) : this(length, 0) { }
		public Segment(Segment other) : this(other.length, other.start) { }
		public virtual object Clone()
		{
			return new Segment(this);
		}
		public override bool Equals(object other)
		{
			Segment ot = (Segment)other;
			return ot.length == length && ot.start == start;
		}
		public override int GetHashCode()
		{
			return length.GetHashCode() + start.GetHashCode();
		}
		public override string ToString()
		{
			return string.Format("({0}=>{1})", start, length);
		}
		public string Substring(string input)
		{
			return input.Substring(start, length);
		}
	}
}
