using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Libraries.Messaging
{
  public abstract class Initiator : TaggedObject 
  {
    protected TaggedObject(Guid id) : base(id) { }
    protected TaggedObject() : base(Guid.NewGuid()) { }
    
    public abstract Message Invoke(Message input);
  }
}
