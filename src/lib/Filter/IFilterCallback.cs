using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libraries.Filter
{
    public interface IFilterCallback
    {
        Guid CurrentFilter { set; }
        string Name { set; }
    }
}
