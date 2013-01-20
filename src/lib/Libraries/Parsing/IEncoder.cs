using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//using Libraries.Collections;

namespace Libraries.Parsing
{
	public interface IEncoder<I,O>
	{
		O Encode(IEnumerable<I> decoding);
		IEnumerable<I> Decode(O encoding);
	}
}
