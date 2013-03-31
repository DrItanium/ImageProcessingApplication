using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.FileFormat;
using System.Drawing;

namespace Formats.Divide 
{
	///<summary>
	///Base class for all square tile image division format converters
	///</summary>
	public abstract class SquareDividerConverter : ImageDividerConverter 
	{
		public abstract int SideLength { get; }
		public sealed override int DivideWidth { get { return SideLength; } }
		public sealed override int DivideHeight { get { return SideLength; } }
		protected SquareDividerConverter(string name) : base(name) { }
	}

}
