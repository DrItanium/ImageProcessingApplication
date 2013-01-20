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
  public class PluginLoader : MarshalByRefObject 
  {
    private Dictionary<Guid, Plugin> dict;
    private Guid objectID;
    private string name, author;
    public string Name { get { return name; } protected set { name = value; } }
    public string Author { get { return author; } protected set { author = value; } }
    public Guid ObjectID { get { return objectID; } }
    public PluginLoader(string assembly)
    {
      objectID = Guid.NewGuid();
      dict = new Dictionary<Guid, Plugin>();
      Assembly asm = Assembly.LoadFile(assembly);
      if(asm.IsDefined(typeof(PluginAssemblyAttribute), false))
      {
        PluginAssemblyAttribute paa = (PluginAssemblyAttribute)asm.GetCustomAttributes(typeof(PluginAssemblyAttribute), false)[0];
        Name = paa.Name;
        if(paa.Author != null && !paa.Author.Equals(string.Empty))
          Author = paa.Author;
        else
          Author = string.Empty;
        var query = from x in asm.GetTypes()
          where x.IsDefined(typeof(PluginAttribute), false)
          select new 
          {
            Header = x.GetCustomAttributes(typeof(PluginAttribute), false)[0] as PluginAttribute,
                   Type = x,
          };
        foreach(var v in query)
        {
          Plugin p = (Plugin)Activator.CreateInstance(v.Type, new object[] { v.Header.Name });
          dict.Add(p.ObjectID, p);
        }
      }
    }
    public int Count { get { return dict.Count; } }
    public IEnumerable<Guid> Names { get { return dict.Keys; } }
    public Plugin this[Guid name] { get { return dict[name]; } }
    public Message Invoke(Message input) { return this[input.Receiver].Invoke(input);  }
  }
}
