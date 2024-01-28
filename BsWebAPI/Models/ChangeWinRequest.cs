namespace BsWebAPI.Models
{
    public class ChangeWinRequest
    {
        public Guid PrivateToken { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousAmount { get; set; }
        public int RTId { get; set; }
        public int PreviousRTId { get; set; }
    }
}
