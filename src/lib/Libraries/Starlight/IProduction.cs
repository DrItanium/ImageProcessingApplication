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
	public interface IProduction : IEnumerable<string>, ICloneable
	{
		SemanticRule Rule { get; }
		string this[int index] { get; }
		int Count { get; }

	}
	public interface IFixedProduction : IProduction
	{
		int Max { get; }
		int Min { get; }
		int Position { get; }
		string Current { get; }
		bool HasNext { get; }
		bool HasPrevious { get; }

	}
	public interface IAdvanceableProduction : IFixedProduction
	{
		IAdvanceableProduction FunctionalNext();
		IAdvanceableProduction FunctionalPrevious();
		bool Next();
		bool Previous();
		void Reset();
	}
}
