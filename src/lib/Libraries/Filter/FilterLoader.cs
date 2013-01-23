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
  public class Initiator : MarshalByRefObject, IPluginLoader 
  {
    private Tuple<string, string, Guid>[] boundaryTuples;
    public Tuple<string, string, Guid>[] DesiredPluginInformation { get { return boundaryTuples; } }
    private Dictionary<Guid, Guid> translationLayer;
    public Initiator(string[] paths)
    {
        translationLayer = new Dictionary<Guid, Guid>(); 
        List<Tuple<string, string, Guid>> r = new List<Tuple<string, string, Guid>>();
        foreach(string str in paths)
        {
          var guids = FilterLoader.LoadPlugins(str);
          foreach(var guid in guids.Item2)
          {
            translationLayer[guid] = guids.Item1;
            r.Add(FilterLoader.GetPlugin(guids.Item1, guid));
          }
        }
        boundaryTuples = r.ToArray();
    }
    public Message Invoke(Message input)
    {
      return FilterLoader.Invoke(translationLayer[input.Receiver], input);
    }
  }
  public static class FilterLoader
  {
    private static Dictionary<Guid, PluginLoader> pluginEnvironments;
    static FilterLoader()
    {
      pluginEnvironments = new Dictionary<Guid, PluginLoader>();
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
      PluginLoader pl = new PluginLoader(path);
      pluginEnvironments.Add(pl.ObjectID, pl); 
      return new Tuple<Guid,Guid[]>(pl.ObjectID, pl.Names.ToArray());
    }
  }
}
