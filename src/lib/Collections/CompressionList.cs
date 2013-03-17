using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace Libraries.Collections
{
	public class CompressionList<T> 
	{
		private T defaultValue;
		private Dictionary<int,int> translationTable;
		private List<T> storage;
		public int Count { get { return storage.Count; } }
		public int Capacity { get { return storage.Capacity; } }
		public CompressionList(T defaultValue)
		{
			this.defaultValue = defaultValue;
		}
		public T this[int index]
		{
			get
			{
				if(translationTable == null || !translationTable.ContainsKey(index))
					return defaultValue;
				else
					return storage[translationTable[index]];
			}
			set
			{
				if(translationTable == null)
				{
					translationTable = new Dictionary<int,int>();
					storage = new List<T>();
				}
				if(!translationTable.ContainsKey(index))
				{
					int target = storage.Count;
					storage.Add(value);
					translationTable[index] = target;
				}
				else
					storage[translationTable[index]] = value;
			}
		}
		public bool Contains(T value)
		{
			return storage.Contains(value);
		}
		public bool Exists(Predicate<T> value)
		{
			return storage.Exists(value);
		}
	}
}
