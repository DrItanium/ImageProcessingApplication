using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Collections;

namespace Frameworks.Plugin
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
  public sealed class PluginAssemblyAttribute : Attribute
  {
     public string Name { get; private set; }
     public string Author { get; set; }
     public string Version { get; set; }

     public PluginAssemblyAttribute(string name)
     {
       Name = name;
     }
  }
}
