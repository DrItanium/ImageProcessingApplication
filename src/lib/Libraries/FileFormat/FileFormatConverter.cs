using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Messaging;
using Frameworks.Plugin;

namespace Libraries.FileFormat 
{
  public abstract class FileFormatConverter : Plugin 
  {
    public abstract string FormCode { get; }
    protected FileFormatConverter(string name, string filter) : base(string.Format("{0}|{1}", name,filter)) { }
    public abstract byte[][][] Load(Hashtable input);
    public override Message Invoke(Message input) 
    {
      return new Message(Guid.NewGuid(), ObjectID, input.Sender, MessageOperationType.Return,
          Load((Hashtable)input.Value));
    }
  }
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class FileFormatAttribute : PluginAttribute
  {
    public FileFormatAttribute(string name) : base(name) { }
  }

}
