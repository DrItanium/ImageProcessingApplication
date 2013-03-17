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
  public class FilterInitiator : PluginInitiator<Tuple<string,string,Guid>>
  {
    private Tuple<string, string, Guid>[] boundaryTuples;
    public override Tuple<string, string, Guid>[] DesiredPluginInformation { get { return boundaryTuples; } }
    public FilterInitiator(string[] paths) : base()
    {
        List<Tuple<string, string, Guid>> r = new List<Tuple<string, string, Guid>>();
        foreach(string str in paths)
        {
          var guids = FilterLoaderBackingStore.LoadPlugins(str);
          foreach(var guid in guids.Item2)
          {
            this[guid] = guids.Item1;
            r.Add(FilterLoaderBackingStore.GetPlugin(guids.Item1, guid));
          }
        }
        boundaryTuples = r.ToArray();
    }
    public override Message Invoke(Message input)
    {
      return FilterLoaderBackingStore.Invoke(this[input.Receiver], input);
    }
  }
  public static class FilterLoaderBackingStore
  {
    private static Dictionary<Guid, FilterLoader> pluginEnvironments;
    static FilterLoaderBackingStore()
    {
      pluginEnvironments = new Dictionary<Guid, FilterLoader>();
    }
    public static Message Invoke(Guid targetPluginGroup, Message input)
    {
      return pluginEnvironments[targetPluginGroup].Invoke(input);
    }
    public static Tuple<string, string, Guid> GetPlugin(Guid targetGuid, Guid pluginGuid)
    {
      Filter target = (Filter)pluginEnvironments[targetGuid][pluginGuid];
      return new Tuple<string, string, Guid>(target.Name, target.InputForm, target.ObjectID);
    }
    public static Tuple<Guid, Guid[]> LoadPlugins(string path)
    {
      FilterLoader pl = new FilterLoader(path);
      pluginEnvironments.Add(pl.ObjectID, pl); 
      return new Tuple<Guid,Guid[]>(pl.ObjectID, pl.Names.ToArray());
    }
  }
  public sealed class FilterAssemblyAttribute : PluginAssemblyAttribute 
  {
    public FilterAssemblyAttribute(string name) : base(name) 
    {

    }
  }
  public sealed class FilterLoader : PluginLoader<FilterAttribute, FilterAssemblyAttribute> 
  {
    public FilterLoader(string path) : base(path) { } 
  }
}
