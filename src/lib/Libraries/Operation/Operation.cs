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
	public delegate void UnfulfilledHookResolver(Operation next);
	public abstract class Operation : List<Operation>
	{
		public int Index { get; set; }
		private Queue<UnfulfilledHookResolver> unfulfilledHooks;
		public Operation()
		{
			unfulfilledHooks = new Queue<UnfulfilledHookResolver>();
		}
		
		public abstract Operation Coalesce();
		///<summary>
		///Performs an initial fulfill by linking
		///nodes together that can be linked together
		///within the confines of the current operation
		///Unfulfillable hooks need to be registered through
		///the RegisterHook method so they can be linked to the
		///next node within the parent operation of this operation
		///</summary>
		public abstract void InitialFulfill();
		public void RegisterHook(UnfulfilledHookResolver resolver)
		{
			unfulfilledHooks.Enqueue(resolver);
		}
		///<summary>
		///Returns the head node of the current operation chain
		///</summary>
		public abstract Operation First();
		public Operation OnTrueHook { get; set; }
		public Operation OnFalseHook { get; set; }
		public void ResolveHooks(Operation next)
		{
			if(unfulfilledHooks.Count > 0)
			{
				do
				{
				  var hook = unfulfilledHooks.Dequeue();
					hook(next);
				}while(unfulfilledHooks.Count > 0);
			}
		}
		public abstract void Build(IGraphBuilder builder);
		public virtual void Enumerate(IGraphBuilder builder)
		{
			Index = builder.Index(this);
		}
	}
}
