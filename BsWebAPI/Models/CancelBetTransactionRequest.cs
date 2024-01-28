namespace BsWebAPI.Models
{
    public class CancelBetTransactionRequest
    {
        public Guid PrivateToken { get; set; }
        public decimal Amount { get; set; }
        public int RTId { get; set; }
        public int BetTransactionId { get; set; }
    }
}
