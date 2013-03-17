using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Drawing;
using Libraries.Messaging;
using Frameworks.Plugin;

namespace Libraries.FileFormat 
{
  public abstract class FileFormatConverter : Plugin 
  {
    public abstract string FilterString { get; }
    public abstract string FormCode { get; }
    protected FileFormatConverter(string name) : base(name) { }
    public abstract Color[][] Process(Hashtable input);
    public override Message Invoke(Message input) 
    {
      return new Message(Guid.NewGuid(), 
          ObjectID, 
          input.Sender, 
          MessageOperationType.Return,
          Process((Hashtable)input.Value));
    }
  }
}
