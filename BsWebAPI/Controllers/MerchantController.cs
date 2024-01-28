using BsWebAPI.Models;
using BsWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantRepository _merchantRepository;

        public MerchantController(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }
        [HttpPost("Auth")] 
        public IActionResult Auth(Guid publicToken)
        {
            try
            {
                (Guid privateToken, int statusCode) = _merchantRepository.GetPrivateToken(publicToken);

                if (statusCode == 200)
                {
                    return Ok(new
                    {
                        StatusCode = statusCode,
                        PrivateToken = privateToken
                    });
                }
                else
                {
                    return Ok(new { StatusCode = statusCode });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpPost("GetCurrentBalance")]
        public IActionResult GetCurrentBalance(Guid privateToken)
        {
            try
            {
                var balanceResult = _merchantRepository.GetCurrentBalance(privateToken);

                if (balanceResult.StatusCode == 200)
                {
                    return Ok(new
                    {
                        StatusCode = balanceResult.StatusCode,
                        Data = new
                        {
                            CurrentBalance = balanceResult.CurrentBalance
                        }
                    });
                }
                else
                {
                    return Ok(new { StatusCode = balanceResult.StatusCode });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }



        [HttpPost("GetPlayerInfo")]
        public IActionResult GetPlayerInfo(Guid privateToken)
        {
            try
            {
                var playerInfo = _merchantRepository.GetPlayerInfo(privateToken);

                if (playerInfo.StatusCode == 200)
                {
                    return Ok(new
                    {
                        StatusCode = playerInfo.StatusCode,
                        Data = new
                        {
                            UserId = playerInfo.UserId,
                            UserName = playerInfo.UserName,
                            CurrentBalance = playerInfo.CurrentBalance
                        }
                    });
                }
                else
                {
                    return Ok(new { StatusCode = playerInfo.StatusCode });
                }
            }
            catch (Exception)
            {
                throw;
            }
        
        }
       

        [HttpPost("ChangeWinTransaction")]
        public IActionResult ChangeWinTransaction(Guid PrivateToken, decimal Amount, decimal PreviousAmount, int RTId, int PreviousRTId)
        {
            try
            {
                var request = new ChangeWinRequest
                {
                    PrivateToken = PrivateToken,
                    Amount = Amount,
                    PreviousAmount = PreviousAmount,
                    RTId = RTId,
                    PreviousRTId = PreviousRTId
                };

                ChangeWinResult result = _merchantRepository.ChangeWinTransaction(request);

                if (result.Success)
                {
                    return Ok(new
                    {
                        StatusCode = result.StatusCode,
                        Data = new
                        {
                            TransactionId = result.TransactionId,
                            CurrentBalance = result.CurrentBalance
                        }
                    });
                }
                else
                {
                    return Ok(new { StatusCode = result.StatusCode });
                }
            }
            catch (Exception)
            {
                throw;
            }
          
        }

    

        [HttpPost("ProcessWinTransaction")]
        public IActionResult ProcessWinTransaction(Guid privateToken, decimal amount, int RTId)
        {
            try
            {
                var request = new WinTransactionRequest
                {
                    PrivateToken = privateToken,
                    Amount = amount,
                    RTId = RTId,
                };
                WinTransactionResult result = _merchantRepository.ProcessWinTransaction(request);

                if (result.Success)
                {
                    return Ok(new
                    {
                        StatusCode = result.StatusCode,
                        Data = new
                        {
                            TransactionId = result.TransactionId,
                            CurrentBalance = result.CurrentBalance
                        }
                    });
                }
                else
                {
                    return Ok(new { StatusCode = result.StatusCode });
                }
            }
            catch (Exception)
            {
                throw;
            }
      
        }


        [HttpPost("CancelBetTransaction")]
        public IActionResult CancelBetTransaction(Guid privateToken, decimal amount, int RTId, int BetTransactionId)
        {
            try
            {
                var request = new CancelBetTransactionRequest
                {
                    PrivateToken = privateToken,
                    Amount = amount,
                    RTId = RTId,
                    BetTransactionId = BetTransactionId
                };
                CancelBetTransactionResult result = _merchantRepository.CancelBetTransaction(request);

                if (result.Success)
                {
                    return Ok(new
                    {
                        StatusCode = result.StatusCode,
                        Data = new
                        {
                            TransactionId = result.TransactionId,
                            CurrentBalance = result.CurrentBalance
                        }
                    });
                }
                else
                {
                    return Ok(new { StatusCode = result.StatusCode });
                }
            }
            catch (Exception)
            {

                throw;
            }
       
        }

        [HttpPost("ProcessBetTransaction")]
        public IActionResult ProcessBetTransaction(Guid privateToken, decimal amount, int RTId)
        {
            try
            {
                var request = new BetTransactionRequest
                {
                    Token = privateToken,
                    Amount = amount,
                    RTId = RTId
                };
                var result = _merchantRepository.ProcessBetTransaction(request);

                if (result.Success)
                {
                    return Ok(new
                    {
                        StatusCode = result.StatusCode,
                        Data = new
                        {
                            TransactionId = result.TransactionId,
                            CurrentBalance = result.CurrentBalance
                        }
                    });
                }
                else
                {
                    return Ok(new { StatusCode = result.StatusCode });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
