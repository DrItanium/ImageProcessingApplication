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
  public abstract class PluginInitiator<T> : Initiator, IPluginLoader<T>
  {
    private Dictionary<Guid, Guid> translationLayer;
    private T[] boundaryTuples;
    public abstract T[] DesiredPluginInformation { get; }
    protected PluginInitiator() 
    {
      translationLayer = new Dictionary<Guid, Guid>();
    }
    protected Guid this[Guid from] 
    {
      get 
      {
        return translationLayer[from]; 
      }
      set
      {
        translationLayer[from] = value;
      }
    }
     
  }
}
