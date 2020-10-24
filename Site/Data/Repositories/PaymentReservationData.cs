using Sellers;

namespace site.Data.Repositories
{
    public class PaymentReservationData
    {
        public string PaymentRef { get; protected set; }
        public string EntityId { get; protected set; }
        public string EntityType { get; protected set; }
        public string Purpose { get; protected set; }
        public decimal Amount { get; protected set; }
        public string Email { get; protected set; }
        public decimal Charge { get; set; }
    }

    public class ActivateStorePaymentReservationData : PaymentReservationData
    {
        public ActivateStorePaymentReservationData(Shared.Store store, decimal amount, string payRef, string email = "")
        {
            Amount = amount;
            EntityId = store.Id.ToString();
            EntityType = "STORE";
            Purpose = $"Activate store '{store.Name}'";
            PaymentRef = payRef;
            Email = email;
        }
    }
}