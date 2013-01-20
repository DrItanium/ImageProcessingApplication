using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.Starlight
{
	public class LookaheadRule : AdvanceableRule, ILookaheadRule
	{
		private string lookaheadSymbol;
		string ILookaheadRule.LookaheadSymbol { get { return lookaheadSymbol; } }
		public string LookaheadSymbol 
		{
		 	get
			{
				return lookaheadSymbol;
			}
			set
			{
				lookaheadSymbol = value;
			}
		}
		public LookaheadRule(string lookaheadSymbol, Rule rule, int offset)
			: base(rule, offset)
		{
			this.lookaheadSymbol = lookaheadSymbol;
		}
		public LookaheadRule(string lookaheadSymbol, string name, params AdvanceableProduction[] prods)
			: base(name, prods)
		{
			this.lookaheadSymbol = lookaheadSymbol;
		}
		public LookaheadRule() : this(string.Empty, null, 0)
		{

		}
		public LookaheadRule(string lookaheadSymbol, Rule rule)
			: this(lookaheadSymbol, rule, 0)
		{
		}
		public LookaheadRule(string lookaheadSymbol, AdvanceableRule rule)
			: base(rule)
		{
			this.lookaheadSymbol = lookaheadSymbol;
		}
		public LookaheadRule(LookaheadRule lr)
			: base(lr)
		{
			lookaheadSymbol = lr.lookaheadSymbol;
		}
		public void Repurpose(string lookahead, Rule r)
		{
		   Repurpose(lookahead, new AdvanceableRule(r));
		}
		public void Repurpose(string lookahead, AdvanceableRule r)
		{
			Clear();
			if(!r.Name.Equals(base.Name))
			  base.Name = r.Name;
			if(!lookahead.Equals(LookaheadSymbol))
				lookaheadSymbol = lookahead;
			for(int i = 0; i < r.Count; i++)
				Add(r[i]);
		}
		public override bool Equals(object other)
		{
			LookaheadRule lr = (LookaheadRule)other;
			return lr.lookaheadSymbol.Equals(lookaheadSymbol) && base.Equals(lr);
		}
		private string ToString0(AdvanceableProduction production)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("[{0} => ",Name);
			for(int i = 0;i < production.Min; i++)
				sb.AppendFormat("{0} ", production[i]);	
			sb.Append("!");
			for(int i = production.Min;i < production.Position; i++)
				sb.AppendFormat("{0} ", production[i]);
			sb.Append("@");
			for(int i = production.Position; i < production.Max; i++)
				sb.AppendFormat("{0} ", production[i]);
			sb.AppendFormat(", {0}]", LookaheadSymbol);
			return sb.ToString();
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(var v in this)
				sb.AppendLine(ToString0(v));
			return sb.ToString();
		}

		public override int GetHashCode()
		{
			long l = lookaheadSymbol.GetHashCode() + base.GetHashCode();
			return (int)l;
		}
		public new object Clone()
		{
			return new LookaheadRule(this);
		}
	}
}
