using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Libraries.FileFormat {
  public interface IFileFormatCallback {
    public Guid UniqueID { get; }
    public string FilterName { get; }
    public string FilterCode { get; }
  }
}


