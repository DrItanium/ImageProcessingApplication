using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.Starlight
{
	public class AdvanceableRule : List<AdvanceableProduction>, ICloneable, 
	IEqualityComparer<AdvanceableRule>, IAdvanceableRule
	{
		private long oldHashCode = 0L;
		private int oldLength = 0;
#if GATHERING_STATS
		public static long InstanceCount = 0L, DeleteCount = 0L;
		//	public static HashSet<TimeSpan> Lifetimes = new HashSet<TimeSpan>();
		//	private DateTime bornOn;
#endif
		private string name;
		public string Name 
		{
			get
			{
				return name;
			}
			protected set
			{
				name = value;
			}
		}
		public AdvanceableRule(Rule r, int offset)
		{
#if GATHERING_STATS
			InstanceCount++;
			//		bornOn = DateTime.Now;
#endif
			if(r != null)
			{
				name = r.Name;
				for(int i = 0; i < r.Count; i++)
					Add(r[i].MakeAdvanceable(offset));
			}

		}	
		public AdvanceableRule(Rule r) : this(r, 0) { }
		public AdvanceableRule(string name)
		{
#if GATHERING_STATS
			InstanceCount++;
			//		bornOn = DateTime.Now;
#endif
			this.name = name;
		}
		public AdvanceableRule(string name, params AdvanceableProduction[] prods)
			: this(name)
		{
			for(int i = 0; i < prods.Length; i++)
				Add(prods[i]);
		}
		public AdvanceableRule(string name, IEnumerable<AdvanceableProduction> prods)
			: this(name)
		{
			foreach(var v in prods)
				Add(v);
		}
		public AdvanceableRule(AdvanceableRule rule)
		{
#if GATHERING_STATS
			InstanceCount++;
			//		bornOn = DateTime.Now;
#endif
			name = rule.name;
			for(int i = 0; i < rule.Count; i++)
				Add(rule[i]); //lets see what this does...
		}
#if GATHERING_STATS
		~AdvanceableRule()
		{
			DeleteCount++;
			//		Lifetimes.Add(DateTime.Now - bornOn);
		}
#endif
		IAdvanceableProduction IAdvanceableRule.this[int ind] { get { return (IAdvanceableProduction)this[ind]; } }
		IProduction IRule.this[int ind] { get { return (IProduction)this[ind]; } }
		IEnumerable<IProduction> IRule.GetEnumerableInterface()
		{
			return ((IAdvanceableRule)this).GetEnumerableInterface();
		}
		IEnumerable<IAdvanceableProduction> IAdvanceableRule.GetEnumerableInterface()
		{
			return ((IEnumerable<AdvanceableProduction>)this);
		}
		public object Clone() 
		{
			return new AdvanceableRule(this);
		}
		protected bool Equals(AdvanceableRule rr)
		{
			if(!rr.name.Equals(name) || rr.Count != Count)
				return false;
			else
			{
				for(int i = 0; i < Count; i++)
					if(!this[i].Equals(rr[i]))
						return false;
				return true;
			}
		}
		bool IAdvanceableRule.Equals(object other)
		{
			AdvanceableRule rr = (AdvanceableRule)other;
			return Equals(rr);
		}
		int IAdvanceableRule.GetHashCode()
		{
			return GetHashCode0();
		}
		public override bool Equals(object other)
		{
			AdvanceableRule rr = (AdvanceableRule)other;
			return Equals(rr);
		}
		private int GetHashCode0()
		{
			if(oldLength != Count)
			{
				long total = name.GetHashCode();
				for(int i = 0; i < Count; i++)
					total += this[i].GetHashCode();
				oldHashCode = total;
				oldLength = Count;
			}
			return (int)oldHashCode;
		}
		bool IEqualityComparer<AdvanceableRule>.Equals(AdvanceableRule x, AdvanceableRule y)
		{
			return x.Equals(y);
		}
		int IEqualityComparer<AdvanceableRule>.GetHashCode(AdvanceableRule x)
		{
			return x.GetHashCode0();
		}
		public override int GetHashCode()
		{
			return GetHashCode0();
		}
		public AdvanceableRule FunctionalNext()
		{
			var clone = this.Clone() as AdvanceableRule;
			clone.Next();
			return clone;
		}
		public AdvanceableRule FunctionalPrevious()
		{
			var clone = this.Clone() as AdvanceableRule;
			clone.Previous();
			return clone;	
		}
		public void Next()
		{
			for(int i = 0; i < Count ; i++)
				if(this[i].HasNext)
					this[i].Next();
		}
		public void Previous()
		{
			for(int i = 0; i < Count; i++)
				if(this[i].HasPrevious)
					this[i].Previous();
		}
		public void Reset()
		{
			for(int i = 0; i < Count; i++)
				this[i].Reset();
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < Count; i++)
				sb.AppendFormat("[{0} => {1}]\n", name, this[i].ToString());
			return sb.ToString();
		}
		public IEnumerable<AdvanceableRule> Subdivide()
		{
			for(int i = 0; i < Count; i++)
				yield return new AdvanceableRule(name) { this[i] };
		}
		public void Repurpose(Rule target)
		{
			Repurpose(new AdvanceableRule(target));
		}
		public void Repurpose(AdvanceableRule target)
		{
			Clear();
			Repurpose(target.name);
			//	if(!target.Name.Equals(Name))
			//	  Name = target.Name;
			for(int i = 0; i < target.Count; i++)
				Add(target[i]);
		}
		public void Repurpose(string name, params AdvanceableProduction[] prods)
		{
			Repurpose(name);
			Repurpose(prods);
		}
		public void Repurpose(params AdvanceableProduction[] prods)
		{
			Clear();
			for(int i = 0; i < prods.Length; i++)
				Add(prods[i]);
		}
		public void Repurpose(string name)
		{
			if(!name.Equals(this.name))
				this.name = name;
		}
	}
}
