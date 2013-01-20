
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
	public class IfThenStatement : NonStandardOperation
	{
		public override Operation First()
		{
			return Condition.First();
		}
		public BooleanOperation Condition
		{
			get
			{
				return (BooleanOperation)this[0];
			}
			set
			{
				this[0] = value;
			}
		}
		public Block OnTrue 
		{
			get 
			{
				return (Block)this[1];
			}
			set
			{
				this[1] = value;
			}
		}
			
		public IfThenStatement(BooleanOperation condition, Block onTrue)
		{
			Add(condition);
			Add(onTrue);
		}
		public override Operation Coalesce()
		{
			Condition = (BooleanOperation)Condition.Coalesce();
			OnTrue = (Block)OnTrue.Coalesce();
			return this;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("\t{0}\n",GetType().ToString());
			sb.AppendFormat("\t\t{0}\n",Condition.ToString());
			sb.AppendFormat("\t\t{0}\n",OnTrue.ToString());
			return sb.ToString();
		}
		public override void InitialFulfill()
		{
			Condition.OnTrueHook = OnTrue.First();
			Condition.InitialFulfill();
			RegisterHook((x) => Condition.ResolveHooks(x));
			OnTrue.InitialFulfill();
		  RegisterHook((x) => OnTrue.ResolveHooks(x));	
		}
		public override void Enumerate(IGraphBuilder builder)
		{
			base.Enumerate(builder);
			foreach(var v in this)
				v.Enumerate(builder);
		}
		public override void Build(IGraphBuilder builder)
		{
			Condition.Build(builder);
			OnTrue.Build(builder);
		}
	}
	public class IfThenElseStatement : IfThenStatement
	{
		public Block OnFalse
		{
			get
			{
				return (Block)this[2];
			}
			set
			{
				this[2] = value; 
			}
		}
		public IfThenElseStatement(BooleanOperation condition, 
				Block onTrue, Block onFalse)
			: base(condition, onTrue)
		{
			Add(onFalse);
		}
		public override Operation Coalesce()
		{
			Condition = (BooleanOperation)Condition.Coalesce();
			OnFalse = (Block)OnFalse.Coalesce();
			OnTrue = (Block)OnTrue.Coalesce();
			return this;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("\t{0}\n",GetType().ToString());
			sb.AppendFormat("\t\t{0}\n",Condition.ToString());
			sb.AppendFormat("\t\t{0}\n",OnTrue.ToString());
			sb.AppendFormat("\t\t{0}",OnFalse.ToString());
			return sb.ToString();
		}
		public override void InitialFulfill()
		{
			Condition.OnTrueHook = OnTrue.First();
			Condition.InitialFulfill();
			Condition.ResolveHooks(OnFalse.First());
			OnTrue.InitialFulfill();
			RegisterHook((x) => OnTrue.ResolveHooks(x));
			OnFalse.InitialFulfill();
			RegisterHook((x) => OnFalse.ResolveHooks(x));
		}
		public override void Build(IGraphBuilder builder)
		{
			//we already gave the proper links so just act as a forwarder
			Condition.Build(builder);
			OnTrue.Build(builder);
			OnFalse.Build(builder);
			//OnTrue and OnFalse
		}
	}
}
