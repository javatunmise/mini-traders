using System;
using System.Collections.Generic;
using System.Text;

namespace Sellers
{
    public class Store
    {
        public Store()
        {
                
        }
        public Store(Shared.Account owner)
        {
            Owner = owner; 
        }

        public Shared.Account Owner { get; internal set; }
    }
}
