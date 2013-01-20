using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Frameworks.Operation
{
	public class Block : Operation
	{
		public Block()
		{

		}
		public override Operation First()
		{
			return this[0];
		}
		public override void InitialFulfill()
		{
			for(int i = 0; i < Count - 1; i++)
			{
				this[i].InitialFulfill();
				this[i].ResolveHooks(this[i + 1].First());

			}
			this[Count - 1].InitialFulfill();
			RegisterHook((x) => { this[Count - 1].ResolveHooks(x); });
			RegisterHook((x) => { this[Count - 1].OnTrueHook = x; });
		}
		public override Operation Coalesce()
		{
			Block outputBlock = new Block();
			outputBlock.OnTrueHook = OnTrueHook;
			outputBlock.OnFalseHook = OnFalseHook;
			int amountSeen = 0;
			foreach(var v in this)
			{
				if(v is StandardOperation)
				{
					amountSeen++;
				}	
				else
				{
					if(amountSeen > 0)
					{
						outputBlock.Add(new StandardOperation()); 
						amountSeen = 0;
					}
					outputBlock.Add(v.Coalesce());
				}
			}
			if(amountSeen > 0) //final list of operations
				outputBlock.Add(new StandardOperation());
			return outputBlock;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(GetType().ToString());
			foreach(var v in this)
				sb.AppendLine(v.ToString());
			return sb.ToString();
		}
		public override void Enumerate(IGraphBuilder builder)
		{
			base.Enumerate(builder);
			foreach(var v in this)
				v.Enumerate(builder);
		}
		public override void Build(IGraphBuilder builder)
		{
			foreach(var v in this)
			{
				v.Build(builder); //thats it :)
			}
			//get the last item
		}
	}
}
