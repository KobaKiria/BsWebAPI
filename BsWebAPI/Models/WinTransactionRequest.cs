namespace BsWebAPI.Models
{
    public class WinTransactionRequest
    {
        public Guid PrivateToken { get; set; }
        public decimal Amount { get; set; }
        public int RTId { get; set; }
    }
}
