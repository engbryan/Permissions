using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restier
{
    public enum GrantAccessType
    {
        None = 0,
        Discover = 1,
        Get = 2,
        Add = 3,
        Set = 4,
        Delete = 5,
        All = 9
    }

}