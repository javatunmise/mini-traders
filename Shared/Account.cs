using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Account
    {
        public bool IsActive { get; internal set; }
        public int Id { get; internal set; }
        public bool HasStore { get; internal set; }
    }
}
