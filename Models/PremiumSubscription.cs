namespace API.Models
{
    public class PremiumSubscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign Key
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PaymentDetails { get; set; }
    }

}
