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
  public class FileFormatLoader : PluginLoader<FileFormatAttribute, FileFormatAssemblyAttribute>
  {
    public FileFormatLoader(string path) : base(path) { }
  }
}
