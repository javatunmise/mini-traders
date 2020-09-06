using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data
{
    public static class DbUtil
    {

        public static string SafeGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
