using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }
}
