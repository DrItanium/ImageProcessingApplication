using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;


namespace Libraries.Starlight
{
	public class AdvanceableProduction : ICloneable, IEnumerable<string>, IAdvanceableProduction
	{
#if GATHERING_STATS
		public static long DeleteCount = 0L;
		public static long InstanceCount = 0L;
#endif
		private int current, min;
		private Production prod;

		public int Max { get { return prod.Count; } }
		public int Min { get { return min; } }
		public int Position { get { return current; } }
		public SemanticRule Rule { get { return prod.Rule; } }
		public string Current 
		{ 
			get
			{
				if(current < Max && current >= min)
					return prod[current]; 
				else 
					return null;
			}
		}
		public int Count { get { return prod.Count; } }
		public Production Target { get { return prod; } }
		public bool HasNext { get { return current < Max; } }
		public bool HasPrevious { get { return current >= min; } }


		public AdvanceableProduction(Production p, int start)
		{
#if GATHERING_STATS
			InstanceCount++;
#endif
			current = start;
			min = start;
			prod = p;
		}
		public AdvanceableProduction(Production p) : this(p, 0) { }
		public AdvanceableProduction(AdvanceableProduction adv) 
			: this(adv.Target, adv.Min)
		{
			current = adv.Position;
		}
#if GATHERING_STATS
		~AdvanceableProduction()
		{
			DeleteCount++;

		}
#endif
		public object Clone()
		{
			return new AdvanceableProduction(this);
		}
		public override bool Equals(object other)
		{
			AdvanceableProduction p = (AdvanceableProduction)other;
			return p.current == current && p.min == min && p.Max == Max &&
				p.prod.Equals(this.prod);
		}
		public override int GetHashCode()
		{
			return min.GetHashCode() + current.GetHashCode() + Max.GetHashCode() + prod.GetHashCode();
		}
		public string this[int index] 
		{
			get
			{
				if(index < min || index > Max)
					throw new ArgumentException("Given index is out of range");
				else
					return prod[index];
			}
		}
		IAdvanceableProduction IAdvanceableProduction.FunctionalNext()
		{
			return FunctionalNext();
		}
		IAdvanceableProduction IAdvanceableProduction.FunctionalPrevious()
		{
			return FunctionalPrevious();
		}
		public AdvanceableProduction FunctionalNext()
		{
			var adp = (AdvanceableProduction)Clone();
			adp.Next();
			return adp;
		}
		public AdvanceableProduction FunctionalPrevious()
		{
			var adp = (AdvanceableProduction)Clone();
			adp.Previous();
			return adp;
		}
		public bool Next()
		{
			bool result = HasNext;
			if(result)
				current++;
			return result && current < Max;
		}
		public bool Previous()
		{
			bool result = HasPrevious;
			if(result)
				current--;
			return result && current >= min;
		}
		public void Reset()
		{
			current = min;
		}
		public IEnumerator<string> GetEnumerator()
		{
			return new Enumerator(this);
		}	
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator<string>
		{
#if GATHERING_STATS
			public static long InstanceCount = 0L,
						 DeleteCount = 0L;
#endif
			private int min;
			private int start;
			private int max;
			private Production target;
			public Enumerator(AdvanceableProduction prod)
			{
#if GATHERING_STATS
				InstanceCount++;
#endif
				min = prod.Min;
				start = prod.Min - 1;
				max = prod.Max;
				target = prod.prod;
			}
#if GATHERING_STATS
			~Enumerator()
			{
				DeleteCount++;
			}
#endif
			object IEnumerator.Current { get { return Current; } }
			public string Current { get { return target[start]; } }
			public bool MoveNext()
			{
				start++;
				return start < max;
			}	
			public void Reset()
			{
				start = min - 1;
			}
			public void Dispose()
			{
				min = -1;
				start = -1;
				max = -1;
				target = null;
			}
		}
		public static implicit operator Production(AdvanceableProduction prod)
		{
			return prod.prod;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < Min; i++)
				sb.Append(prod[i]);
			sb.Append("!");
			for(int i = Min; i < current; i++)
				sb.AppendFormat("{0} ", prod[i]);
			sb.Append("@");
			for(int i = current; i < Max; i++)
				sb.AppendFormat("{0} ", prod[i]);
			return sb.ToString();
		}
	}
}
