using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Store
    {
        public Store(Account account)
        {
            Owner = account;
        }

        public Account Owner { get; private set; }
        public string Name { get; set; }
        public string StoreDescription { get; set; }
        public string UploadedDocLocation { get; set; }
        public string ReferrerCode { get; set; }

        public int CampusId { get; set; }
        public int HostelId { get; set; }

    }
}
