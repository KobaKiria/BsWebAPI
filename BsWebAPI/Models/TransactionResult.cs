namespace BsWebAPI.Models
{
    public class TransactionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public dynamic Data { get; set; }
    }
}
