namespace Shared.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }
        public string LogoPath { get; set; }
        public string UploadedDocPath { get; set; }
        public string ReferrerCode { get; set; }
        public string PhoneNumber { get; set; }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }

        public int HostelId { get; set; }
        public Hostel Hostel { get; set; }

        public int SiteUserId { get; set; }
        public SiteUser User { get; set; }
    }
}
