using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using Frameworks.Plugin;
using Libraries.Messaging;

namespace Libraries.Filter
{
	public abstract class ImageFilter : Plugin
	{
		public abstract string InputForm { get; }
		protected ImageFilter(string name) : base(name) { }
		public abstract Hashtable TranslateData(Hashtable input);
		public abstract int[][] TransformImage(Hashtable source);
		public override Message Invoke(Message input)
		{
			var result = TransformImage(TranslateData((Hashtable)input.Value));
			return new Message(Guid.NewGuid(), ObjectID, input.Sender, MessageOperationType.Return,
					result);
		}
	}
}
