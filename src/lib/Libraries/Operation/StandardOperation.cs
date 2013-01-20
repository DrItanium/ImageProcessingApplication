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
	public class StandardOperation : Operation
	{
		public override Operation Coalesce()
		{
			return this;
		}
		public override string ToString()
		{
			return GetType().ToString();
		}
		public override Operation First()
		{
			return this;
		}
		public override void InitialFulfill()
		{
			RegisterHook((x) => 
					{
					//Console.WriteLine("StandardOperation, initial fulfill, index = {0}", x.Index);	 
					OnTrueHook = (x is StandardOperation) ? x : x.First();
					});
		}
		public override void Build(IGraphBuilder builder)
		{
			if(OnTrueHook != null)
				builder.Link(Index, OnTrueHook.Index, string.Empty);
		}
	}
}
