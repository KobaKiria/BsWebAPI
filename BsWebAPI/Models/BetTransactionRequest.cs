namespace BsWebAPI.Models
{
    public class BetTransactionRequest
    {
        public Guid Token { get; set; }
        public decimal Amount { get; set; }
        public int RTId { get; set; }

    }
}
