using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int StoreId { get; set; }
        public string Message { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }

        [NotMapped]
        public string ReviewerProfileImage { get; set; }

        public double Rating { get; set; }
        public bool HideUserIdentity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
