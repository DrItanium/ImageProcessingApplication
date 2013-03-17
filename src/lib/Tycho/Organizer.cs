using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Collections;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
	public class StringOrganizer : List<Keyword>
	{
		private List<ShakeCondition<string>> shakedConditions; 
		private List<TypedShakeCondition<string>> typedShakedConditions;
		public StringOrganizer(IEnumerable<Keyword> input)
		{
			shakedConditions = new List<ShakeCondition<string>>();
			typedShakedConditions = new List<TypedShakeCondition<string>>();
			//select all elements that are contained within the
			//current element
			//What this does is make functions that are used as 
			//shake selectors
			//To this end its important to find those segments that are 
			Dictionary<string, Keyword> keys = new Dictionary<string, Keyword>();
			foreach(var v in input)
				keys.Add(v.TargetWord, v);
			var selection = from x in input
				let y = (from z in input
						where !z.TargetWord.Equals(x.TargetWord) && z.TargetWord.Contains(x.TargetWord)
						select z.TargetWord)
				group y by x.TargetWord into element
				select element;

			Dictionary<string,int> frequencyTable = new Dictionary<string,int>();
			foreach(var v in input)
				frequencyTable.Add(v.TargetWord,0);
			foreach(var v in selection)
			{
				foreach(var q in v)
				{
					frequencyTable[v.Key] += q.Count();
				}
			}
			var result = from zz in frequencyTable
				orderby zz.Value ascending, zz.Key.Length descending
				select new
				{
					Key = zz.Key,
							Frequency = zz.Value,
							NeedsEqualityCheck = (zz.Value > 0 && zz.Key.Length > 1)
				};

			foreach(var v in result)
			{
				if (v.NeedsEqualityCheck)
				{
					//Console.WriteLine("v.Key = {0}", v.Key);
					keys[v.Key].RequiresEqualityCheck = v.NeedsEqualityCheck;
				}
				Add(keys[v.Key]);
			}
			// Console.WriteLine("------");
			//foreach (var v in this)
			//   Console.WriteLine(v.TargetWord);
			//Console.WriteLine("------");
			foreach(var v in this)
				shakedConditions.Add(v.AsShakeCondition());
			foreach(var v in this)
				typedShakedConditions.Add(v.AsTypedShakeCondition());
		}
		public IEnumerable<ShakeCondition<string>> GetShakeConditions()
		{
			return shakedConditions;
		}
		public IEnumerable<TypedShakeCondition<string>> GetTypedShakeConditions()
		{
			return typedShakedConditions;
		}
	}
}
