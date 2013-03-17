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
  public class FileFormatInitiator : PluginInitiator<Tuple<string,string,string,Guid>>
  {
    private Tuple<string,string,string,Guid>[] backingStore;
    public override Tuple<string,string,string,Guid>[] DesiredPluginInformation 
    {
      get 
      { 
        return backingStore; 
      } 
    }
    public FileFormatInitiator(string[] paths) 
    {
      List<Tuple<string,string,string,Guid>> l = new List<Tuple<string,string,string,Guid>>();
      foreach(string str in paths) 
      {
        var guids = FileFormatLoaderBackingStore.LoadPlugins(str);
        foreach(var guid in guids.Item2) 
        {
          this[guid] = guids.Item1;
          l.Add(FileFormatLoaderBackingStore.GetPlugin(guids.Item1, guid));
        }
      }
      backingStore = l.ToArray();
    }

    public override Message Invoke(Message input) 
    {
      return FileFormatLoaderBackingStore.Invoke(this[input.Receiver], input);
    }
  }
}
