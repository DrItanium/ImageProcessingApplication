using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Drawing;
using Libraries.Messaging;
using Frameworks.Plugin;

namespace Libraries.FileFormat 
{
  public abstract class FileFormatConverter : Plugin 
  {
    public abstract bool SupportsSaving { get; }
    public abstract bool SupportsLoading { get; }
    public abstract string FilterString { get; }
    public abstract string FormCode { get; }
    protected FileFormatConverter(string name) : base(name) { }
    public virtual object Process(Hashtable input) 
    {
      string action = (string)input["action"];
      switch(action.ToLower()) 
      {
        case "load":
          return Load(input);
        case "save":
          Save(input);
          return null;
        case "supports-loading":
          return SupportsLoading;
        case "supports-saving":
          return SupportsSaving;
        case "get-filter-string":
          return FilterString;
        case "get-form-code":
          return FormCode;
        default:
          throw new ArgumentException(
              string.Format("ERROR: Provided action {0} isn't valid", action));
      }
    }
    public abstract int[][] Load(Hashtable input);
    public abstract void Save(Hashtable input);
    public override Message Invoke(Message input) 
    {
      return new Message(Guid.NewGuid(), 
          ObjectID, 
          input.Sender, 
          MessageOperationType.Return,
          Process((Hashtable)input.Value));
    }
  }
}
