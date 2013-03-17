using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading;
using Libraries.Extensions;

namespace Libraries.Collections
{
	public class Container<T>
	{
		public T Value1 { get; set; }
		public Container(T value)
		{
			Value1 = value;
		}
		public Container() : this(default(T))
		{

		}
		public override bool Equals(object other)
		{
			Container<T> si = (Container<T>)other;
			return si.Value1.Equals(Value1);
		}
		public override int GetHashCode()
		{
			return Value1.GetHashCode();
		}
		public override string ToString()
		{
			return Value1.ToString();
		}
	}
	public class Container<T1,T2> : Container<T1>
	{
		public T2 Value2 { get; set; }
		public Container(T1 value1, T2 value2)
			: base(value1)
		{
			Value2 = value2;
		}
		public Container() : base()
		{

		}
		public override bool Equals(object other)
		{
			Container<T1,T2> c = (Container<T1,T2>)other;
			return c.Value2.Equals(Value2) && c.Value1.Equals(Value1);
		}
		public override string ToString()
		{
			return string.Format("[{0},{1}]", Value1, Value2);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode() + Value2.GetHashCode();
		}
	}
	public class Container<T1,T2,T3> : Container<T1,T2>
	{
		public T3 Value3 { get; set; }
		public Container(T1 v1, T2 v2, T3 v3)
			: base(v1,v2)
		{
			Value3 = v3;
		}
		public Container() : base() 
		{

		}
		public override int GetHashCode()
		{
			return base.GetHashCode() + Value3.GetHashCode();
		}
		public override bool Equals(object other)
		{
			Container<T1,T2,T3> c = (Container<T1,T2,T3>)other;
			return base.Equals(other) && c.Value3.Equals(Value3);
		}
		public override string ToString()
		{
			return string.Format("[{0},{1},{2}]", Value1, Value2, Value3);
		}
	}
}
