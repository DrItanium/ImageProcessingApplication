using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Libraries.Messaging
{
  public abstract class TaggedObject : MarshalByRefObject 
  {
    private Guid id;
    public Guid ObjectID { get { return id; } protected set { id = value; } }
    protected TaggedObject(Guid id)
    {
      this.id = id;
    }
    public override bool Equals(object other)
    {
      TaggedObject obj = (TaggedObject)other;
      Guid oth = obj.id;
      return oth.Equals(id);
    }
    public override int GetHashCode()
    {
      return id.GetHashCode();
    }
    public override string ToString()
    {
      return id.ToString();
    }
  }
}
