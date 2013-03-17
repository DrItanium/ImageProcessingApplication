using System;
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
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class FileFormatAttribute : PluginAttribute
  {
    public FileFormatAttribute(string name) : base(name) { }
  }

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
  public sealed class FileFormatAssemblyAttribute : PluginAssemblyAttribute 
  {
    public FileFormatAssemblyAttribute(string name) : base(name) { }
  }

}
