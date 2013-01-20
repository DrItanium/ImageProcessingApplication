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
	public abstract class Filter : Plugin, IFilter
	{
		public static Color[] GreyScaleColors = new Color[256];
		static Filter()
		{
			for(int i = 0; i < 256; i++)
				GreyScaleColors[i] = Color.FromArgb(255, i, i, i);
		}
    public abstract string InputForm { get; } 
		protected Filter(string name) : base(name) { }
		public abstract byte[][] Transform(Hashtable source);
    public override Message Invoke(Message input)
    {
      var result = Transform(TranslateData((Hashtable)input.Value));
      return new Message(Guid.NewGuid(), ObjectID, input.Sender, MessageOperationType.Return,
          result);
    }
    public abstract Hashtable TranslateData(Hashtable input);
	}
	public delegate byte[][] ImageTransformOperation(byte[][] input);
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class FilterAttribute : PluginAttribute
	{
		public FilterAttribute(string name) 
      : base(name)
    {
		}
	}
}
