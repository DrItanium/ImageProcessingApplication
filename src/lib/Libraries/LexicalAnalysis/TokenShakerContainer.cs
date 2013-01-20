using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libraries.Collections;
using Libraries.Extensions;

namespace Libraries.LexicalAnalysis
{
	public class TokenShakerContainer<T> : ShakerContainer<T>
	{
		private TypedShakeSelector<T> selector;
		public new TypedShakeSelector<T> Selector
		{
			get
			{
				return selector;
			}
			set
			{
				this.selector = value;
				base.Selector = (x, y) => CompatibilitySelector(x, y, selector);

			}
		}
		private static Tuple<Hunk<T>, Hunk<T>> CompatibilitySelector(Segment seg, Hunk<T> hunk, TypedShakeSelector<T> selector)
		{
			var r = selector(new TypedSegment(seg, string.Empty), new Token<T>(hunk));
			return new Tuple<Hunk<T>, Hunk<T>>(r.Item1, r.Item2);
		}
		public TokenShakerContainer(TypedShakeSelector<T> selector)
			: base(null) //compat 
		{
			Selector = selector;
		}
		public IEnumerable<Token<T>> TypedShake(Token<T> target, TypedShakeCondition<T> cond)
		{
			return TypedShake(target, cond, Selector);
		}
		public IEnumerable<Token<T>> TypedShake(Token<T> target, TypedShakeCondition<T> cond, TypedShakeSelector<T> selector)
		{
			Token<T> prev = target;
			Tuple<Token<T>, Token<T>, Token<T>> curr = new Tuple<Token<T>, Token<T>, Token<T>>(null, null, target);
			//Console.WriteLine("Yes this is getting called!");
			do
			{
				curr = BasicTypedShakerFunction(curr.Item3, cond, selector);
				if (curr.Item1 != null)
				{
					if(!curr.Item1.IsBig)
						curr.Item1.IsBig = true;
					yield return curr.Item1;
				}
				if (curr.Item2 != null)
					yield return curr.Item2;
				if (curr.Item3.Equals((Hunk<T>)prev))
				{
					
					if (curr.Item3.Length > 0)
					{
						if(!curr.Item3.IsBig)
							curr.Item3.IsBig = true;
						yield return curr.Item3;
					}
					yield break;
				}
				prev = curr.Item3;
			} while (true);
		}
		public IEnumerable<Token<T>> TypedShake(Token<T> target, TypedShakeCondition<T> a, TypedShakeCondition<T> b)
		{
			foreach (var v in TypedShake(target, a))
			{
				if (v.IsBig)
					foreach (var q in TypedShake(v, b))
						yield return q;
				else
					yield return v;
			}
		}
		private IEnumerable<Token<T>> TypedShakeInternal(IEnumerable<Token<T>> outer, TypedShakeCondition<T> cond)
		{
			if (cond == null)
			{
				foreach (var v in outer)
					yield return v;
			}
			else
			{
				foreach (var v in outer)
				{
					if (v.IsBig)
					{
						
						foreach (var q in TypedShake(v, cond))
							yield return q;
					}
					else
						yield return v;
				}
			}
		}
		private IEnumerable<Token<T>> TypedShakeSingle(Token<T> tok) 
		{
			return new Token<T>[] { tok };
		}
		public IEnumerable<Token<T>> TypedShake(Hunk<T> input, IEnumerable<TypedShakeCondition<T>> conds)
		{
			return TypedShake(new Token<T>(input), conds);
		}
		public IEnumerable<Token<T>> TypedShake(Token<T> target, IEnumerable<TypedShakeCondition<T>> conds)
		{
			IEnumerable<Token<T>> initial = TypedShake(target, conds.First()).ToArray();
			foreach (var cond in conds.Skip(1))
				initial = TypedShakeInternal(initial, cond);
			return TypedShakeInternal(initial, null);
		}

		public Tuple<Token<T>, Token<T>, Token<T>> BasicTypedShakerFunction(Token<T> target, TypedShakeCondition<T> cond)
		{
			return BasicTypedShakerFunction(target, cond, selector);
		}
		protected Tuple<Token<T>, Token<T>, Token<T>> BasicTypedShakerFunction(
				Token<T> target,
				TypedShakeCondition<T> cond,
				TypedShakeSelector<T> selector)
		{
			Func<TypedSegment, Token<T>, int> getRest = (seg, hunk) => (hunk.Length - (seg.Start + seg.Length));
			Func<TypedSegment, int> getComputedStart = (seg) => seg.Start + seg.Length;

			TypedSegment result = cond(target);
			if (result == null)
			{
				return new Tuple<Token<T>, Token<T>, Token<T>>(null, null, target);
			}
			else
			{
				TypedSegment restSection = new TypedSegment(getRest(result, target), string.Empty, getComputedStart(result));
				var matchTokens = selector(result, target);
				var before = matchTokens.Item1;
				var match = matchTokens.Item2;
				//Console.WriteLine("\t\tIncoming match = {0}", match);
				match.IsBig = false;
				var restTokens = selector(restSection, target);
				var rest = restTokens.Item2;
				rest.IsBig = true;
				return new Tuple<Token<T>, Token<T>, Token<T>>(before, match, rest);
			}
		}
	}
	public delegate Tuple<Token<T>, Token<T>> TypedShakeSelector<T>(TypedSegment seg, Token<T> token);
	public delegate TypedSegment TypedShakeCondition<T>(Token<T> hunk);
	public delegate Tuple<Token<T>, Token<T>, Token<T>> TypedShaker<T>(Hunk<T> target,
			TypedShakeCondition<T> condition, TypedShakeSelector<T> selector);
	/*
		 public delegate Segment ShakeCondition<T>(Hunk<T> hunk);
		 public delegate Tuple<Hunk<T>, Hunk<T>> ShakeSelector<T>(Segment seg, Hunk<T> target);
		 public delegate Tuple<Hunk<T>, Hunk<T>, Hunk<T>> Shaker<T>(Hunk<T> target,
		 ShakeCondition<T> condition, ShakeSelector<T> selector);
	 */
}
