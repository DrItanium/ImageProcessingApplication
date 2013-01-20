using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Libraries.Messaging
{
  ///<summary>
  ///Represents a container over a set of actions to perform
  ///with respect to messages. This dispatches actions based 
  ///on messages received. Recieving a ton of messages will
  ///cause it to queue them. Processing them one after another.
  ///</summary>
  public abstract class Node : TaggedObject
  {
    protected Node(Guid id) : base(id) { }
    public virtual void Receive(Message incoming)
    {
      switch(incoming.OperationType)
      {
        case MessageOperationType.Pass:
          OnPass(incoming);
          break;
        case MessageOperationType.Execute:
          OnExecute(incoming);
          break;
        case MessageOperationType.Return:
          OnReturn(incoming);
          break;
        case MessageOperationType.System:
          OnSystemCall(incoming);
          break;
        case MessageOperationType.Init:
          OnInit(incoming);
          break;
        case MessageOperationType.Failure:
          throw new ArgumentException(string.Format("Error: {0}", incoming.Value));
          break;
        default:
          DispatchTo(incoming.Sender, ObjectID, MessageOperationType.Failure, "Invalid Action Given");
          break;
      }
    }
    public virtual void OnPass(Message incoming)
    {
      //do nothing
    }
    public virtual void OnExecute(Message incoming)
    {
      //do nothing
    }
    public virtual void OnReturn(Message incoming)
    {
      //do nothing
    }
    public virtual void OnSystemCall(Message incoming)
    {
      //do nothing
    }
    public virtual void OnInit(Message incoming)
    {
      //do nothing
    }
    public virtual void DispatchTo(Guid target, Guid from, MessageOperationType type, object input)
    {
      DispatchTo(new Message(Guid.NewGuid(), from, target, type, input)); 
    }
    public abstract void DispatchTo(Message mess);
  }
}
