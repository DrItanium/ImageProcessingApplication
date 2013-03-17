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
	public class Rule : List<Production>, ICloneable, IRule
	{
#if GATHERING_STATS
		public static long InstanceCount = 0, DeleteCount = 0;
#endif
		private string name;
		public string Name { get { return name; } set { name = value; } }
		public Rule(string name)
		{
#if GATHERING_STATS
			InstanceCount++;
#endif
			this.name = name;
		}
		public Rule(Rule r)
			: this(r.Name)
		{
			foreach(var v in r)
				Add(v); //why do we need copies?
		}
#if GATHERING_STATS
		~Rule()
		{
			DeleteCount++;
		}
#endif
		public override bool Equals(object other)
		{
			Rule r = (Rule)other;
			if(r.Name.Equals(Name) && r.Count == Count)
			{
				for(int i = 0; i < Count; i++)
					if(!r[i].Equals(this[i]))
						return false;
				return true;
			}
			else
				return false;
		}
		public override int GetHashCode()
		{
			long total = 0L;
			for(int i = 0; i < Count; i++)
				total += this[i].GetHashCode();
			return Name.GetHashCode() + (int)total;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0} => ", Name);
			foreach(Production p in this)
				sb.AppendFormat("{0} | ",p.ToString());
			return sb.ToString();
		}
		public AdvanceableRule MakeAdvanceable(int offset)
		{
			return new AdvanceableRule(this, offset);
		}
		public AdvanceableRule MakeAdvanceable()
		{
			return new AdvanceableRule(this);
		}
#if GATHERING_STATS
		public static long SubdivideCalls = 0L;
#endif
		public IEnumerable<IFixedRule> Subdivide()
		{
#if GATHERING_STATS
			SubdivideCalls++;
#endif
			List<IFixedRule> fr = new List<IFixedRule>();
			for(int i = 0; i < Count; i++)
				fr.Add(new FixedRule(this, i));
			return fr;
				//yield return new FixedRule(this, i);
		}
		public object Clone()
		{
			return new Rule(this);
		}
		private IEnumerable<Production> GetValid()
		{
			return (IEnumerable<Production>)this;
		}
		IEnumerable<IProduction> IRule.GetEnumerableInterface()
		{
			return (from x in this 
					    select (IProduction)x);
		}	
		IProduction IRule.this[int index] { get { return this[index]; } }
	}
}
