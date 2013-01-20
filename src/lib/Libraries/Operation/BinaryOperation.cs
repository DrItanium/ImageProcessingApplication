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
	public class BinaryOperation : BooleanOperation
	{
		public override Operation First()
		{
			//we don't want the first symbol of the binary grouping,
			//this resolves a granularity issue
			return this;
		}
		public BinaryOperation(StandardOperation op0,
				string operation, StandardOperation op1)
			: base(op0, operation, op1)
		{

		}
		public override void InitialFulfill()
		{
			//we don't actually register this
			RegisterHook((x) => 
					{
//					Console.WriteLine("OnFalse is now {0}, first is {1}", x.Index, x.First().Index);
				 	OnFalseHook = x.First();
				 	});
		}
		public override void Enumerate(IGraphBuilder builder)
		{
			Index = builder.Index(this); //lets not even index the components
		}
		public override void Build(IGraphBuilder builder)
		{
			//do nothing 
			//we can do the linkage here
			if(OnFalseHook != null)
				builder.Link(Index, OnFalseHook.First().Index, "[label = \"f\"]");
			if(OnTrueHook != null)
				builder.Link(Index, OnTrueHook.First().Index, "[label = \"t\"]");
		}
	}
}
