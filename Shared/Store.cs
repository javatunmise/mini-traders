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
        public string LogoPath { get; set; }
        public string ReferrerCode { get; set; }
        public string PhoneNumber { get; set; }

        public int CampusId { get; set; }
        public int? HostelId { get; set; }
    }
}
