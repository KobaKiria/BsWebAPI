namespace BsWebAPI.Models
{
    public class WinTransactionResult
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public int? TransactionId { get; set; }
        public decimal? CurrentBalance { get; set; }
    }
}
