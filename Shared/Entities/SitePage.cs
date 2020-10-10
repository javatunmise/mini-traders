using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class SitePage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
