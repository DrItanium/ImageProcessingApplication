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
	public class GraphVizLink : IGraphLink
	{
		private int from, to;
		private string label;
		public int From { get { return from; } }
		public int To { get { return to; } }
		public string Label { get { return label; } }
		public GraphVizLink(int from, int to, string label)
		{
			this.from = from;
			this.to = to;
			this.label = label;
		}
					
	 public override string ToString()
	 {
		 if(label.Equals(string.Empty))
			 return string.Format("node{0} -> node{1};", From, To);
		 else
		   return string.Format("node{0} -> node{1} {2};", From, To, Label);
	 }	 
	}
	public class GraphVizGraphBuilder : GraphBuilder<GraphVizLink>
	{
		public GraphVizGraphBuilder() : base() { }
		public override void Link(int i0, int i1, string label)
		{
			Add(new GraphVizLink(i0,i1,label));
		}
		public override string ToString(string graphName)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("digraph {0}\n",graphName);
			sb.AppendLine("{");
			foreach(var v in this)
				sb.AppendLine(v.ToString());
			sb.AppendLine("}");
			return sb.ToString();
		}
	}
}
