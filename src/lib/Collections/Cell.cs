using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace Libraries.Collections
{
	public interface ICell<T> : IEnumerable<T>, ICloneable
	{
		T this[int index] { get; }
		int Length { get; }
		int Count { get; }
		bool IsFull { get; }
		void Expand(int newSize);
		bool Exists(Predicate<T> predicate);
		int IndexOf(T value);
		bool Add(T value);
		bool Remove(T value);
		bool Contains(T value);
		T[] ToArray();
		void Clear();
	}
	public class Cell<T> : ICell<T>
	{
		public const int DEFAULT_CAPACITY = 7;
		protected T[] backingStore;
		private int count; 
		public T this[int index] { get { return backingStore[index]; } set { backingStore[index] = value; } }
		public int Count { get { return count; } protected set { count = value; } }
		public int Length { get { return backingStore.Length; } }
		public bool IsFull { get { return backingStore.Length == Count; } }
		public Cell(IEnumerable<T> elements)
			: this(elements.Count())
		{
			foreach(T e in elements)
				Add(e);	
		}
		public Cell(int capacity)
		{
			backingStore = new T[capacity];
			count = 0;
		}
		public Cell() : this(DEFAULT_CAPACITY) { }
		public void Expand(int newSize)
		{
			if(newSize == Length)
				return;
			else if(newSize < Length)
				throw new ArgumentException("Given expansion size is less than the current size");
			else
			{
				T[] newBackingStore = new T[newSize];
				for(int i = 0; i < Count; i++)
				{
					newBackingStore[i] = backingStore[i];
					backingStore[i] = default(T);
				}
				backingStore = newBackingStore;
			}
		}
		public bool Exists(Predicate<T> predicate)
		{
			for(int i = 0; i < Count; i++)
				if(predicate(backingStore[i]))
					return true;
			return false;
		}
		public int IndexOf(T value)
		{
			int index = 0;
			Predicate<T> pred = (x) => 
			{
				bool result = x.Equals(value);
				if(!result)
					index++;
				return result;
			};
			if(Exists(pred))
				return index;
			else
				return -1;
		}
		public bool Contains(T value)
		{
			return IndexOf(value) != -1;
		}
		public bool Add(T value)
		{
			bool result = !IsFull;
			if(result)
			{
				backingStore[count] = value;
				count++;
			}
			return result;
		}
		///<summary>
		///Used to remove all intermediate empty cells and put them at the back
		///This code assumes that there is at least one free cell, otherwise it will
		///do nothing. It also assumes that the starting position is empty and that we are removing
		///</summary>
		protected void CompressCell(int startingAt)
		{
			if((startingAt < (Length - 1)))
			{	
				for(int i = startingAt; (i + 1) < Length; i++)
					backingStore[i] = backingStore[i + 1];
			}
		}
		public bool Remove(T value)
		{
			int index = IndexOf(value);
			bool result = (index != -1);
			if(result)
			{
				backingStore[index] = default(T);
				if(index != (Count - 1))
					CompressCell(index);
				count--;
			}
			return result;
		}
		public bool RemoveFirst()
		{
			bool result = Count == 0;
			if(!result)
			{
				backingStore[0] = default(T);
				if(Count > 1)
					CompressCell(0);
				count--;
			}
			return result;
		}
		public bool RemoveLast()
		{
			bool result = Count == 0;
			if(!result)
			{
				backingStore[Count - 1] = default(T);
				count--;
			}
			return result;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public IEnumerator<T> GetEnumerator()
		{
			return new CellEnumerator(this);
		}
		public void SecureClear()
		{
			//linear operation to clear it
			for(int i = 0; i < Count; i++)
				this[i] = default(T);
			count = 0;
		}
		public void Clear()
		{
			count = 0;
			//we already have the block, lets not
			//waste it
		}
		public T[] ToArray()
		{
			T[] newElements = new T[backingStore.Length];
			for(int i = 0; i < Count; i++)
				newElements[i] = backingStore[i];
			return newElements;
		}
		public class CellEnumerator : IEnumerator<T>
		{
			private T[] backingStore;
			private int index, count;
			public T Current { get { return backingStore[index]; } }
			object IEnumerator.Current { get { return backingStore[index]; } }
			public CellEnumerator(Cell<T> cell)
			{
				this.backingStore = cell.backingStore;
				this.count = cell.count;
				this.index = -1;
			}
			public bool MoveNext()
			{
				index++;
				return index < count;	
			}
			public void Reset()
			{
				index = -1;
			}
			public void Dispose()
			{
				backingStore = null;
			}
		}
		public virtual object Clone()
		{
			return new Cell<T>(this);
		}
	}
}
