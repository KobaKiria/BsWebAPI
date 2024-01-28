namespace BsWebAPI.Models
{
    public class CancelBetTransactionResult
    {
        public int StatusCode { get; set; }
        public int? TransactionId { get; set; }
        public decimal? CurrentBalance { get; set; }
        public bool Success { get; set; }
    }
}
