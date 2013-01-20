//#define DEBUG
#undef NET4
#define NET35
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
	public class Hunk<T> : Segment
	{
		public bool IsBig { get; set; }
		public T Value { get; set; }

		public Hunk(T value, int size, int offset = 0, bool isBig = true)
			: base(size, offset)
		{
			Value = value;
			IsBig = isBig;
		}

		public override bool Equals(object other)
		{
			Hunk<T> ot = (Hunk<T>)other;
			return (ot.IsBig == IsBig) && (ot.Value.Equals(Value)) && base.Equals(ot);
		}

		public override string ToString()
		{
			return string.Format("{0}",Value.ToString());
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode() + IsBig.GetHashCode() + base.GetHashCode();
		}
	}
    
}
