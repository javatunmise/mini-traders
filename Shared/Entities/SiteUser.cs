namespace Shared.Entities
{
    public class SiteUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Store Store { get; set; }
        public string ProfilePicturePath { get; set; }
        public string ReferralCode { get; set; }
        public int? CampusId { get; set; }
        public Campus Location { get; set; }
        public string WalletAccountCode { get; set; }
        public string TokenAccountCode { get; set; }
    }
}
