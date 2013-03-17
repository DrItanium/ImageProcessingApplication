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
	public interface IGraphBuilder
	{
		int Index(Operation op);
		void Link(int from, int to, string label);
		void Link(int from, int to);
		string ToString(string graphName);
	}
}
