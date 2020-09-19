namespace Shared
{
    public class SellerRegistrationForm
    {
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }
        public string DocumentLocation { get; set; }
        public string ReferrerCode { get; set; }
        public string PhoneNumber { get;set; }
        public int CampusId { get; set; }
        public int HostelId { get; set; }

        /// <summary>
        /// Throws invalid argument exception
        /// </summary>
        public void Validate()
        {

        }
    }
}
