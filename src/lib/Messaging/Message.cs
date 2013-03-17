using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Libraries.Messaging
{
  public class Message : TaggedObject 
  {
    private Guid receiver, sender;
    public Guid Sender { get { return sender; } protected set { sender = value; } }
    public Guid Receiver { get { return receiver; } protected set { receiver = value; } }
    private MessageOperationType operationType;
    public MessageOperationType OperationType { get { return operationType; } protected set { operationType = value; } }
    private object value;
    public object Value { get { return value; } protected set { this.value = value; } }

    public Message(Guid id, Guid sender, Guid receiver, MessageOperationType type, object value)
      : base(id)
    {
      this.sender = sender;
      this.receiver = receiver;
      this.operationType = type;
      this.value = value;
    }
    public Message(Guid sender, Guid receiver, MessageOperationType type, object value)
      : this(Guid.NewGuid(), sender, receiver, type, value)
    {

    }
  }
  public enum MessageOperationType
  {
    ///<summary>
    /// Tells the receiever that they are passing data. This is 
    /// the default action.
    ///</summary>
    Pass,
    ///<summary>
    /// Tells the receiever to execute the data they are
    /// being sent. This is usually a function. Most of the
    /// time an execute message is sent before a pass message to 
    /// tell another domain to get ready to execute something.
    ///</summary>
    Execute,
    ///<summary>
    /// Tells the reciever that this the result of an
    /// execution that occured remotely
    ///</summary>
    Return,
    ///<summary>
    /// Tells the reciever that a message runtime action is occuring.
    /// This is how this library can extend functionality without needing
    /// a recompile.
    ///</summary>
    System,
    ///<summary>
    /// Used by the message back end to ensure that a link can be
    /// established and a pipe can be created.
    ///</summary>
    Init,
    ///<summary>
    /// Message used to describe a failure to another node.
    /// This is an internal type used by the library
    ///</summary>
    Failure, 
  }
}
