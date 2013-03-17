using System;
using System.Collections;
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
	public class Production : List<string>, ICloneable, IProduction
	{
#if GATHERING_STATS
		public static int InstanceCount = 0, DeleteCount = 0;
#endif
		private int oldSize = 0;
		private long cachedHashCode = 0L;
		protected static readonly Regex SpaceSplitter = new Regex(" ");
		private SemanticRule rule;
		public SemanticRule Rule { get { return rule; } protected set { rule = value; } }
		public Production(IEnumerable<string> input)
		{
#if GATHERING_STATS
			InstanceCount++;
#endif
			foreach(var i in input)
				Add(i);
		}
		public Production() : this((x) => x)
		{
		}
		public Production(SemanticRule rule)
			: base()
		{
#if GATHERING_STATS
			InstanceCount++;
#endif
			this.rule = rule;

		}
		public Production(Production p)
			: this()
		{
			this.rule = p.rule;
			for(int i = 0; i < p.Count; i++)
				Add(p[i]);
		}
#if GATHERING_STATS
		~Production()
		{
			DeleteCount++;
		}
#endif
		public override bool Equals(object other)
		{
			Production oth = (Production)other;
			if(oth.Count != Count)
				return false;
			else
			{
				for(int i = 0; i < oth.Count; i++)
					if(!oth[i].Equals(this[i]))
						return false;
				return true;
			}	
		}

		public override int GetHashCode()
		{
			if(oldSize != Count)
			{
				long total = 0L;
				for(int i = 0; i < Count; i++)
					total += this[i].GetHashCode();
				cachedHashCode = total;
				oldSize = Count;
			}
			return (int)cachedHashCode;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < Count; i++)
				sb.AppendFormat("{0} ", this[i]);
			return sb.ToString();
		}
		public AdvanceableProduction MakeAdvanceable(int offset)
		{
			return new AdvanceableProduction(this, offset);
		}
		public AdvanceableProduction MakeAdvanceable()
		{
			return new AdvanceableProduction(this);
		}
		public object Clone()
		{
			return new Production(this);
		}
	}
}
