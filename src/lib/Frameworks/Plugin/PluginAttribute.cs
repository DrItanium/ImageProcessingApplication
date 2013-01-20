using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Collections;

namespace Frameworks.Plugin
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class PluginAttribute : Attribute
  {
     public string Name { get; private set; }
     public string Version { get; set; }

     public PluginAttribute(string name)
     {
       Name = name;
     }
  }
}
