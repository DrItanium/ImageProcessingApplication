using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.Starlight
{
	public interface ILookaheadRule : IAdvanceableRule
	{
		string LookaheadSymbol { get; }
	}
}
