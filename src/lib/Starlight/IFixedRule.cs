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
	public interface IRule : ICloneable
	{
		string Name { get; }
		IProduction this[int index] { get; }
		int Count { get; }
		IEnumerable<IProduction> GetEnumerableInterface();
	}
	///<summary>
	/// Represents a rule that is fixed on a target production.
	/// This is not the same as an advancible rule because the actual
	/// production is not be advanced itself.
	///</summary>
	public interface IFixedRule : IRule
	{
		int Offset { get; }
		IProduction Current { get; }
		new IProduction this[int index] { get; }
		int GetHashCode();
		bool Equals(object other);
	}
	public interface IAdvanceableRule : IRule
	{
		new IAdvanceableProduction this[int index] { get; }
		bool Equals(object other);
		int GetHashCode();
		new IEnumerable<IAdvanceableProduction> GetEnumerableInterface();
	}
}
