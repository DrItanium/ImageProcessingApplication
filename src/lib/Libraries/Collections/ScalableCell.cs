using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace Libraries.Collections
{
	public class ScalableCell<R, T> : MultiCell<R, T> 
		where R : Cell<T>
	{
		public ScalableCell(Func<int, R> ctor, int size, int defaultCellSize) : base(ctor, size, defaultCellSize) { }
		public ScalableCell(Func<int, R> ctor, int size) : this(ctor, size, DEFAULT_CAPACITY) { }
		public ScalableCell(Func<int, R> ctor) : this(ctor, DEFAULT_CAPACITY) { }
		public ScalableCell(ScalableCell<R, T> cell) : base(cell) { }

		public new bool Add(T value)
		{
			bool result = base.Add(value);
			if(!result && (CurrentCell >= CellCount) && (CellCount < CellLength))
			{
				R newCell = ctor(DefaultCellSize);		
				Add(newCell);
				return Add(value);
			}
			return result;
		}
		public new bool AddRange(IEnumerable<T> values)
		{
			foreach(T v in values)
			{
				if(!Add(v))
					return false;	
			}
			return true;
		}
		public new void PerformFullCompression()
		{
			lock(compressionLock)
			{
				T[] newElements = ToArray();
				Clear();
				AddRange(newElements);
			}
		}
		public override object Clone()
		{
			return new ScalableCell<R, T>(this);
		}
	}
	public class ScalableCell<T> : ScalableCell<Cell<T>, T>
	{
		public ScalableCell(int numCells, int defaultCellSize)
			: base((x) => new Cell<T>(x), numCells, defaultCellSize)
		{

		}
		public ScalableCell(int numCells) : this(numCells, DEFAULT_CAPACITY) { }
		public ScalableCell() : this(DEFAULT_CAPACITY) { }

		public ScalableCell(ScalableCell<T> other) : base(other) { }

		public override object Clone() 
		{
			return new ScalableCell<T>(this); 
		}
	}
}
