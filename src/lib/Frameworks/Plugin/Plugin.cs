using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using Libraries.Messaging;

namespace Frameworks.Plugin
{
  public abstract class Plugin : TaggedObject 
  {
    private string name;
    public string Name { get { return name; } protected set { name = value; } }
    protected Plugin(string name) 
      : base(Guid.NewGuid())
    { 
      this.name = name;
    }
    public abstract Message Invoke(Message input);
  }
}
