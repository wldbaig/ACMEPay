using System;

#nullable disable

namespace API.Repository
{
    public partial class Payment
    {
        public long Id { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public short ExpiryMonth { get; set; }
        public short ExpiryYear { get; set; }
        public short Cvv { get; set; }
        public string OrderReference { get; set; }
        public short Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
