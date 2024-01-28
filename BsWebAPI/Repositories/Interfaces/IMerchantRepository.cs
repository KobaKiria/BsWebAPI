using BsWebAPI.Models;

namespace BsWebAPI.Repositories.Interfaces
{
    public interface IMerchantRepository
    {
        public ChangeWinResult ChangeWinTransaction(ChangeWinRequest request);
        public BetTransactionResult ProcessBetTransaction(BetTransactionRequest request);
        public WinTransactionResult ProcessWinTransaction(WinTransactionRequest request);
        public CancelBetTransactionResult CancelBetTransaction(CancelBetTransactionRequest request);
        public PlayerInfo GetPlayerInfo(Guid privateToken);
        (Guid privateToken, int statusCode) GetPrivateToken(Guid publicToken);
        public BalanceResult GetCurrentBalance(Guid PrivateToken);
    }
}
