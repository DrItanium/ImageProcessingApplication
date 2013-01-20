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
	public interface IGraphLink 
	{
		int From { get; }
		int To { get; }
		string Label { get; }
		string ToString();
	}
	public abstract class GraphBuilder<T> : List<T>, IGraphBuilder
		where T : IGraphLink
	{
		public const string DEFAULT_GRAPH_NAME = "whatever";
		private List<Operation> operations; 
		public GraphBuilder()
		{
			operations = new List<Operation>();
		}
		public int Index(Operation op)
		{
			int index = operations.Count;
			operations.Add(op);
			return index; 
		}
		public abstract void Link(int i0, int i1, string label);
		public void Link(int i0, int i1)
		{
			Link(i0,i1,string.Empty);
		}
		public abstract string ToString(string graphName);
		public override string ToString()
		{
			return ToString(DEFAULT_GRAPH_NAME);
		}
		public void PrintRegisteredOperations()
		{
			for(int i = 0; i < operations.Count; i++)
				Console.WriteLine("/*{0}: {1}*/",i, operations[i]);
		}
	}
}
