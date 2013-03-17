using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Extensions;
using Libraries.Collections;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
	public class ModifiableStringLiteral : StringLiteral
	{
		public const string MODIFIABLE_IDENTIFIER = "\"([^\"]|((?<=\\\\)((?<!\\\\\\\\)\"{1})))*\"";
		public ModifiableStringLiteral(string expression, string name, string type, string appendToFront)
			: base(string.Format("{0}{1}",appendToFront, expression), name, type)
			{

			}
		public ModifiableStringLiteral(string expression, string name, string type)
			: this(expression, name, type, string.Empty)
		{
		}
		public ModifiableStringLiteral(string expression, string name)
			: this(expression, name, "string-literal")
		{
		}
		public ModifiableStringLiteral(string expression)
			: this(expression, "String Literal")
		{
		}
		public ModifiableStringLiteral()
			: this(MODIFIABLE_IDENTIFIER)
		{
		}
	}
}
