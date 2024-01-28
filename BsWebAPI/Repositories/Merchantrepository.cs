using BsWebAPI.Models;
using BsWebAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BsWebAPI.Repositories
{
    public class Merchantrepository : IMerchantRepository
    {
        private readonly IConfiguration _configuration;

        public Merchantrepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public BalanceResult GetCurrentBalance(Guid privateToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PrivateToken", privateToken);
                parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                db.Execute("GetCurrentBalanceSP", parameters, commandType: CommandType.StoredProcedure);

                int statusCode = parameters.Get<int?>("@StatusCode") ?? 404;
                decimal currentBalance = parameters.Get<decimal?>("@CurrentBalance") ?? 0;

                return new BalanceResult
                {
                    StatusCode = statusCode,
                    CurrentBalance = currentBalance
                };
            }
        }



        public PlayerInfo GetPlayerInfo(Guid privateToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PrivateToken", privateToken);
                parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var playerInfo = db.QuerySingleOrDefault<PlayerInfo>(
                            "GetPlayerInfoSP",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                int statusCode = parameters.Get<int>("@StatusCode");

                if (playerInfo != null)
                {
                    playerInfo.StatusCode = statusCode; 
                }
                else
                {
                    return new PlayerInfo { StatusCode = statusCode }; 
                }

                return playerInfo;
            }
        }


        public (Guid privateToken, int statusCode) GetPrivateToken(Guid publicToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PublicToken", publicToken);
                parameters.Add("@PrivateToken", dbType: DbType.Guid, direction: ParameterDirection.Output);
                parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                db.Execute("GetPrivateTokenSP", parameters, commandType: CommandType.StoredProcedure);

                int statusCode = parameters.Get<int>("@StatusCode");
                Guid privateToken = statusCode == 200 ? parameters.Get<Guid>("@PrivateToken") : Guid.Empty;

                return (privateToken, statusCode);
            }
        }


        public ChangeWinResult ChangeWinTransaction(ChangeWinRequest request)
        {
            ChangeWinResult response = new ChangeWinResult();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PrivateToken", request.PrivateToken);
                parameters.Add("@Amount", request.Amount);
                parameters.Add("@PreviousAmount", request.PreviousAmount);
                parameters.Add("@RTId", request.RTId);
                parameters.Add("@PreviousRTId", request.PreviousRTId);
                parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@NewBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("ChangeWinTransactionSP", parameters, commandType: CommandType.StoredProcedure);

                response.StatusCode = parameters.Get<int>("@StatusCode");
                response.Success = response.StatusCode == 200;
                if (response.Success)
                {
                    response.TransactionId = parameters.Get<int>("@TransactionId");
                    response.CurrentBalance = parameters.Get<decimal>("@NewBalance");
                }
            }

            return response;
        }



        public WinTransactionResult ProcessWinTransaction(WinTransactionRequest request)
        {
            WinTransactionResult response = new WinTransactionResult();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PrivateToken", request.PrivateToken);
                    parameters.Add("@Amount", request.Amount);
                    parameters.Add("@RTId", request.RTId);
                    parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@NewBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                    parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("ProcessWinTransactionSP", parameters, commandType: CommandType.StoredProcedure);

                    response.StatusCode = parameters.Get<int>("@StatusCode");
                    if (response.StatusCode == 200)
                    {
                        response.TransactionId = parameters.Get<int?>("@TransactionId");
                        response.CurrentBalance = parameters.Get<decimal?>("@NewBalance");
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }


        public CancelBetTransactionResult CancelBetTransaction(CancelBetTransactionRequest request)
        {
            CancelBetTransactionResult response = new CancelBetTransactionResult();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PrivateToken", request.PrivateToken);
                    parameters.Add("@Amount", request.Amount);
                    parameters.Add("@RTId", request.RTId);
                    parameters.Add("@BetTransactionId", request.BetTransactionId);
                    parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@NewBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                    parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("CancelBetTransactionSP", parameters, commandType: CommandType.StoredProcedure);

                    response.StatusCode = parameters.Get<int>("@StatusCode");
                    if (response.StatusCode == 200)
                    {
                        response.TransactionId = parameters.Get<int?>("@TransactionId");
                        response.CurrentBalance = parameters.Get<decimal?>("@NewBalance");
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }

        public BetTransactionResult ProcessBetTransaction(BetTransactionRequest request)
        {
            BetTransactionResult response = new BetTransactionResult();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PrivateToken", request.Token);
                    parameters.Add("@Amount", request.Amount);
                    parameters.Add("@RTId", request.RTId);
                    parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@NewBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                    parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("ProcessBetTransaction", parameters, commandType: CommandType.StoredProcedure);

                    response.StatusCode = parameters.Get<int>("@StatusCode");
                    if (response.StatusCode == 200)
                    {
                        response.TransactionId = parameters.Get<int>("@TransactionId");
                        response.CurrentBalance = parameters.Get<decimal>("@NewBalance");
                        response.Success = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }
    }
}
