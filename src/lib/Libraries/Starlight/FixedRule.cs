using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.Starlight
{
	public abstract class GenericFixedRule : IFixedRule
	{
		protected abstract IProduction Current_Impl { get; }
		public abstract int Count { get; }
		public abstract int Offset { get; }
		public abstract string Name { get; }
		public IProduction Current { get { return Current_Impl; } }
		public abstract IProduction this[int index] { get; }
		IProduction IRule.this[int index] { get { return this[index]; } }
		public IEnumerable<IProduction> GetEnumerableInterface()
		{
			yield return Current_Impl;
		}
		public abstract object Clone();
	}
	public class FixedRule : GenericFixedRule 
	{
		private IRule targetRule;
		private int offset;
		string IRule.Name { get { return Name; } }
		public override string Name { get { return targetRule.Name; } }
		public override int Count { get { return targetRule.Count; } }
		public override int Offset { get { return offset; } }
		public new IProduction Current { get { return targetRule[offset]; } }
		protected override IProduction Current_Impl { get { return Current; } }
		public FixedRule(IRule targetRule, int offset)
		{
			this.targetRule = targetRule;
			this.offset = offset;
		}
		public FixedRule(FixedRule r) : this(r.targetRule, r.offset) { }
		IProduction IRule.this[int index] { get { return targetRule[index]; } }
		public override IProduction this[int index] { get { return targetRule[offset]; } }
		public override object Clone()
		{
		  return new FixedRule(this);
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode() + Current.GetHashCode();
		}
		public override bool Equals(object other)
		{
			if(other is IFixedRule)
			{
			  IFixedRule r = (IFixedRule)other;
				return r.Name.Equals(Name) && r.Current.Equals(Current);
			}
			else
			{
				IRule rr = (IRule)other;
				return rr.Name.Equals(Name) && rr[offset].Equals(Current);
			}
		}
	}
	public abstract class IntermediaryFixedRule : GenericFixedRule
	{
		public override string Name { get { return Name_Impl; } }
		protected abstract string Name_Impl { get; }
	}
	public class SingularRule : IntermediaryFixedRule 
	{
		private IProduction p;
		private string name; 
		public new IProduction Current { get { return p; } set { p = value; } }
		public new string Name { get { return name; } set { name = value; } }
		protected override string Name_Impl { get { return Name; } }
		public override int Count { get { return p == null ? 0 : 1; } }
		public override int Offset { get { return 0; } }
		IProduction IFixedRule.Current { get { return Current; } }
		protected override IProduction Current_Impl { get { return Current; } }
		
		public override IProduction this[int index] { get { return p; } }
		IProduction IRule.this[int index] { get { return p; } }
		public SingularRule() { }
		public SingularRule(string name, IProduction prod)
	 	{
			this.name = name;
			this.p = prod;
	 	}
		public override int GetHashCode()
		{
			return Name.GetHashCode() + Current.GetHashCode();
		}
		public override bool Equals(object other)
		{
			 IFixedRule r = (IFixedRule)other;
			 return r.Name.Equals(Name) && r.Current.Equals(Current);
		}
		public override object Clone()
		{
			return new SingularRule(name, p);
		}
	}
}
