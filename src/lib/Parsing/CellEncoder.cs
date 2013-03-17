using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Libraries.Parsing
{
	//	public static class Program
	//	{
	//		public static void Main(string[] args)
	//		{
	//			CellEncoder ce = new CellEncoder();
	//			uint[] elements = new uint[] { 255, 16384, 6561 };
	//			foreach(var v in elements)
	//				Console.WriteLine(v);
	//			ulong value = ce.Encode(elements);
	//			Console.WriteLine("Encoded Value is: {0}", value);
	//			var decoded = ce.Decode(value);
	//			foreach(var v in decoded)
	//				Console.WriteLine(v);
	//		}
	//	}
	public class CellEncoder : IEncoder<uint, ulong>
	{
		public ulong Encode(IEnumerable<uint> decoding)
		{
			ulong value = 0L;
			ulong v0 = (ulong)decoding.First();
			ulong v1 = (ulong)decoding.ElementAt(1);
			ulong v2 = (ulong)decoding.ElementAt(2);
			value = (v0 << 56);
			value = value + v2;
			value = value + (v1 << 28);
			return value;
		}
		public IEnumerable<uint> Decode(ulong encoding)
		{
			yield return (uint)(encoding >> 56);
			yield return (uint)((encoding << 8) >> 36);
			yield return (uint)((encoding << 36) >> 36);
		}
	}
}
