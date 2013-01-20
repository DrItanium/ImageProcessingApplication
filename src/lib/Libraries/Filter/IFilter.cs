using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace Libraries.Filter
{
	public interface IFilter
	{
		byte[][] Transform(Hashtable input);
		string Name { get; }
	}
}
