using System;

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

        public StoreStatuses Status { get; set; }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }
        public int? HostelId { get; set; }
        public Hostel Hostel { get; set; }
        public int SiteUserId { get; set; }
        public SiteUser User { get; set; }
        public bool IsDocumentVerified { get; set; }
        public DateTime? LastSubscribedOn { get; set; }
        public decimal LastSubscriptionAmount { get; set; }
        public DateTime? SubscriptionExpiresOn { get; set; }
        public DateTime? ActivatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class StoreView
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }
        public string LogoPath { get; set; }
        public string UploadedDocPath { get; set; }
        public string ReferrerCode { get; set; }
        public string PhoneNumber { get; set; }

        public StoreStatuses Status { get; set; }

        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public int? HostelId { get; set; }
        public string HostelName { get; set; }
        public int SiteUserId { get; set; }
        public SiteUser User { get; set; }
        public bool IsDocumentVerified { get; set; }
        public DateTime? LastSubscribedOn { get; set; }
        public decimal LastSubscriptionAmount { get; set; }
        public DateTime? SubscriptionExpiresOn { get; set; }
        public DateTime? ActivatedOn { get; set; }
    }


    public enum StoreStatuses
    {
        Inactive = 0,
        Active = 1,
        Deactivated = 2,
        Suspended = 3
    }
}
