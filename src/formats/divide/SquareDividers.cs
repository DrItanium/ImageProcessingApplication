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
	[FileFormat("Save as 8x8 tiles (*.*)")]
	public class Divider8x8 : SquareDividerConverter 
	{
		public override int SideLength { get { return 8; } }
		public Divider8x8(string name) : base(name) { }
	}
	[FileFormat("Save as 16x16 tiles (*.*)")]
	public class Divider16x16 : SquareDividerConverter 
	{
		public override int SideLength { get { return 16; } }
		public Divider16x16(string name) : base(name) { }
	}
	[FileFormat("Save as 32x32 tiles (*.*)")]
	public class Divider32x32 : SquareDividerConverter 
	{
		public override int SideLength { get { return 32; } }
		public Divider32x32(string name) : base(name) { }
	}
	[FileFormat("Save as 64x64 tiles (*.*)")]
	public class Divider64x64 : SquareDividerConverter 
	{
		public override int SideLength { get { return 64; } }
		public Divider64x64(string name) : base(name) { }
	}
	[FileFormat("Save as 128x128 tiles (*.*)")]
	public class Divider128x128 : SquareDividerConverter 
	{
		public override int SideLength { get { return 128; } }
		public Divider128x128(string name) : base(name) { }
	}
}
