using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly IBasePage _basePage;
        private readonly ILogger<TransactionController> _logger;
        private readonly IFrontTrnService _frontTrnService;
        private readonly ITransactionProcess _transactionProcess;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICancelOrderProcess _cancelOrderProcess;
        string dummyResponce = "";
        static int i = 1;

        public TransactionController(ILogger<TransactionController> logger, IBasePage basePage, IFrontTrnService frontTrnService, UserManager<ApplicationUser> userManager, ITransactionProcess transactionProcess, ICancelOrderProcess cancelOrderProcess)
        {
            _logger = logger;
            _basePage = basePage;
            _frontTrnService = frontTrnService;
            _transactionProcess = transactionProcess;
            _userManager = userManager;
            _cancelOrderProcess = cancelOrderProcess;
        }

        private ActionResult returnDynamicResult(dynamic respObjJson)
        {
            i++;
            if (i % 2 == 0)
            {
                return Ok(respObjJson);
            }
            else if (i % 3 == 0)
            {
                return BadRequest();
            }
            else if (i % 5 == 0)
            {
                return Unauthorized();
            }
            else if (i % 7 == 0)
            {
                return NotFound();
            }
            else if (i % 9 == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(respObjJson);
            }

        }

        #region "Methods"

        [HttpGet("GetTradePairAsset")]
        public IActionResult GetTradePairAsset()
        {
            TradePairAssetResponce Response = new TradePairAssetResponce();
            try
            {
                var responsedata = _frontTrnService.GetTradePairAsset();
                if (responsedata != null && responsedata.Count != 0)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost("CreateTransactionOrder/{Pair}")]
        [Authorize]
        public async Task<ActionResult> CreateTransactionOrder([FromBody]CreateTransactionRequest Request, string Pair)
        {
            //Do Process for CreateOrder
            //For Testing Purpose
            var user = await _userManager.GetUserAsync(HttpContext.User);
            NewTransactionRequestCls Req = new NewTransactionRequestCls();
            Req.TrnMode = Request.TrnMode;
            Req.TrnType = Request.OrderSide;
            Req.ordertype = Request.OrderType;
            Req.MemberID = user.Id;
            Req.MemberMobile = user.Mobile;
            //Req.MemberID = 5;
            //Req.MemberMobile = "1234567890";
            Req.SMSCode = Pair;
            Req.TransactionAccount = Request.CurrencyPairID.ToString();
            Req.Amount = Request.Total;
            Req.PairID = Request.CurrencyPairID;
            Req.Price = Request.Price;
            Req.Qty = Request.Amount;
            Req.DebitAccountID = Request.DebitWalletID;
            Req.CreditAccountID = Request.CreditWalletID;          

            //BizResponse myResp = await _transactionProcess.ProcessNewTransactionAsync(Req);           
            // var myResp = new Task(async()=>_transactionProcess.ProcessNewTransactionAsync(Req));

            CreateTransactionResponse Response = new CreateTransactionResponse();
            Task<BizResponse> MethodRespTsk = _transactionProcess.ProcessNewTransactionAsync(Req);
            BizResponse MethodResp = await MethodRespTsk;

            if (MethodResp.ReturnCode == enResponseCodeService.Success)
                Response.ReturnCode = enResponseCode.Success;
            else if (MethodResp.ReturnCode == enResponseCodeService.Fail)
                Response.ReturnCode = enResponseCode.Fail;
            else if (MethodResp.ReturnCode == enResponseCodeService.InternalError)
                Response.ReturnCode = enResponseCode.InternalError;

            Response.ReturnMsg = MethodResp.ReturnMsg;
            Response.ErrorCode = MethodResp.ErrorCode;

            Response.response = new CreateOrderInfo()
            {
                TrnID=Req.GUID
                //order_id = 1000001,
                //pair_name = "ltcusd",
                //price = 10,
                //side = "buy",
                //type = "stop-loss",
                //volume = 10
            };

            //Response.ReturnCode = enResponseCode.Success;
            return returnDynamicResult(Response);
        }

        [HttpPost("Withdrawal")]
        //[Authorize]
        public async Task<ActionResult> Withdrawal([FromBody]WithdrawalRequest Request)
        {
            //Do Process for CreateOrder
            //For Testing Purpose
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            NewTransactionRequestCls Req = new NewTransactionRequestCls();
            Req.TrnMode = Request.TrnMode;
            Req.TrnType = enTrnType.Withdraw;
            //Req.MemberID = user.Id;
            //Req.MemberMobile = user.Mobile;
            Req.MemberID = 16;
            Req.MemberMobile = "1234567890";
            Req.SMSCode = Request.asset;
            Req.TransactionAccount = Request.address;
            Req.Amount = Request.Amount;
            Req.DebitAccountID = Request.DebitWalletID;
            Req.AddressLabel = Request.AddressLabel;
            Req.WhitelistingBit = Request.WhitelistingBit;

            //BizResponse myResp = await _transactionProcess.ProcessNewTransactionAsync(Req);           
            // var myResp = new Task(async()=>_transactionProcess.ProcessNewTransactionAsync(Req));

            CreateTransactionResponse Response = new CreateTransactionResponse();
            Task<BizResponse> MethodRespTsk = _transactionProcess.ProcessNewTransactionAsync(Req);
            BizResponse MethodResp = await MethodRespTsk;

            if (MethodResp.ReturnCode == enResponseCodeService.Success)
                Response.ReturnCode = enResponseCode.Success;
            else if (MethodResp.ReturnCode == enResponseCodeService.Fail)
                Response.ReturnCode = enResponseCode.Fail;
            else if (MethodResp.ReturnCode == enResponseCodeService.InternalError)
                Response.ReturnCode = enResponseCode.InternalError;

            Response.ReturnMsg = MethodResp.ReturnMsg;
            Response.ErrorCode = MethodResp.ErrorCode;

            Response.response = new CreateOrderInfo()
            {
                TrnID = Req.GUID
                //order_id = 1000001,
                //pair_name = "ltcusd",
                //price = 10,
                //side = "buy",
                //type = "stop-loss",
                //volume = 10
            };

            //Response.ReturnCode = enResponseCode.Success;
            return returnDynamicResult(Response);
        }

        [HttpPost("CancelOrder")]
       // [Authorize]
        public async Task<ActionResult> CancelOrder([FromBody]CancelOrderRequest Request)
        {
            try
            {
                BizResponse MethodRespCancel = _cancelOrderProcess.ProcessCancelOrder(Request);
                return Ok(MethodRespCancel);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet("GetVolumeData/{BasePair}")]
        public IActionResult GetVolumeData(string BasePair)
        {
            GetVolumeDataResponse Response = new GetVolumeDataResponse();
            try
            {
                long BasePairId = _frontTrnService.GetBasePairIdByName(BasePair);
                if (BasePairId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                var responsedata = _frontTrnService.GetVolumeData(BasePairId);
                if (responsedata != null && responsedata.Count != 0)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost("GetTradeHistory")]
        [Authorize]
        public async Task<ActionResult> GetTradeHistory([FromBody] TradeHistoryRequest request)
        {
            GetTradeHistoryResponse Response = new GetTradeHistoryResponse();
            Int16 trnType = 999, marketType = 999, status = 999;
            //
            long PairId = 999;
            string sCondition = "1=1";
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (!string.IsNullOrEmpty(request.Pair))
                {
                    if (!_frontTrnService.IsValidPairName(request.Pair))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                    PairId = _frontTrnService.GetPairIdByName(request.Pair);
                    if (PairId == 0)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                    sCondition += " And TTQ.PairID=" + PairId;
                }
                if (!string.IsNullOrEmpty(request.Trade))
                {
                    trnType = _frontTrnService.IsValidTradeType(request.Trade);
                    if (trnType == 999)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InValidTrnType;
                        return BadRequest(Response);
                    }
                    sCondition += " AND TTQ.TrnType=" + trnType;
                }
                if (!string.IsNullOrEmpty(request.MarketType))
                {
                    marketType = _frontTrnService.IsValidMarketType(request.MarketType);
                    if (marketType == 999)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidMarketType;
                        return BadRequest(Response);
                    }
                    sCondition += " AND TSL.ordertype=" + marketType;
                }
                if (!string.IsNullOrEmpty(request.FromDate))
                {
                    if (string.IsNullOrEmpty(request.ToDate))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidFromDateFormate;
                        return BadRequest(Response);
                    }
                    if (!_frontTrnService.IsValidDateFormate(request.FromDate))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidFromDateFormate;
                        return BadRequest(Response);
                    }
                    if (!_frontTrnService.IsValidDateFormate(request.ToDate))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidToDateFormate;
                        return BadRequest(Response);
                    }
                    //sCondition += " AND TTQ.TrnDate Between '" + fDate  + " AND '" + tDate  + "' ";
                    sCondition += "AND TTQ.TrnDate Between {0} AND {1} ";
                }
                if ((request.Status.ToString()) == "0")
                {
                    status = 999;
                }
                else
                {
                    if (request.Status != 1 && request.Status != 2 && request.Status != 9)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidStatusType;
                        return Ok(Response);
                    }
                    status = Convert.ToInt16(request.Status);
                }

                //if (request.Page == 0)
                //{
                //    Response.ReturnCode = enResponseCode.Fail;
                //    Response.ErrorCode = enErrorCode.InValidPageNo;
                //    return BadRequest(Response);
                //}

                long MemberID =user.Id;
                Response.response = _frontTrnService.GetTradeHistory(MemberID, sCondition, request.FromDate, request.ToDate, request.Page, status);
                if (Response.response.Count == 0)
                {
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    Response.ReturnCode = enResponseCode.Success;
                    return Ok(Response);
                }
                Response.ReturnCode = enResponseCode.Success;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpPost("GetActiveOrder")]  //GetActiveOrder
        [Authorize]
        public async Task<ActionResult> GetActiveOrder([FromBody]GetActiveOrderRequest request)
        {
            GetActiveOrderResponse Response = new GetActiveOrderResponse();
            Int16 trnType = 999;
            string sCondition = "1=1";
            long PairId = 999;
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (!string.IsNullOrEmpty(request.Pair))
                {
                    if (!_frontTrnService.IsValidPairName(request.Pair))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                    PairId = _frontTrnService.GetPairIdByName(request.Pair);
                    if (PairId == 0)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                    sCondition += " And TTQ.PairID=" + PairId;
                }
                //if (request.Page == 0)
                //{
                //    Response.ReturnCode = enResponseCode.Fail;
                //    Response.ErrorCode = enErrorCode.InValidPageNo;
                //    return BadRequest(Response);
                //}
                if (!string.IsNullOrEmpty(request.OrderType))
                {
                    trnType = _frontTrnService.IsValidTradeType(request.OrderType);
                    if (trnType == 999)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InValidTrnType;
                        return BadRequest(Response);
                    }
                    sCondition += " AND TTQ.TrnType=" + trnType +" ";
                }
                if (!string.IsNullOrEmpty(request.FromDate))
                {
                    if (string.IsNullOrEmpty(request.ToDate))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidFromDateFormate;
                        return BadRequest(Response);
                    }
                    if (!_frontTrnService.IsValidDateFormate(request.FromDate))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidFromDateFormate;
                        return BadRequest(Response);
                    }
                    if (!_frontTrnService.IsValidDateFormate(request.ToDate))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidToDateFormate;
                        return BadRequest(Response);
                    }
                    //sCondition += " AND TTQ.TrnDate Between '" + fDate  + " AND '" + tDate  + "' ";
                    sCondition += "AND TTQ.TrnDate Between {2} AND {3} ";
                }
                long MemberID =user.Id;
                Response.response = _frontTrnService.GetActiveOrder(MemberID, sCondition,request .FromDate,request .ToDate, PairId, request.Page);
                if (Response.response.Count == 0)
                {
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    Response.ReturnCode = enResponseCode.Success;
                    return BadRequest(Response);
                }
                Response.ReturnCode = enResponseCode.Success;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpPost("GetRecentOrder")] //binance https://api.binance.com//api/v1/trades?symbol=LTCBTC
        [Authorize]
        public async Task<IActionResult> GetRecentOrder(string Pair="999")
        {
            long PairId = 999;
            GetRecentTradeResponce Response = new GetRecentTradeResponce();
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if(Pair != "999")
                {
                    if (!_frontTrnService.IsValidPairName(Pair))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                    PairId = _frontTrnService.GetPairIdByName(Pair);
                    if (PairId == 0)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                }
                long MemberID =user.Id;
                Response.response = _frontTrnService.GetRecentOrder(PairId,MemberID);
                if (Response.response.Count == 0)
                {
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    Response.ReturnCode = enResponseCode.Success;
                    return BadRequest(Response);
                }
                Response.ReturnCode = enResponseCode.Success;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet("GetOrderhistory")]
        public ActionResult GetOrderhistory(string Pair="999")
        {
            GetTradeHistoryResponse Response = new GetTradeHistoryResponse();
            long PairId = 999;
            try
            {
                if(Pair != "999")
                {
                    if (!_frontTrnService.IsValidPairName(Pair))
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                    PairId = _frontTrnService.GetPairIdByName(Pair);
                    if (PairId == 0)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ErrorCode = enErrorCode.InvalidPairName;
                        return BadRequest(Response);
                    }
                }
                Response.response = _frontTrnService.GetTradeHistory(PairId, "", "", "", 0, 0);
                Response.ReturnCode = enResponseCode.Success;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }
        
        [HttpGet("GetBuyerBook/{Pair}")]
        public ActionResult GetBuyerBook(string Pair)
        {
            GetBuySellBookResponse Response = new GetBuySellBookResponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                var responsedata = _frontTrnService.GetBuyerBook(id);
                if (responsedata != null && responsedata.Count != 0)
                {
                    Response.response = responsedata;
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpGet("GetSellerBook/{Pair}")]
        public ActionResult GetSellerBook(string Pair)
        {
            GetBuySellBookResponse Response = new GetBuySellBookResponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                var responsedata = _frontTrnService.GetSellerBook(id);
                if (responsedata != null && responsedata.Count != 0)
                {
                    Response.response = responsedata;
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpGet("GetTradePairByName/{Pair}")]
        public ActionResult GetTradePairByName(string Pair)
        {
            TradePairByNameResponse Response = new TradePairByNameResponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                Response.response = _frontTrnService.GetTradePairByName(id);
                Response.ReturnCode = enResponseCode.Success;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }

        }

        [HttpGet("GetGraphDetail/{Pair}/{Interval}")]
        public ActionResult GetGraphDetail(string Pair,string Interval)
        {
            int IntervalTime = 0;
            string IntervalData = "";
            GetGraphDetailReponse Response = new GetGraphDetailReponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                _frontTrnService.GetIntervalTimeValue(Interval, ref IntervalTime, ref IntervalData);
                if(IntervalTime == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.Graph_InvalidIntervalTime;
                    return Ok(Response);
                }
                var responsedata = _frontTrnService.GetGraphDetail(id, IntervalTime,IntervalData);
                if (responsedata != null && responsedata.Count != 0)
                {
                    Response.response = responsedata;
                    Response.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet("GetMarketCap/{Pair}")]
        public ActionResult GetMarketCap(string Pair)
        {
            MarketCapResponse Response = new MarketCapResponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                Response.response = _frontTrnService.GetMarketCap(id);
                Response.ReturnCode = enResponseCode.Success;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet("GetVolumeDataByPair/{Pair}")]
        public IActionResult GetVolumeDataByPair(string Pair)
        {
            GetVolumeDataByPairResponse Response = new GetVolumeDataByPairResponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                var responsedata = _frontTrnService.GetVolumeDataByPair(id);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet("GetPairRates/{Pair}")]
        public IActionResult GetPairRates(string Pair)
        {
            GetPairRatesResponse Response = new GetPairRatesResponse();
            try
            {
                if (!_frontTrnService.IsValidPairName(Pair))
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidPairName;
                    return Ok(Response);
                }
                long id = _frontTrnService.GetPairIdByName(Pair);
                if (id == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                    return Ok(Response);
                }
                var responsedata = _frontTrnService.GetPairRates(id);
                if (responsedata != null)
                {
                    Response.ReturnCode = enResponseCode.Success;
                    Response.response = responsedata;
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.NoDataFound;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("AddToFavouritePair/{PairId}")]
        public async Task<IActionResult> AddToFavouritePair(long PairId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                if (PairId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.FavPair_InvalidPairId;
                    Response.ErrorCode = enErrorCode.FavPair_InvalidPairId;
                }
                else
                {
                    var UserId = user.Id;
                    var returnCode = _frontTrnService.AddToFavouritePair(PairId, UserId);
                    if(returnCode == 2)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.FavPair_InvalidPairId;
                        Response.ErrorCode = enErrorCode.FavPair_InvalidPairId;
                    }
                    if(returnCode == 1)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.FavPair_AlreadyAdded;
                        Response.ErrorCode = enErrorCode.FavPair_AlreadyAdded;
                    }
                    else if(returnCode == 0)
                    {
                        Response.ReturnCode = enResponseCode.Success;
                        Response.ReturnMsg = EnResponseMessage.FavPair_AddedSuccess;
                        Response.ErrorCode = enErrorCode.FavPair_AddedSuccess;
                    }
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("RemoveFromFavouritePair/{PairId}")]
        public async Task<IActionResult> RemoveFromFavouritePair(long PairId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                if (PairId == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.FavPair_InvalidPairId;
                    Response.ErrorCode = enErrorCode.FavPair_InvalidPairId;
                }
                else
                {
                    var UserId = user.Id;
                    var returnCode = _frontTrnService.RemoveFromFavouritePair(PairId, UserId);
                    if (returnCode == 1)
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.FavPair_InvalidPairId;
                        Response.ErrorCode = enErrorCode.FavPair_InvalidPairId;
                    }
                    else if (returnCode == 0)
                    {
                        Response.ReturnCode = enResponseCode.Success;
                        Response.ReturnMsg = EnResponseMessage.FavPair_RemoveSuccess;
                        Response.ErrorCode = enErrorCode.FavPair_RemoveSuccess;
                    }
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("GetFavouritePair")]
        public async Task<IActionResult> GetFavouritePair()
        {
            FavoritePairResponse Response = new FavoritePairResponse();
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    var UserId = user.Id;
                    var response = _frontTrnService.GetFavouritePair(UserId);
                    if (response != null && response.Count != 0)
                    {
                        Response.response = response;
                        Response.ReturnCode = enResponseCode.Success;
                    }
                    else
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.FavPair_NoPairFound;
                        Response.ErrorCode = enErrorCode.FavPair_NoPairFound;
                    }
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        #endregion


        //[HttpGet("GetTickerInformation")] //get Trade Pair Binnance https://api.binance.com/api/v3/ticker/bookTicker //https://api.binance.com/api/v3/ticker/bookTicker?symbol=LTCBTC
        //public IActionResult GetTickerInformation(TradePairAssetRequest request)
        //{
        //    TickerWrapper tickerWrapper = new TickerWrapper();

        //    dummyResponce = "[{'symbol':'ETHBTC','bidPrice':'0.03386200','bidQty':'4.12200000','askPrice':'0.03387100','askQty':'28.00000000'},{'symbol':'LTCBTC','bidPrice':'0.00891300','bidQty':'10.00000000','askPrice':'0.00892800','askQty':'3.21000000'},{'symbol':'BNBBTC','bidPrice':'0.00157720','bidQty':'1.29000000','askPrice':'0.00157830','askQty':'1.36000000'},{'symbol':'NEOBTC','bidPrice':'0.00276900','bidQty':'0.72000000','askPrice':'0.00277300','askQty':'2.00000000'},{'symbol':'QTUMETH','bidPrice':'0.01698400','bidQty':'80.40000000','askPrice':'0.01707000','askQty':'75.17000000'},{'symbol':'EOSETH','bidPrice':'0.02514500','bidQty':'43.52000000','askPrice':'0.02518200','askQty':'64.70000000'},{'symbol':'SNTETH','bidPrice':'0.00016829','bidQty':'60.00000000','askPrice':'0.00016890','askQty':'5112.00000000'},{'symbol':'BNTETH','bidPrice':'0.00657800','bidQty':'53.82000000','askPrice':'0.00658100','askQty':'0.98000000'},{'symbol':'BCCBTC','bidPrice':'0.08001500','bidQty':'0.02400000','askPrice':'0.08011200','askQty':'5.57900000'},{'symbol':'GASBTC','bidPrice':'0.00086300','bidQty':'2.50000000','askPrice':'0.00086700','askQty':'58.32000000'},{'symbol':'BNBETH','bidPrice':'0.04647100','bidQty':'70.34000000','askPrice':'0.04662300','askQty':'1.46000000'},{'symbol':'BTCUSDT','bidPrice':'6465.00000000','bidQty':'0.18734500','askPrice':'6466.99000000','askQty':'0.20946400'},{'symbol':'ETHUSDT','bidPrice':'218.80000000','bidQty':'0.59376000','askPrice':'218.91000000','askQty':'0.00158000'},{'symbol':'HSRBTC','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'OAXETH','bidPrice':'0.00079990','bidQty':'423.00000000','askPrice':'0.00080590','askQty':'46.00000000'},{'symbol':'DNTETH','bidPrice':'0.00010608','bidQty':'2821.00000000','askPrice':'0.00010657','askQty':'13028.00000000'},{'symbol':'MCOETH','bidPrice':'0.02041800','bidQty':'5.20000000','askPrice':'0.02050900','askQty':'4.13000000'},{'symbol':'ICNETH','bidPrice':'0.00169710','bidQty':'373.00000000','askPrice':'0.00170540','askQty':'12.00000000'},{'symbol':'MCOBTC','bidPrice':'0.00069000','bidQty':'105.88000000','askPrice':'0.00069500','askQty':'27.63000000'},{'symbol':'WTCBTC','bidPrice':'0.00043450','bidQty':'10.25000000','askPrice':'0.00043550','askQty':'5.64000000'},{'symbol':'WTCETH','bidPrice':'0.01280900','bidQty':'13.80000000','askPrice':'0.01286300','askQty':'12.47000000'},{'symbol':'LRCBTC','bidPrice':'0.00001633','bidQty':'475.00000000','askPrice':'0.00001640','askQty':'1898.00000000'},{'symbol':'LRCETH','bidPrice':'0.00048264','bidQty':'1056.00000000','askPrice':'0.00048294','askQty':'59.00000000'},{'symbol':'QTUMBTC','bidPrice':'0.00057500','bidQty':'1197.10000000','askPrice':'0.00057700','askQty':'80.06000000'},{'symbol':'YOYOBTC','bidPrice':'0.00000433','bidQty':'46534.00000000','askPrice':'0.00000435','askQty':'24217.00000000'},{'symbol':'OMGBTC','bidPrice':'0.00053000','bidQty':'243.62000000','askPrice':'0.00053100','askQty':'401.52000000'},{'symbol':'OMGETH','bidPrice':'0.01564900','bidQty':'23.39000000','askPrice':'0.01569900','askQty':'6.76000000'},{'symbol':'ZRXBTC','bidPrice':'0.00009389','bidQty':'38.00000000','askPrice':'0.00009405','askQty':'147.00000000'},{'symbol':'ZRXETH','bidPrice':'0.00277181','bidQty':'4.00000000','askPrice':'0.00277999','askQty':'26.00000000'},{'symbol':'STRATBTC','bidPrice':'0.00022860','bidQty':'6.16000000','askPrice':'0.00022960','askQty':'47.84000000'},{'symbol':'STRATETH','bidPrice':'0.00673800','bidQty':'112.17000000','askPrice':'0.00676300','askQty':'822.88000000'},{'symbol':'SNGLSBTC','bidPrice':'0.00000372','bidQty':'10273.00000000','askPrice':'0.00000375','askQty':'9426.00000000'},{'symbol':'SNGLSETH','bidPrice':'0.00011004','bidQty':'6250.00000000','askPrice':'0.00011116','askQty':'471.00000000'},{'symbol':'BQXBTC','bidPrice':'0.00004870','bidQty':'814.00000000','askPrice':'0.00004887','askQty':'65.00000000'},{'symbol':'BQXETH','bidPrice':'0.00143550','bidQty':'73.00000000','askPrice':'0.00144210','askQty':'180.00000000'},{'symbol':'KNCBTC','bidPrice':'0.00005961','bidQty':'220.00000000','askPrice':'0.00005991','askQty':'168.00000000'},{'symbol':'KNCETH','bidPrice':'0.00175580','bidQty':'220.00000000','askPrice':'0.00176860','askQty':'65.00000000'},{'symbol':'FUNBTC','bidPrice':'0.00000238','bidQty':'106201.00000000','askPrice':'0.00000239','askQty':'109444.00000000'},{'symbol':'FUNETH','bidPrice':'0.00007018','bidQty':'4066.00000000','askPrice':'0.00007038','askQty':'5448.00000000'},{'symbol':'SNMBTC','bidPrice':'0.00000817','bidQty':'10725.00000000','askPrice':'0.00000820','askQty':'1245.00000000'},{'symbol':'SNMETH','bidPrice':'0.00024078','bidQty':'124.00000000','askPrice':'0.00024260','askQty':'4326.00000000'},{'symbol':'NEOETH','bidPrice':'0.08176500','bidQty':'0.16000000','askPrice':'0.08190000','askQty':'166.41000000'},{'symbol':'IOTABTC','bidPrice':'0.00008526','bidQty':'677.00000000','askPrice':'0.00008540','askQty':'335.00000000'},{'symbol':'IOTAETH','bidPrice':'0.00251568','bidQty':'150.00000000','askPrice':'0.00252110','askQty':'5.00000000'},{'symbol':'LINKBTC','bidPrice':'0.00004814','bidQty':'22.00000000','askPrice':'0.00004823','askQty':'3762.00000000'},{'symbol':'LINKETH','bidPrice':'0.00142301','bidQty':'79.00000000','askPrice':'0.00142806','askQty':'1078.00000000'},{'symbol':'XVGBTC','bidPrice':'0.00000230','bidQty':'369590.00000000','askPrice':'0.00000231','askQty':'270943.00000000'},{'symbol':'XVGETH','bidPrice':'0.00006782','bidQty':'28557.00000000','askPrice':'0.00006801','askQty':'893.00000000'},{'symbol':'SALTBTC','bidPrice':'0.00007310','bidQty':'865.85000000','askPrice':'0.00007330','askQty':'1522.87000000'},{'symbol':'SALTETH','bidPrice':'0.00215700','bidQty':'139.00000000','askPrice':'0.00216400','askQty':'179.63000000'},{'symbol':'MDABTC','bidPrice':'0.00006672','bidQty':'156.00000000','askPrice':'0.00006688','askQty':'1633.00000000'},{'symbol':'MDAETH','bidPrice':'0.00196340','bidQty':'156.00000000','askPrice':'0.00198120','askQty':'115.00000000'},{'symbol':'MTLBTC','bidPrice':'0.00010150','bidQty':'291.32000000','askPrice':'0.00010180','askQty':'4317.09000000'},{'symbol':'MTLETH','bidPrice':'0.00299000','bidQty':'32.60000000','askPrice':'0.00301400','askQty':'349.52000000'},{'symbol':'SUBBTC','bidPrice':'0.00001939','bidQty':'218.00000000','askPrice':'0.00001942','askQty':'524.00000000'},{'symbol':'SUBETH','bidPrice':'0.00057045','bidQty':'20880.00000000','askPrice':'0.00057204','askQty':'18.00000000'},{'symbol':'EOSBTC','bidPrice':'0.00085070','bidQty':'58.63000000','askPrice':'0.00085180','askQty':'14.07000000'},{'symbol':'SNTBTC','bidPrice':'0.00000569','bidQty':'61978.00000000','askPrice':'0.00000573','askQty':'45533.00000000'},{'symbol':'ETCETH','bidPrice':'0.04989500','bidQty':'0.75000000','askPrice':'0.04995800','askQty':'1.76000000'},{'symbol':'ETCBTC','bidPrice':'0.00169000','bidQty':'293.05000000','askPrice':'0.00169200','askQty':'34.32000000'},{'symbol':'MTHBTC','bidPrice':'0.00000445','bidQty':'38113.00000000','askPrice':'0.00000449','askQty':'24915.00000000'},{'symbol':'MTHETH','bidPrice':'0.00013124','bidQty':'5798.00000000','askPrice':'0.00013257','askQty':'7221.00000000'},{'symbol':'ENGBTC','bidPrice':'0.00009339','bidQty':'87.00000000','askPrice':'0.00009375','askQty':'22.00000000'},{'symbol':'ENGETH','bidPrice':'0.00274810','bidQty':'87.00000000','askPrice':'0.00277060','askQty':'8.00000000'},{'symbol':'DNTBTC','bidPrice':'0.00000360','bidQty':'45678.00000000','askPrice':'0.00000362','askQty':'55185.00000000'},{'symbol':'ZECBTC','bidPrice':'0.01886400','bidQty':'8.84400000','askPrice':'0.01890000','askQty':'11.96400000'},{'symbol':'ZECETH','bidPrice':'0.55623000','bidQty':'0.04200000','askPrice':'0.55736000','askQty':'0.21200000'},{'symbol':'BNTBTC','bidPrice':'0.00022310','bidQty':'29.00000000','askPrice':'0.00022380','askQty':'74.00000000'},{'symbol':'ASTBTC','bidPrice':'0.00001235','bidQty':'82.00000000','askPrice':'0.00001239','askQty':'780.00000000'},{'symbol':'ASTETH','bidPrice':'0.00036330','bidQty':'82.00000000','askPrice':'0.00036500','askQty':'33.00000000'},{'symbol':'DASHBTC','bidPrice':'0.02745600','bidQty':'11.29000000','askPrice':'0.02748100','askQty':'3.60000000'},{'symbol':'DASHETH','bidPrice':'0.80897000','bidQty':'3.06800000','askPrice':'0.81210000','askQty':'0.01400000'},{'symbol':'OAXBTC','bidPrice':'0.00002719','bidQty':'423.00000000','askPrice':'0.00002720','askQty':'2996.00000000'},{'symbol':'ICNBTC','bidPrice':'0.00005770','bidQty':'117.00000000','askPrice':'0.00005800','askQty':'517.00000000'},{'symbol':'BTGBTC','bidPrice':'0.00387500','bidQty':'2.40000000','askPrice':'0.00388200','askQty':'18.86000000'},{'symbol':'BTGETH','bidPrice':'0.11424900','bidQty':'0.29000000','askPrice':'0.11470400','askQty':'3.00000000'},{'symbol':'EVXBTC','bidPrice':'0.00006555','bidQty':'423.00000000','askPrice':'0.00006566','askQty':'547.00000000'},{'symbol':'EVXETH','bidPrice':'0.00192950','bidQty':'423.00000000','askPrice':'0.00194560','askQty':'42.00000000'},{'symbol':'REQBTC','bidPrice':'0.00000693','bidQty':'11955.00000000','askPrice':'0.00000698','askQty':'9463.00000000'},{'symbol':'REQETH','bidPrice':'0.00020401','bidQty':'11301.00000000','askPrice':'0.00020552','askQty':'2451.00000000'},{'symbol':'VIBBTC','bidPrice':'0.00000625','bidQty':'750.00000000','askPrice':'0.00000628','askQty':'35685.00000000'},{'symbol':'VIBETH','bidPrice':'0.00018440','bidQty':'163.00000000','askPrice':'0.00018575','askQty':'608.00000000'},{'symbol':'HSRETH','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'TRXBTC','bidPrice':'0.00000332','bidQty':'2429723.00000000','askPrice':'0.00000333','askQty':'3017104.00000000'},{'symbol':'TRXETH','bidPrice':'0.00009807','bidQty':'39749.00000000','askPrice':'0.00009814','askQty':'54303.00000000'},{'symbol':'POWRBTC','bidPrice':'0.00002433','bidQty':'141.00000000','askPrice':'0.00002448','askQty':'356.00000000'},{'symbol':'POWRETH','bidPrice':'0.00071821','bidQty':'96.00000000','askPrice':'0.00072295','askQty':'417.00000000'},{'symbol':'ARKBTC','bidPrice':'0.00010620','bidQty':'94.16000000','askPrice':'0.00010630','askQty':'9301.85000000'},{'symbol':'ARKETH','bidPrice':'0.00312800','bidQty':'96.00000000','askPrice':'0.00314100','askQty':'96.00000000'},{'symbol':'YOYOETH','bidPrice':'0.00012748','bidQty':'18124.00000000','askPrice':'0.00012801','askQty':'27760.00000000'},{'symbol':'XRPBTC','bidPrice':'0.00008006','bidQty':'772.00000000','askPrice':'0.00008009','askQty':'174.00000000'},{'symbol':'XRPETH','bidPrice':'0.00236024','bidQty':'3781.00000000','askPrice':'0.00236702','askQty':'476.00000000'},{'symbol':'MODBTC','bidPrice':'0.00010040','bidQty':'1067.37000000','askPrice':'0.00010110','askQty':'346.40000000'},{'symbol':'MODETH','bidPrice':'0.00296200','bidQty':'43.26000000','askPrice':'0.00298700','askQty':'101.00000000'},{'symbol':'ENJBTC','bidPrice':'0.00000813','bidQty':'162.00000000','askPrice':'0.00000814','askQty':'14969.00000000'},{'symbol':'ENJETH','bidPrice':'0.00023954','bidQty':'3916.00000000','askPrice':'0.00024065','askQty':'2146.00000000'},{'symbol':'STORJBTC','bidPrice':'0.00003922','bidQty':'475.00000000','askPrice':'0.00003938','askQty':'59.00000000'},{'symbol':'STORJETH','bidPrice':'0.00115460','bidQty':'1587.00000000','askPrice':'0.00115730','askQty':'25.00000000'},{'symbol':'BNBUSDT','bidPrice':'10.18380000','bidQty':'282.48000000','askPrice':'10.19600000','askQty':'0.02000000'},{'symbol':'VENBNB','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'YOYOBNB','bidPrice':'0.00273200','bidQty':'20277.00000000','askPrice':'0.00275000','askQty':'2502.00000000'},{'symbol':'POWRBNB','bidPrice':'0.01539000','bidQty':'1607.20000000','askPrice':'0.01556000','askQty':'2683.80000000'},{'symbol':'VENBTC','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'VENETH','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'KMDBTC','bidPrice':'0.00016860','bidQty':'30.07000000','askPrice':'0.00016910','askQty':'527.21000000'},{'symbol':'KMDETH','bidPrice':'0.00496000','bidQty':'56.20000000','askPrice':'0.00500200','askQty':'74.14000000'},{'symbol':'NULSBNB','bidPrice':'0.11328000','bidQty':'156.00000000','askPrice':'0.11447000','askQty':'319.00000000'},{'symbol':'RCNBTC','bidPrice':'0.00000405','bidQty':'2663.00000000','askPrice':'0.00000406','askQty':'46123.00000000'},{'symbol':'RCNETH','bidPrice':'0.00011922','bidQty':'2663.00000000','askPrice':'0.00011959','askQty':'84.00000000'},{'symbol':'RCNBNB','bidPrice':'0.00254100','bidQty':'3302.00000000','askPrice':'0.00259700','askQty':'20862.00000000'},{'symbol':'NULSBTC','bidPrice':'0.00017921','bidQty':'75.00000000','askPrice':'0.00017922','askQty':'12.00000000'},{'symbol':'NULSETH','bidPrice':'0.00527256','bidQty':'391.00000000','askPrice':'0.00529101','askQty':'566.00000000'},{'symbol':'RDNBTC','bidPrice':'0.00007121','bidQty':'112.00000000','askPrice':'0.00007156','askQty':'464.00000000'},{'symbol':'RDNETH','bidPrice':'0.00209380','bidQty':'492.00000000','askPrice':'0.00211450','askQty':'10.00000000'},{'symbol':'RDNBNB','bidPrice':'0.04467000','bidQty':'139.80000000','askPrice':'0.04572000','askQty':'139.80000000'},{'symbol':'XMRBTC','bidPrice':'0.01763300','bidQty':'0.44400000','askPrice':'0.01768300','askQty':'0.35400000'},{'symbol':'XMRETH','bidPrice':'0.52050000','bidQty':'0.13400000','askPrice':'0.52265000','askQty':'0.21800000'},{'symbol':'DLTBNB','bidPrice':'0.00641000','bidQty':'23995.30000000','askPrice':'0.00653000','askQty':'168.80000000'},{'symbol':'WTCBNB','bidPrice':'0.27420000','bidQty':'158.09000000','askPrice':'0.27790000','askQty':'5.39000000'},{'symbol':'DLTBTC','bidPrice':'0.00001022','bidQty':'206.00000000','askPrice':'0.00001023','askQty':'578.00000000'},{'symbol':'DLTETH','bidPrice':'0.00030146','bidQty':'1260.00000000','askPrice':'0.00030508','askQty':'605.00000000'},{'symbol':'AMBBTC','bidPrice':'0.00002128','bidQty':'792.00000000','askPrice':'0.00002129','askQty':'4636.00000000'},{'symbol':'AMBETH','bidPrice':'0.00062323','bidQty':'792.00000000','askPrice':'0.00063158','askQty':'435.00000000'},{'symbol':'AMBBNB','bidPrice':'0.01338000','bidQty':'353.80000000','askPrice':'0.01356000','askQty':'277.80000000'},{'symbol':'BCCETH','bidPrice':'2.35927000','bidQty':'0.38200000','askPrice':'2.36841000','askQty':'0.04500000'},{'symbol':'BCCUSDT','bidPrice':'517.19000000','bidQty':'0.03863000','askPrice':'517.74000000','askQty':'0.12479000'},{'symbol':'BCCBNB','bidPrice':'50.68000000','bidQty':'4.76559000','askPrice':'50.87000000','askQty':'0.53500000'},{'symbol':'BATBTC','bidPrice':'0.00002597','bidQty':'809.00000000','askPrice':'0.00002600','askQty':'1526.00000000'},{'symbol':'BATETH','bidPrice':'0.00076576','bidQty':'376.00000000','askPrice':'0.00076763','askQty':'1229.00000000'},{'symbol':'BATBNB','bidPrice':'0.01640000','bidQty':'1686.10000000','askPrice':'0.01652000','askQty':'314.80000000'},{'symbol':'BCPTBTC','bidPrice':'0.00001328','bidQty':'20575.00000000','askPrice':'0.00001329','askQty':'6415.00000000'},{'symbol':'BCPTETH','bidPrice':'0.00039132','bidQty':'765.00000000','askPrice':'0.00039238','askQty':'765.00000000'},{'symbol':'BCPTBNB','bidPrice':'0.00836000','bidQty':'692.90000000','askPrice':'0.00850000','askQty':'692.90000000'},{'symbol':'ARNBTC','bidPrice':'0.00005714','bidQty':'20.00000000','askPrice':'0.00005731','askQty':'1626.00000000'},{'symbol':'ARNETH','bidPrice':'0.00168612','bidQty':'69.00000000','askPrice':'0.00169529','askQty':'34.00000000'},{'symbol':'GVTBTC','bidPrice':'0.00154010','bidQty':'3.63000000','askPrice':'0.00154430','askQty':'3.06000000'},{'symbol':'GVTETH','bidPrice':'0.04538600','bidQty':'9.52000000','askPrice':'0.04549900','askQty':'4.97000000'},{'symbol':'CDTBTC','bidPrice':'0.00000234','bidQty':'667486.00000000','askPrice':'0.00000235','askQty':'104330.00000000'},{'symbol':'CDTETH','bidPrice':'0.00006893','bidQty':'6087.00000000','askPrice':'0.00006915','askQty':'500.00000000'},{'symbol':'GXSBTC','bidPrice':'0.00019920','bidQty':'37.42000000','askPrice':'0.00020080','askQty':'46.69000000'},{'symbol':'GXSETH','bidPrice':'0.00588400','bidQty':'47.00000000','askPrice':'0.00591700','askQty':'1.08000000'},{'symbol':'NEOUSDT','bidPrice':'17.92100000','bidQty':'100.00000000','askPrice':'17.93600000','askQty':'19.42200000'},{'symbol':'NEOBNB','bidPrice':'1.75400000','bidQty':'6.83400000','askPrice':'1.76000000','askQty':'7.49000000'},{'symbol':'POEBTC','bidPrice':'0.00000153','bidQty':'48095.00000000','askPrice':'0.00000154','askQty':'136131.00000000'},{'symbol':'POEETH','bidPrice':'0.00004513','bidQty':'19324.00000000','askPrice':'0.00004533','askQty':'8000.00000000'},{'symbol':'QSPBTC','bidPrice':'0.00000567','bidQty':'444.00000000','askPrice':'0.00000569','askQty':'9445.00000000'},{'symbol':'QSPETH','bidPrice':'0.00016707','bidQty':'686.00000000','askPrice':'0.00016770','askQty':'276.00000000'},{'symbol':'QSPBNB','bidPrice':'0.00351600','bidQty':'1420.00000000','askPrice':'0.00360600','askQty':'1348.00000000'},{'symbol':'BTSBTC','bidPrice':'0.00001674','bidQty':'2089.00000000','askPrice':'0.00001680','askQty':'5765.00000000'},{'symbol':'BTSETH','bidPrice':'0.00049405','bidQty':'1640.00000000','askPrice':'0.00049665','askQty':'606.00000000'},{'symbol':'BTSBNB','bidPrice':'0.01057000','bidQty':'154.80000000','askPrice':'0.01075000','askQty':'3630.40000000'},{'symbol':'XZCBTC','bidPrice':'0.00149200','bidQty':'1.57000000','askPrice':'0.00149800','askQty':'24.83000000'},{'symbol':'XZCETH','bidPrice':'0.04392900','bidQty':'1.57000000','askPrice':'0.04416300','askQty':'4.73000000'},{'symbol':'XZCBNB','bidPrice':'0.93900000','bidQty':'2.10900000','askPrice':'0.95900000','askQty':'3.00000000'},{'symbol':'LSKBTC','bidPrice':'0.00049210','bidQty':'10.68000000','askPrice':'0.00049390','askQty':'4.45000000'},{'symbol':'LSKETH','bidPrice':'0.01450400','bidQty':'17.38000000','askPrice':'0.01455900','askQty':'0.69000000'},{'symbol':'LSKBNB','bidPrice':'0.30960000','bidQty':'16.13000000','askPrice':'0.31250000','askQty':'13.10000000'},{'symbol':'TNTBTC','bidPrice':'0.00000429','bidQty':'6685.00000000','askPrice':'0.00000432','askQty':'4876.00000000'},{'symbol':'TNTETH','bidPrice':'0.00012637','bidQty':'8741.00000000','askPrice':'0.00012713','askQty':'160.00000000'},{'symbol':'FUELBTC','bidPrice':'0.00000270','bidQty':'296843.00000000','askPrice':'0.00000272','askQty':'23482.00000000'},{'symbol':'FUELETH','bidPrice':'0.00007975','bidQty':'378.00000000','askPrice':'0.00008000','askQty':'1873.00000000'},{'symbol':'MANABTC','bidPrice':'0.00001149','bidQty':'57719.00000000','askPrice':'0.00001150','askQty':'2804.00000000'},{'symbol':'MANAETH','bidPrice':'0.00033902','bidQty':'25.00000000','askPrice':'0.00034082','askQty':'1159.00000000'},{'symbol':'BCDBTC','bidPrice':'0.00029700','bidQty':'9415.94900000','askPrice':'0.00029900','askQty':'6507.05500000'},{'symbol':'BCDETH','bidPrice':'0.00877000','bidQty':'31.75400000','askPrice':'0.00880000','askQty':'626.30800000'},{'symbol':'DGDBTC','bidPrice':'0.00570900','bidQty':'0.33200000','askPrice':'0.00574100','askQty':'0.82400000'},{'symbol':'DGDETH','bidPrice':'0.16841000','bidQty':'1.29100000','askPrice':'0.16970000','askQty':'0.06000000'},{'symbol':'IOTABNB','bidPrice':'0.05395000','bidQty':'2993.70000000','askPrice':'0.05425000','askQty':'503.20000000'},{'symbol':'ADXBTC','bidPrice':'0.00002974','bidQty':'1826.00000000','askPrice':'0.00002976','askQty':'1330.00000000'},{'symbol':'ADXETH','bidPrice':'0.00087490','bidQty':'2475.00000000','askPrice':'0.00087970','askQty':'175.00000000'},{'symbol':'ADXBNB','bidPrice':'0.01872000','bidQty':'2372.70000000','askPrice':'0.01896000','askQty':'3785.40000000'},{'symbol':'ADABTC','bidPrice':'0.00001237','bidQty':'23082.00000000','askPrice':'0.00001238','askQty':'20735.00000000'},{'symbol':'ADAETH','bidPrice':'0.00036524','bidQty':'308.00000000','askPrice':'0.00036640','askQty':'4039.00000000'},{'symbol':'PPTBTC','bidPrice':'0.00048070','bidQty':'269.63000000','askPrice':'0.00048110','askQty':'195.01000000'},{'symbol':'PPTETH','bidPrice':'0.01416600','bidQty':'21.65000000','askPrice':'0.01425200','askQty':'101.93000000'},{'symbol':'CMTBTC','bidPrice':'0.00001553','bidQty':'788.00000000','askPrice':'0.00001559','askQty':'6832.00000000'},{'symbol':'CMTETH','bidPrice':'0.00045986','bidQty':'22.00000000','askPrice':'0.00046000','askQty':'1018.00000000'},{'symbol':'CMTBNB','bidPrice':'0.00980000','bidQty':'406.90000000','askPrice':'0.00995000','askQty':'6098.70000000'},{'symbol':'XLMBTC','bidPrice':'0.00003728','bidQty':'540.00000000','askPrice':'0.00003729','askQty':'28.00000000'},{'symbol':'XLMETH','bidPrice':'0.00109857','bidQty':'535.00000000','askPrice':'0.00110099','askQty':'891.00000000'},{'symbol':'XLMBNB','bidPrice':'0.02360000','bidQty':'100.00000000','askPrice':'0.02365000','askQty':'535.00000000'},{'symbol':'CNDBTC','bidPrice':'0.00000329','bidQty':'187921.00000000','askPrice':'0.00000331','askQty':'6128.00000000'},{'symbol':'CNDETH','bidPrice':'0.00009692','bidQty':'7291.00000000','askPrice':'0.00009798','askQty':'14890.00000000'},{'symbol':'CNDBNB','bidPrice':'0.00206900','bidQty':'3560.00000000','askPrice':'0.00212800','askQty':'15185.00000000'},{'symbol':'LENDBTC','bidPrice':'0.00000220','bidQty':'228869.00000000','askPrice':'0.00000221','askQty':'4134.00000000'},{'symbol':'LENDETH','bidPrice':'0.00006483','bidQty':'1770.00000000','askPrice':'0.00006509','askQty':'1802.00000000'},{'symbol':'WABIBTC','bidPrice':'0.00003088','bidQty':'411.00000000','askPrice':'0.00003089','askQty':'2358.00000000'},{'symbol':'WABIETH','bidPrice':'0.00091018','bidQty':'329.00000000','askPrice':'0.00091364','askQty':'2527.00000000'},{'symbol':'WABIBNB','bidPrice':'0.01946000','bidQty':'323.00000000','askPrice':'0.01978000','askQty':'253.70000000'},{'symbol':'LTCETH','bidPrice':'0.26326000','bidQty':'0.22100000','askPrice':'0.26390000','askQty':'1.06900000'},{'symbol':'LTCUSDT','bidPrice':'57.69000000','bidQty':'0.00026000','askPrice':'57.72000000','askQty':'13.80499000'},{'symbol':'LTCBNB','bidPrice':'5.65000000','bidQty':'0.73178000','askPrice':'5.67000000','askQty':'0.68589000'},{'symbol':'TNBBTC','bidPrice':'0.00000140','bidQty':'133735.00000000','askPrice':'0.00000141','askQty':'147504.00000000'},{'symbol':'TNBETH','bidPrice':'0.00004123','bidQty':'243.00000000','askPrice':'0.00004137','askQty':'456.00000000'},{'symbol':'WAVESBTC','bidPrice':'0.00032150','bidQty':'31.17000000','askPrice':'0.00032200','askQty':'409.76000000'},{'symbol':'WAVESETH','bidPrice':'0.00946400','bidQty':'31.17000000','askPrice':'0.00953100','askQty':'341.89000000'},{'symbol':'WAVESBNB','bidPrice':'0.20310000','bidQty':'31.17000000','askPrice':'0.20570000','askQty':'22.23000000'},{'symbol':'GTOBTC','bidPrice':'0.00001000','bidQty':'13076.00000000','askPrice':'0.00001005','askQty':'37737.00000000'},{'symbol':'GTOETH','bidPrice':'0.00029506','bidQty':'3264.00000000','askPrice':'0.00029668','askQty':'1014.00000000'},{'symbol':'GTOBNB','bidPrice':'0.00633000','bidQty':'4332.30000000','askPrice':'0.00637000','askQty':'200.30000000'},{'symbol':'ICXBTC','bidPrice':'0.00010220','bidQty':'175.11000000','askPrice':'0.00010230','askQty':'294.16000000'},{'symbol':'ICXETH','bidPrice':'0.00301700','bidQty':'50.00000000','askPrice':'0.00302600','askQty':'485.35000000'},{'symbol':'ICXBNB','bidPrice':'0.06461000','bidQty':'120.00000000','askPrice':'0.06507000','askQty':'69.10000000'},{'symbol':'OSTBTC','bidPrice':'0.00000668','bidQty':'36693.00000000','askPrice':'0.00000670','askQty':'22266.00000000'},{'symbol':'OSTETH','bidPrice':'0.00019699','bidQty':'1520.00000000','askPrice':'0.00019775','askQty':'570.00000000'},{'symbol':'OSTBNB','bidPrice':'0.00419800','bidQty':'1189.00000000','askPrice':'0.00428200','askQty':'1149.00000000'},{'symbol':'ELFBTC','bidPrice':'0.00005371','bidQty':'628.00000000','askPrice':'0.00005377','askQty':'172.00000000'},{'symbol':'ELFETH','bidPrice':'0.00158316','bidQty':'189.00000000','askPrice':'0.00159229','askQty':'75.00000000'},{'symbol':'AIONBTC','bidPrice':'0.00006160','bidQty':'2170.36000000','askPrice':'0.00006190','askQty':'1275.63000000'},{'symbol':'AIONETH','bidPrice':'0.00182600','bidQty':'1202.51000000','askPrice':'0.00183600','askQty':'1057.21000000'},{'symbol':'AIONBNB','bidPrice':'0.03897000','bidQty':'100.00000000','askPrice':'0.03938000','askQty':'57.30000000'},{'symbol':'NEBLBTC','bidPrice':'0.00028800','bidQty':'269.78000000','askPrice':'0.00028860','askQty':'63.48000000'},{'symbol':'NEBLETH','bidPrice':'0.00848400','bidQty':'50.97000000','askPrice':'0.00854300','askQty':'197.43000000'},{'symbol':'NEBLBNB','bidPrice':'0.18104000','bidQty':'26.90000000','askPrice':'0.18410000','askQty':'30.20000000'},{'symbol':'BRDBTC','bidPrice':'0.00005209','bidQty':'479.00000000','askPrice':'0.00005239','askQty':'327.00000000'},{'symbol':'BRDETH','bidPrice':'0.00153530','bidQty':'769.00000000','askPrice':'0.00154820','askQty':'12.00000000'},{'symbol':'BRDBNB','bidPrice':'0.03283000','bidQty':'1133.50000000','askPrice':'0.03345000','askQty':'126.00000000'},{'symbol':'MCOBNB','bidPrice':'0.43222000','bidQty':'88.70000000','askPrice':'0.44634000','askQty':'99.60000000'},{'symbol':'EDOBTC','bidPrice':'0.00013510','bidQty':'37.67000000','askPrice':'0.00013550','askQty':'9.21000000'},{'symbol':'EDOETH','bidPrice':'0.00397400','bidQty':'73.67000000','askPrice':'0.00399400','askQty':'7.86000000'},{'symbol':'WINGSBTC','bidPrice':'0.00002129','bidQty':'4888.00000000','askPrice':'0.00002137','askQty':'86.00000000'},{'symbol':'WINGSETH','bidPrice':'0.00062560','bidQty':'3658.00000000','askPrice':'0.00063170','askQty':'207.00000000'},{'symbol':'NAVBTC','bidPrice':'0.00004900','bidQty':'253.32000000','askPrice':'0.00004930','askQty':'2285.05000000'},{'symbol':'NAVETH','bidPrice':'0.00144400','bidQty':'548.76000000','askPrice':'0.00145400','askQty':'792.09000000'},{'symbol':'NAVBNB','bidPrice':'0.03078000','bidQty':'1501.60000000','askPrice':'0.03139000','askQty':'1276.20000000'},{'symbol':'LUNBTC','bidPrice':'0.00051200','bidQty':'40.37000000','askPrice':'0.00051270','askQty':'161.70000000'},{'symbol':'LUNETH','bidPrice':'0.01505700','bidQty':'154.90000000','askPrice':'0.01511400','askQty':'0.67000000'},{'symbol':'TRIGBTC','bidPrice':'0.00003860','bidQty':'1293.34000000','askPrice':'0.00003870','askQty':'1818.46000000'},{'symbol':'TRIGETH','bidPrice':'0.00113700','bidQty':'263.00000000','askPrice':'0.00114100','askQty':'1692.06000000'},{'symbol':'TRIGBNB','bidPrice':'0.02432000','bidQty':'82.30000000','askPrice':'0.02480000','askQty':'1200.50000000'},{'symbol':'APPCBTC','bidPrice':'0.00001773','bidQty':'436.00000000','askPrice':'0.00001779','askQty':'833.00000000'},{'symbol':'APPCETH','bidPrice':'0.00052330','bidQty':'152.00000000','askPrice':'0.00052510','askQty':'172.00000000'},{'symbol':'APPCBNB','bidPrice':'0.01108000','bidQty':'450.80000000','askPrice':'0.01130000','askQty':'790.80000000'},{'symbol':'VIBEBTC','bidPrice':'0.00001120','bidQty':'6940.00000000','askPrice':'0.00001121','askQty':'460.00000000'},{'symbol':'VIBEETH','bidPrice':'0.00032980','bidQty':'783.00000000','askPrice':'0.00033140','askQty':'67.00000000'},{'symbol':'RLCBTC','bidPrice':'0.00006120','bidQty':'365.47000000','askPrice':'0.00006160','askQty':'237.54000000'},{'symbol':'RLCETH','bidPrice':'0.00180600','bidQty':'11.39000000','askPrice':'0.00182100','askQty':'41.52000000'},{'symbol':'RLCBNB','bidPrice':'0.03768000','bidQty':'29.80000000','askPrice':'0.03904000','askQty':'93.70000000'},{'symbol':'INSBTC','bidPrice':'0.00005520','bidQty':'156.70000000','askPrice':'0.00005540','askQty':'1174.00000000'},{'symbol':'INSETH','bidPrice':'0.00162800','bidQty':'277.41000000','askPrice':'0.00163500','askQty':'2061.01000000'},{'symbol':'PIVXBTC','bidPrice':'0.00016210','bidQty':'83.82000000','askPrice':'0.00016310','askQty':'126.34000000'},{'symbol':'PIVXETH','bidPrice':'0.00477800','bidQty':'29.41000000','askPrice':'0.00481300','askQty':'4.24000000'},{'symbol':'PIVXBNB','bidPrice':'0.10224000','bidQty':'68.10000000','askPrice':'0.10489000','askQty':'334.10000000'},{'symbol':'IOSTBTC','bidPrice':'0.00000195','bidQty':'978592.00000000','askPrice':'0.00000196','askQty':'42538.00000000'},{'symbol':'IOSTETH','bidPrice':'0.00005778','bidQty':'174.00000000','askPrice':'0.00005803','askQty':'66956.00000000'},{'symbol':'CHATBTC','bidPrice':'0.00000324','bidQty':'133267.00000000','askPrice':'0.00000325','askQty':'62114.00000000'},{'symbol':'CHATETH','bidPrice':'0.00009549','bidQty':'3135.00000000','askPrice':'0.00009586','askQty':'909.00000000'},{'symbol':'STEEMBTC','bidPrice':'0.00013030','bidQty':'75.51000000','askPrice':'0.00013080','askQty':'0.02000000'},{'symbol':'STEEMETH','bidPrice':'0.00383600','bidQty':'70.93000000','askPrice':'0.00385200','askQty':'57.47000000'},{'symbol':'STEEMBNB','bidPrice':'0.08232000','bidQty':'49.10000000','askPrice':'0.08388000','askQty':'57.40000000'},{'symbol':'NANOBTC','bidPrice':'0.00032750','bidQty':'501.89000000','askPrice':'0.00032800','askQty':'1617.01000000'},{'symbol':'NANOETH','bidPrice':'0.00965400','bidQty':'4.50000000','askPrice':'0.00970400','askQty':'10.53000000'},{'symbol':'NANOBNB','bidPrice':'0.20700000','bidQty':'39.46000000','askPrice':'0.20850000','askQty':'30.33000000'},{'symbol':'VIABTC','bidPrice':'0.00009530','bidQty':'1335.48000000','askPrice':'0.00009580','askQty':'37.74000000'},{'symbol':'VIAETH','bidPrice':'0.00280700','bidQty':'221.91000000','askPrice':'0.00283500','askQty':'64.00000000'},{'symbol':'VIABNB','bidPrice':'0.05987000','bidQty':'83.40000000','askPrice':'0.06112000','askQty':'108.00000000'},{'symbol':'BLZBTC','bidPrice':'0.00001921','bidQty':'1161.00000000','askPrice':'0.00001925','askQty':'800.00000000'},{'symbol':'BLZETH','bidPrice':'0.00056556','bidQty':'1161.00000000','askPrice':'0.00056799','askQty':'18.00000000'},{'symbol':'BLZBNB','bidPrice':'0.01205000','bidQty':'913.20000000','askPrice':'0.01229000','askQty':'3162.80000000'},{'symbol':'AEBTC','bidPrice':'0.00015960','bidQty':'624.61000000','askPrice':'0.00015990','askQty':'155.83000000'},{'symbol':'AEETH','bidPrice':'0.00470300','bidQty':'115.99000000','askPrice':'0.00472800','askQty':'23.48000000'},{'symbol':'AEBNB','bidPrice':'0.10066000','bidQty':'14.90000000','askPrice':'0.10210000','askQty':'382.10000000'},{'symbol':'RPXBTC','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'RPXETH','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'RPXBNB','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'NCASHBTC','bidPrice':'0.00000078','bidQty':'10916861.00000000','askPrice':'0.00000079','askQty':'10412121.00000000'},{'symbol':'NCASHETH','bidPrice':'0.00002314','bidQty':'2594.00000000','askPrice':'0.00002327','askQty':'75425.00000000'},{'symbol':'NCASHBNB','bidPrice':'0.00049500','bidQty':'15545.00000000','askPrice':'0.00050200','askQty':'112836.00000000'},{'symbol':'POABTC','bidPrice':'0.00001176','bidQty':'2328.00000000','askPrice':'0.00001179','askQty':'468.00000000'},{'symbol':'POAETH','bidPrice':'0.00034771','bidQty':'478.00000000','askPrice':'0.00034906','askQty':'263.00000000'},{'symbol':'POABNB','bidPrice':'0.00744000','bidQty':'171.10000000','askPrice':'0.00755000','askQty':'397.80000000'},{'symbol':'ZILBTC','bidPrice':'0.00000565','bidQty':'315844.00000000','askPrice':'0.00000566','askQty':'60535.00000000'},{'symbol':'ZILETH','bidPrice':'0.00016702','bidQty':'949.00000000','askPrice':'0.00016722','askQty':'3571.00000000'},{'symbol':'ZILBNB','bidPrice':'0.00357300','bidQty':'29153.00000000','askPrice':'0.00360300','askQty':'1161.00000000'},{'symbol':'ONTBTC','bidPrice':'0.00029510','bidQty':'50.69000000','askPrice':'0.00029570','askQty':'42.80000000'},{'symbol':'ONTETH','bidPrice':'0.00871800','bidQty':'43.29000000','askPrice':'0.00873200','askQty':'3.50000000'},{'symbol':'ONTBNB','bidPrice':'0.18671000','bidQty':'7.70000000','askPrice':'0.18762000','askQty':'28.50000000'},{'symbol':'STORMBTC','bidPrice':'0.00000121','bidQty':'266201.00000000','askPrice':'0.00000122','askQty':'902627.00000000'},{'symbol':'STORMETH','bidPrice':'0.00003571','bidQty':'43460.00000000','askPrice':'0.00003599','askQty':'7387.00000000'},{'symbol':'STORMBNB','bidPrice':'0.00076600','bidQty':'4636.00000000','askPrice':'0.00077900','askQty':'3495.00000000'},{'symbol':'QTUMBNB','bidPrice':'0.36369000','bidQty':'568.60000000','askPrice':'0.36761000','askQty':'3.20000000'},{'symbol':'QTUMUSDT','bidPrice':'3.72500000','bidQty':'13.42200000','askPrice':'3.73000000','askQty':'16.72900000'},{'symbol':'XEMBTC','bidPrice':'0.00001576','bidQty':'3192.00000000','askPrice':'0.00001579','askQty':'1600.00000000'},{'symbol':'XEMETH','bidPrice':'0.00046596','bidQty':'40.00000000','askPrice':'0.00046856','askQty':'243.00000000'},{'symbol':'XEMBNB','bidPrice':'0.00998000','bidQty':'721.20000000','askPrice':'0.01007000','askQty':'437.00000000'},{'symbol':'WANBTC','bidPrice':'0.00014790','bidQty':'100.00000000','askPrice':'0.00014820','askQty':'169.10000000'},{'symbol':'WANETH','bidPrice':'0.00435900','bidQty':'877.67000000','askPrice':'0.00438500','askQty':'471.30000000'},{'symbol':'WANBNB','bidPrice':'0.09294000','bidQty':'53.70000000','askPrice':'0.09517000','askQty':'393.00000000'},{'symbol':'WPRBTC','bidPrice':'0.00000428','bidQty':'28000.00000000','askPrice':'0.00000429','askQty':'19857.00000000'},{'symbol':'WPRETH','bidPrice':'0.00012604','bidQty':'18292.00000000','askPrice':'0.00012689','askQty':'2376.00000000'},{'symbol':'QLCBTC','bidPrice':'0.00000795','bidQty':'334.00000000','askPrice':'0.00000800','askQty':'27000.00000000'},{'symbol':'QLCETH','bidPrice':'0.00023416','bidQty':'1675.00000000','askPrice':'0.00023723','askQty':'749.00000000'},{'symbol':'SYSBTC','bidPrice':'0.00001424','bidQty':'11021.00000000','askPrice':'0.00001427','askQty':'11449.00000000'},{'symbol':'SYSETH','bidPrice':'0.00042078','bidQty':'36.00000000','askPrice':'0.00042359','askQty':'8164.00000000'},{'symbol':'SYSBNB','bidPrice':'0.00899000','bidQty':'3711.40000000','askPrice':'0.00911000','askQty':'881.10000000'},{'symbol':'QLCBNB','bidPrice':'0.00496500','bidQty':'1354.00000000','askPrice':'0.00509800','askQty':'8699.00000000'},{'symbol':'GRSBTC','bidPrice':'0.00008407','bidQty':'8.00000000','askPrice':'0.00008408','askQty':'876.00000000'},{'symbol':'GRSETH','bidPrice':'0.00246853','bidQty':'478.00000000','askPrice':'0.00249284','askQty':'829.00000000'},{'symbol':'ADAUSDT','bidPrice':'0.08001000','bidQty':'499.40000000','askPrice':'0.08008000','askQty':'19046.00000000'},{'symbol':'ADABNB','bidPrice':'0.00784000','bidQty':'16558.10000000','askPrice':'0.00786000','askQty':'150.00000000'},{'symbol':'CLOAKBTC','bidPrice':'0.00035850','bidQty':'203.34000000','askPrice':'0.00035990','askQty':'21.71000000'},{'symbol':'CLOAKETH','bidPrice':'0.01059300','bidQty':'62.39000000','askPrice':'0.01063400','askQty':'9.47000000'},{'symbol':'GNTBTC','bidPrice':'0.00002299','bidQty':'565.00000000','askPrice':'0.00002310','askQty':'5281.00000000'},{'symbol':'GNTETH','bidPrice':'0.00067799','bidQty':'2095.00000000','askPrice':'0.00068164','askQty':'7221.00000000'},{'symbol':'GNTBNB','bidPrice':'0.01451000','bidQty':'175.70000000','askPrice':'0.01478000','askQty':'2012.00000000'},{'symbol':'LOOMBTC','bidPrice':'0.00001555','bidQty':'1563.00000000','askPrice':'0.00001561','askQty':'299.00000000'},{'symbol':'LOOMETH','bidPrice':'0.00046030','bidQty':'102.00000000','askPrice':'0.00046137','askQty':'109.00000000'},{'symbol':'LOOMBNB','bidPrice':'0.00983000','bidQty':'5015.50000000','askPrice':'0.00993000','askQty':'5860.40000000'},{'symbol':'XRPUSDT','bidPrice':'0.51754000','bidQty':'1179.80000000','askPrice':'0.51788000','askQty':'24646.40000000'},{'symbol':'BCNBTC','bidPrice':'0.00000034','bidQty':'62360543.00000000','askPrice':'0.00000035','askQty':'32582026.00000000'},{'symbol':'BCNETH','bidPrice':'0.00001022','bidQty':'92081.00000000','askPrice':'0.00001028','askQty':'1005.00000000'},{'symbol':'BCNBNB','bidPrice':'0.00022000','bidQty':'38222.00000000','askPrice':'0.00022200','askQty':'106742.00000000'},{'symbol':'REPBTC','bidPrice':'0.00196700','bidQty':'25.63500000','askPrice':'0.00197300','askQty':'89.00700000'},{'symbol':'REPETH','bidPrice':'0.05802000','bidQty':'1.04200000','askPrice':'0.05816000','askQty':'0.00100000'},{'symbol':'REPBNB','bidPrice':'1.24200000','bidQty':'3.81100000','askPrice':'1.26000000','askQty':'3.81100000'},{'symbol':'TUSDBTC','bidPrice':'0.00015635','bidQty':'74.00000000','askPrice':'0.00015640','askQty':'4815.00000000'},{'symbol':'TUSDETH','bidPrice':'0.00461027','bidQty':'24.00000000','askPrice':'0.00462495','askQty':'102.00000000'},{'symbol':'TUSDBNB','bidPrice':'0.09864000','bidQty':'3291.10000000','askPrice':'0.09981000','askQty':'16.90000000'},{'symbol':'ZENBTC','bidPrice':'0.00251700','bidQty':'0.46000000','askPrice':'0.00252500','askQty':'1.07500000'},{'symbol':'ZENETH','bidPrice':'0.07457000','bidQty':'0.13500000','askPrice':'0.07474000','askQty':'50.25300000'},{'symbol':'ZENBNB','bidPrice':'1.59400000','bidQty':'0.00900000','askPrice':'1.61400000','askQty':'25.63600000'},{'symbol':'SKYBTC','bidPrice':'0.00054000','bidQty':'16.01900000','askPrice':'0.00054300','askQty':'1.96400000'},{'symbol':'SKYETH','bidPrice':'0.01589000','bidQty':'16.01900000','askPrice':'0.01601000','askQty':'171.26200000'},{'symbol':'SKYBNB','bidPrice':'0.34100000','bidQty':'5.82200000','askPrice':'0.34700000','askQty':'20.00000000'},{'symbol':'EOSUSDT','bidPrice':'5.50320000','bidQty':'50.00000000','askPrice':'5.50840000','askQty':'18.11000000'},{'symbol':'EOSBNB','bidPrice':'0.53980000','bidQty':'164.45000000','askPrice':'0.54140000','askQty':'113.42000000'},{'symbol':'CVCBTC','bidPrice':'0.00001859','bidQty':'1614.00000000','askPrice':'0.00001868','askQty':'363.00000000'},{'symbol':'CVCETH','bidPrice':'0.00054700','bidQty':'1614.00000000','askPrice':'0.00055011','askQty':'34.00000000'},{'symbol':'CVCBNB','bidPrice':'0.01171000','bidQty':'468.30000000','askPrice':'0.01195000','askQty':'243.90000000'},{'symbol':'THETABTC','bidPrice':'0.00001368','bidQty':'17278.00000000','askPrice':'0.00001374','askQty':'518.00000000'},{'symbol':'THETAETH','bidPrice':'0.00040376','bidQty':'919.00000000','askPrice':'0.00040406','askQty':'28.00000000'},{'symbol':'THETABNB','bidPrice':'0.00862000','bidQty':'5729.40000000','askPrice':'0.00876000','askQty':'452.00000000'},{'symbol':'XRPBNB','bidPrice':'0.05062000','bidQty':'1712.00000000','askPrice':'0.05090000','askQty':'785.20000000'},{'symbol':'TUSDUSDT','bidPrice':'1.01070000','bidQty':'626.00000000','askPrice':'1.01140000','askQty':'1460.84000000'},{'symbol':'IOTAUSDT','bidPrice':'0.55180000','bidQty':'75.60000000','askPrice':'0.55230000','askQty':'97.86000000'},{'symbol':'XLMUSDT','bidPrice':'0.24083000','bidQty':'16.50000000','askPrice':'0.24105000','askQty':'16.50000000'},{'symbol':'IOTXBTC','bidPrice':'0.00000204','bidQty':'349517.00000000','askPrice':'0.00000205','askQty':'4062.00000000'},{'symbol':'IOTXETH','bidPrice':'0.00006022','bidQty':'7182.00000000','askPrice':'0.00006040','askQty':'1528.00000000'},{'symbol':'QKCBTC','bidPrice':'0.00000742','bidQty':'14299.00000000','askPrice':'0.00000746','askQty':'2435.00000000'},{'symbol':'QKCETH','bidPrice':'0.00021916','bidQty':'529.00000000','askPrice':'0.00022005','askQty':'3468.00000000'},{'symbol':'AGIBTC','bidPrice':'0.00000675','bidQty':'4064.00000000','askPrice':'0.00000676','askQty':'4933.00000000'},{'symbol':'AGIETH','bidPrice':'0.00019892','bidQty':'1506.00000000','askPrice':'0.00019958','askQty':'2307.00000000'},{'symbol':'AGIBNB','bidPrice':'0.00423000','bidQty':'4044.30000000','askPrice':'0.00431000','askQty':'253.30000000'},{'symbol':'NXSBTC','bidPrice':'0.00012000','bidQty':'533.98000000','askPrice':'0.00012010','askQty':'1595.08000000'},{'symbol':'NXSETH','bidPrice':'0.00353400','bidQty':'518.53000000','askPrice':'0.00355500','askQty':'498.20000000'},{'symbol':'NXSBNB','bidPrice':'0.07570000','bidQty':'100.00000000','askPrice':'0.07680000','askQty':'1139.34000000'},{'symbol':'ENJBNB','bidPrice':'0.00513200','bidQty':'555.00000000','askPrice':'0.00517400','askQty':'246.00000000'},{'symbol':'DATABTC','bidPrice':'0.00000571','bidQty':'9972.00000000','askPrice':'0.00000572','askQty':'3435.00000000'},{'symbol':'DATAETH','bidPrice':'0.00016863','bidQty':'120.00000000','askPrice':'0.00016870','askQty':'5124.00000000'},{'symbol':'ONTUSDT','bidPrice':'1.90900000','bidQty':'3.49200000','askPrice':'1.91000000','askQty':'40.74100000'},{'symbol':'TRXBNB','bidPrice':'0.00210600','bidQty':'58358.00000000','askPrice':'0.00211700','askQty':'152785.00000000'},{'symbol':'TRXUSDT','bidPrice':'0.02147000','bidQty':'351652.10000000','askPrice':'0.02150000','askQty':'21452.00000000'},{'symbol':'ETCUSDT','bidPrice':'10.93110000','bidQty':'5.00000000','askPrice':'10.94160000','askQty':'200.00000000'},{'symbol':'ETCBNB','bidPrice':'1.06950000','bidQty':'204.39000000','askPrice':'1.07350000','askQty':'0.01000000'},{'symbol':'ICXUSDT','bidPrice':'0.66100000','bidQty':'2639.05000000','askPrice':'0.66200000','askQty':'381.69000000'},{'symbol':'SCBTC','bidPrice':'0.00000102','bidQty':'4868127.00000000','askPrice':'0.00000103','askQty':'952003.00000000'},{'symbol':'SCETH','bidPrice':'0.00003019','bidQty':'3568.00000000','askPrice':'0.00003033','askQty':'18276.00000000'},{'symbol':'SCBNB','bidPrice':'0.00064400','bidQty':'84312.00000000','askPrice':'0.00065600','askQty':'9951.00000000'},{'symbol':'NPXSBTC','bidPrice':'0.00000023','bidQty':'757430542.00000000','askPrice':'0.00000024','askQty':'372137879.00000000'},{'symbol':'NPXSETH','bidPrice':'0.00000685','bidQty':'1809392.00000000','askPrice':'0.00000686','askQty':'194362.00000000'},{'symbol':'VENUSDT','bidPrice':'0.00000000','bidQty':'0.00000000','askPrice':'0.00000000','askQty':'0.00000000'},{'symbol':'KEYBTC','bidPrice':'0.00000089','bidQty':'4164775.00000000','askPrice':'0.00000090','askQty':'7090107.00000000'},{'symbol':'KEYETH','bidPrice':'0.00002640','bidQty':'187996.00000000','askPrice':'0.00002648','askQty':'5643.00000000'},{'symbol':'NASBTC','bidPrice':'0.00025350','bidQty':'33.06000000','askPrice':'0.00025380','askQty':'24.42000000'},{'symbol':'NASETH','bidPrice':'0.00746700','bidQty':'72.74000000','askPrice':'0.00750200','askQty':'2.63000000'},{'symbol':'NASBNB','bidPrice':'0.15903000','bidQty':'47.60000000','askPrice':'0.16315000','askQty':'91.10000000'},{'symbol':'MFTBTC','bidPrice':'0.00000113','bidQty':'1806250.00000000','askPrice':'0.00000114','askQty':'1685474.00000000'},{'symbol':'MFTETH','bidPrice':'0.00003350','bidQty':'301.00000000','askPrice':'0.00003362','askQty':'220397.00000000'},{'symbol':'MFTBNB','bidPrice':'0.00071300','bidQty':'67275.00000000','askPrice':'0.00072700','askQty':'82148.00000000'},{'symbol':'DENTBTC','bidPrice':'0.00000033','bidQty':'57465020.00000000','askPrice':'0.00000034','askQty':'51222101.00000000'},{'symbol':'DENTETH','bidPrice':'0.00000999','bidQty':'26853.00000000','askPrice':'0.00001005','askQty':'89332.00000000'},{'symbol':'ARDRBTC','bidPrice':'0.00001729','bidQty':'936.00000000','askPrice':'0.00001740','askQty':'349.00000000'},{'symbol':'ARDRETH','bidPrice':'0.00050934','bidQty':'946.00000000','askPrice':'0.00051341','askQty':'4025.00000000'},{'symbol':'ARDRBNB','bidPrice':'0.01094000','bidQty':'0.70000000','askPrice':'0.01117000','askQty':'179.50000000'},{'symbol':'NULSUSDT','bidPrice':'1.15570000','bidQty':'726.10000000','askPrice':'1.16220000','askQty':'59.00000000'},{'symbol':'HOTBTC','bidPrice':'0.00000014','bidQty':'1298620367.00000000','askPrice':'0.00000015','askQty':'301975287.00000000'},{'symbol':'HOTETH','bidPrice':'0.00000441','bidQty':'1097527.00000000','askPrice':'0.00000442','askQty':'3176419.00000000'},{'symbol':'VETBTC','bidPrice':'0.00000196','bidQty':'288258.00000000','askPrice':'0.00000197','askQty':'1821561.00000000'},{'symbol':'VETETH','bidPrice':'0.00005790','bidQty':'140173.00000000','askPrice':'0.00005807','askQty':'1792.00000000'},{'symbol':'VETUSDT','bidPrice':'0.01270000','bidQty':'1891.70000000','askPrice':'0.01272000','askQty':'3653.10000000'},{'symbol':'VETBNB','bidPrice':'0.00124000','bidQty':'154010.70000000','askPrice':'0.00125000','askQty':'8143.00000000'},{'symbol':'DOCKBTC','bidPrice':'0.00000280','bidQty':'18433.00000000','askPrice':'0.00000281','askQty':'556.00000000'},{'symbol':'DOCKETH','bidPrice':'0.00008247','bidQty':'3113.00000000','askPrice':'0.00008321','askQty':'209.00000000'},{'symbol':'POLYBTC','bidPrice':'0.00002742','bidQty':'7000.00000000','askPrice':'0.00002764','askQty':'83.00000000'},{'symbol':'POLYBNB','bidPrice':'0.01734000','bidQty':'450.30000000','askPrice':'0.01785000','askQty':'374.80000000'},{'symbol':'PHXBTC','bidPrice':'0.00000204','bidQty':'197425.00000000','askPrice':'0.00000205','askQty':'130602.00000000'},{'symbol':'PHXETH','bidPrice':'0.00006028','bidQty':'14845.00000000','askPrice':'0.00006100','askQty':'1525.00000000'},{'symbol':'PHXBNB','bidPrice':'0.00128800','bidQty':'5197.00000000','askPrice':'0.00132900','askQty':'11829.00000000'},{'symbol':'HCBTC','bidPrice':'0.00035610','bidQty':'7.02000000','askPrice':'0.00035820','askQty':'23.05000000'},{'symbol':'HCETH','bidPrice':'0.01052300','bidQty':'2.86000000','askPrice':'0.01063600','askQty':'24.60000000'},{'symbol':'GOBTC','bidPrice':'0.00000502','bidQty':'17551.00000000','askPrice':'0.00000504','askQty':'649435.00000000'},{'symbol':'GOBNB','bidPrice':'0.00316900','bidQty':'1484.00000000','askPrice':'0.00325100','askQty':'5978.00000000'},{'symbol':'PAXBTC','bidPrice':'0.00015540','bidQty':'15100.00000000','askPrice':'0.00015553','askQty':'64.00000000'},{'symbol':'PAXBNB','bidPrice':'0.09844000','bidQty':'40.00000000','askPrice':'0.09894000','askQty':'49.00000000'}]";
        //    tickerWrapper.TickerResponces = JsonConvert.DeserializeObject<List<TickerResponce>>(dummyResponce);
        //    tickerWrapper.ReturnCode = enResponseCode.Success;
        //    return Ok(tickerWrapper);
        //}

        //[HttpGet("GetRecentTrade")] //binance https://api.binance.com//api/v1/trades?symbol=LTCBTC
        //public IActionResult GetRecentTrade(GetRecentTradeRequest request)
        //{
        //    GetRecentTradeResponce responce = new GetRecentTradeResponce();
        //    dummyResponce = "[{'id':16522291,'price':'0.00893600','qty':'1.38000000','time':1538546339115,'isBuyerMaker':true,'isBestMatch':true},{'id':16522292,'price':'0.00893700','qty':'0.34000000','time':1538546340224,'isBuyerMaker':true,'isBestMatch':true},{'id':16522293,'price':'0.00893700','qty':'1.54000000','time':1538546341356,'isBuyerMaker':true,'isBestMatch':true},{'id':16522294,'price':'0.00893700','qty':'1.37000000','time':1538546343056,'isBuyerMaker':true,'isBestMatch':true},{'id':16522295,'price':'0.00893600','qty':'0.16000000','time':1538546344652,'isBuyerMaker':true,'isBestMatch':true},{'id':16522296,'price':'0.00893600','qty':'6.17000000','time':1538546345406,'isBuyerMaker':true,'isBestMatch':true},{'id':16522297,'price':'0.00893500','qty':'2.00000000','time':1538546345416,'isBuyerMaker':true,'isBestMatch':true},{'id':16522298,'price':'0.00893400','qty':'0.12000000','time':1538546347119,'isBuyerMaker':true,'isBestMatch':true},{'id':16522299,'price':'0.00893400','qty':'1.99000000','time':1538546350559,'isBuyerMaker':true,'isBestMatch':true},{'id':16522300,'price':'0.00893300','qty':'1.12000000','time':1538546352366,'isBuyerMaker':false,'isBestMatch':true},{'id':16522301,'price':'0.00893500','qty':'4.41000000','time':1538546352384,'isBuyerMaker':false,'isBestMatch':true},{'id':16522302,'price':'0.00893500','qty':'2.00000000','time':1538546352384,'isBuyerMaker':false,'isBestMatch':true},{'id':16522303,'price':'0.00893500','qty':'0.23000000','time':1538546352497,'isBuyerMaker':false,'isBestMatch':true},{'id':16522304,'price':'0.00893300','qty':'7.90000000','time':1538546368055,'isBuyerMaker':false,'isBestMatch':true},{'id':16522305,'price':'0.00893500','qty':'12.89000000','time':1538546368068,'isBuyerMaker':false,'isBestMatch':true},{'id':16522306,'price':'0.00893500','qty':'7.99000000','time':1538546368086,'isBuyerMaker':false,'isBestMatch':true},{'id':16522307,'price':'0.00893200','qty':'1.34000000','time':1538546391692,'isBuyerMaker':true,'isBestMatch':true},{'id':16522308,'price':'0.00893100','qty':'1.48000000','time':1538546397745,'isBuyerMaker':true,'isBestMatch':true},{'id':16522309,'price':'0.00893100','qty':'2.27000000','time':1538546400130,'isBuyerMaker':true,'isBestMatch':true},{'id':16522310,'price':'0.00893400','qty':'3.21000000','time':1538546401528,'isBuyerMaker':false,'isBestMatch':true},{'id':16522311,'price':'0.00893500','qty':'5.83000000','time':1538546401528,'isBuyerMaker':false,'isBestMatch':true},{'id':16522312,'price':'0.00892700','qty':'0.21000000','time':1538546402724,'isBuyerMaker':true,'isBestMatch':true},{'id':16522313,'price':'0.00892700','qty':'0.12000000','time':1538546403120,'isBuyerMaker':true,'isBestMatch':true},{'id':16522314,'price':'0.00893000','qty':'3.21000000','time':1538546412920,'isBuyerMaker':false,'isBestMatch':true},{'id':16522315,'price':'0.00893100','qty':'3.86000000','time':1538546412937,'isBuyerMaker':false,'isBestMatch':true},{'id':16522316,'price':'0.00894000','qty':'0.40000000','time':1538546423063,'isBuyerMaker':false,'isBestMatch':true},{'id':16522317,'price':'0.00893700','qty':'15.57000000','time':1538546430834,'isBuyerMaker':false,'isBestMatch':true},{'id':16522318,'price':'0.00894400','qty':'12.08000000','time':1538546430836,'isBuyerMaker':false,'isBestMatch':true},{'id':16522319,'price':'0.00894500','qty':'16.42000000','time':1538546430863,'isBuyerMaker':false,'isBestMatch':true},{'id':16522320,'price':'0.00893400','qty':'3.21000000','time':1538546435102,'isBuyerMaker':true,'isBestMatch':true},{'id':16522321,'price':'0.00893400','qty':'5.41000000','time':1538546435102,'isBuyerMaker':true,'isBestMatch':true},{'id':16522322,'price':'0.00893400','qty':'0.03000000','time':1538546439178,'isBuyerMaker':true,'isBestMatch':true},{'id':16522323,'price':'0.00893500','qty':'0.45000000','time':1538546451329,'isBuyerMaker':true,'isBestMatch':true},{'id':16522324,'price':'0.00893000','qty':'14.49000000','time':1538546457615,'isBuyerMaker':false,'isBestMatch':true},{'id':16522325,'price':'0.00893600','qty':'16.10000000','time':1538546457634,'isBuyerMaker':false,'isBestMatch':true},{'id':16522326,'price':'0.00893800','qty':'18.39000000','time':1538546457652,'isBuyerMaker':false,'isBestMatch':true},{'id':16522327,'price':'0.00892900','qty':'0.34000000','time':1538546481379,'isBuyerMaker':true,'isBestMatch':true},{'id':16522328,'price':'0.00893200','qty':'11.42000000','time':1538546482086,'isBuyerMaker':false,'isBestMatch':true},{'id':16522329,'price':'0.00893600','qty':'6.66000000','time':1538546482093,'isBuyerMaker':false,'isBestMatch':true},{'id':16522330,'price':'0.00892900','qty':'0.39000000','time':1538546488168,'isBuyerMaker':true,'isBestMatch':true},{'id':16522331,'price':'0.00892900','qty':'0.03000000','time':1538546494706,'isBuyerMaker':true,'isBestMatch':true},{'id':16522332,'price':'0.00892900','qty':'0.39000000','time':1538546498819,'isBuyerMaker':true,'isBestMatch':true},{'id':16522333,'price':'0.00892900','qty':'0.40000000','time':1538546509548,'isBuyerMaker':true,'isBestMatch':true},{'id':16522334,'price':'0.00892800','qty':'0.40000000','time':1538546518761,'isBuyerMaker':true,'isBestMatch':true},{'id':16522335,'price':'0.00893500','qty':'8.25000000','time':1538546524616,'isBuyerMaker':false,'isBestMatch':true},{'id':16522336,'price':'0.00893600','qty':'12.40000000','time':1538546524642,'isBuyerMaker':false,'isBestMatch':true},{'id':16522337,'price':'0.00893000','qty':'0.01000000','time':1538546528030,'isBuyerMaker':true,'isBestMatch':true},{'id':16522338,'price':'0.00893000','qty':'0.40000000','time':1538546528829,'isBuyerMaker':true,'isBestMatch':true},{'id':16522339,'price':'0.00892800','qty':'0.40000000','time':1538546538201,'isBuyerMaker':true,'isBestMatch':true},{'id':16522340,'price':'0.00892300','qty':'1.12000000','time':1538546543578,'isBuyerMaker':true,'isBestMatch':true},{'id':16522341,'price':'0.00892800','qty':'14.00000000','time':1538546552100,'isBuyerMaker':false,'isBestMatch':true},{'id':16522342,'price':'0.00892800','qty':'10.61000000','time':1538546552102,'isBuyerMaker':false,'isBestMatch':true},{'id':16522343,'price':'0.00893300','qty':'18.82000000','time':1538546559732,'isBuyerMaker':false,'isBestMatch':true},{'id':16522344,'price':'0.00893300','qty':'4.12000000','time':1538546559751,'isBuyerMaker':false,'isBestMatch':true},{'id':16522345,'price':'0.00893500','qty':'2.41000000','time':1538546564829,'isBuyerMaker':false,'isBestMatch':true},{'id':16522346,'price':'0.00893600','qty':'4.71000000','time':1538546564829,'isBuyerMaker':false,'isBestMatch':true},{'id':16522347,'price':'0.00892800','qty':'2.51000000','time':1538546566178,'isBuyerMaker':true,'isBestMatch':true},{'id':16522348,'price':'0.00893500','qty':'11.33000000','time':1538546570129,'isBuyerMaker':false,'isBestMatch':true},{'id':16522349,'price':'0.00893500','qty':'13.18000000','time':1538546570140,'isBuyerMaker':false,'isBestMatch':true},{'id':16522350,'price':'0.00892700','qty':'3.81000000','time':1538546572706,'isBuyerMaker':true,'isBestMatch':true},{'id':16522351,'price':'0.00892700','qty':'0.05000000','time':1538546572762,'isBuyerMaker':true,'isBestMatch':true},{'id':16522352,'price':'0.00892600','qty':'1.07000000','time':1538546572762,'isBuyerMaker':true,'isBestMatch':true},{'id':16522353,'price':'0.00892600','qty':'1.12000000','time':1538546572798,'isBuyerMaker':true,'isBestMatch':true},{'id':16522354,'price':'0.00892600','qty':'0.14000000','time':1538546572890,'isBuyerMaker':true,'isBestMatch':true},{'id':16522355,'price':'0.00892600','qty':'0.01000000','time':1538546573741,'isBuyerMaker':true,'isBestMatch':true},{'id':16522356,'price':'0.00893200','qty':'6.37000000','time':1538546577862,'isBuyerMaker':false,'isBestMatch':true},{'id':16522357,'price':'0.00893000','qty':'9.47000000','time':1538546593594,'isBuyerMaker':false,'isBestMatch':true},{'id':16522358,'price':'0.00893500','qty':'0.01000000','time':1538546601280,'isBuyerMaker':true,'isBestMatch':true},{'id':16522359,'price':'0.00893500','qty':'1.51000000','time':1538546603809,'isBuyerMaker':true,'isBestMatch':true},{'id':16522360,'price':'0.00893400','qty':'3.62000000','time':1538546610751,'isBuyerMaker':false,'isBestMatch':true},{'id':16522361,'price':'0.00893700','qty':'1.90000000','time':1538546610754,'isBuyerMaker':false,'isBestMatch':true},{'id':16522362,'price':'0.00893500','qty':'0.24000000','time':1538546618669,'isBuyerMaker':true,'isBestMatch':true},{'id':16522363,'price':'0.00893500','qty':'1.26000000','time':1538546619097,'isBuyerMaker':true,'isBestMatch':true},{'id':16522364,'price':'0.00893500','qty':'3.67000000','time':1538546619111,'isBuyerMaker':false,'isBestMatch':true},{'id':16522365,'price':'0.00893600','qty':'2.58000000','time':1538546619131,'isBuyerMaker':false,'isBestMatch':true},{'id':16522366,'price':'0.00893800','qty':'1.20000000','time':1538546623959,'isBuyerMaker':false,'isBestMatch':true},{'id':16522367,'price':'0.00893500','qty':'1.26000000','time':1538546637691,'isBuyerMaker':true,'isBestMatch':true},{'id':16522368,'price':'0.00893500','qty':'3.66000000','time':1538546637691,'isBuyerMaker':true,'isBestMatch':true},{'id':16522369,'price':'0.00893600','qty':'0.01000000','time':1538546640282,'isBuyerMaker':true,'isBestMatch':true},{'id':16522370,'price':'0.00894100','qty':'3.66000000','time':1538546643119,'isBuyerMaker':false,'isBestMatch':true},{'id':16522371,'price':'0.00893600','qty':'3.74000000','time':1538546643125,'isBuyerMaker':true,'isBestMatch':true},{'id':16522372,'price':'0.00893600','qty':'0.01000000','time':1538546643159,'isBuyerMaker':false,'isBestMatch':true},{'id':16522373,'price':'0.00893200','qty':'2.02000000','time':1538546663312,'isBuyerMaker':true,'isBestMatch':true},{'id':16522374,'price':'0.00893500','qty':'7.08000000','time':1538546666179,'isBuyerMaker':false,'isBestMatch':true},{'id':16522375,'price':'0.00893500','qty':'9.84000000','time':1538546666179,'isBuyerMaker':false,'isBestMatch':true},{'id':16522376,'price':'0.00893300','qty':'1.52000000','time':1538546667895,'isBuyerMaker':true,'isBestMatch':true},{'id':16522377,'price':'0.00893900','qty':'0.05000000','time':1538546674563,'isBuyerMaker':false,'isBestMatch':true},{'id':16522378,'price':'0.00893500','qty':'2.93000000','time':1538546689797,'isBuyerMaker':true,'isBestMatch':true},{'id':16522379,'price':'0.00894100','qty':'1.58000000','time':1538546697513,'isBuyerMaker':false,'isBestMatch':true},{'id':16522380,'price':'0.00893500','qty':'1.00000000','time':1538546699125,'isBuyerMaker':true,'isBestMatch':true},{'id':16522381,'price':'0.00894100','qty':'2.00000000','time':1538546701031,'isBuyerMaker':false,'isBestMatch':true},{'id':16522382,'price':'0.00893700','qty':'5.87000000','time':1538546703533,'isBuyerMaker':true,'isBestMatch':true},{'id':16522383,'price':'0.00893600','qty':'3.08000000','time':1538546703553,'isBuyerMaker':true,'isBestMatch':true},{'id':16522384,'price':'0.00893700','qty':'15.96000000','time':1538546721409,'isBuyerMaker':false,'isBestMatch':true},{'id':16522385,'price':'0.00893600','qty':'10.02000000','time':1538546721418,'isBuyerMaker':true,'isBestMatch':true},{'id':16522386,'price':'0.00893400','qty':'1.54000000','time':1538546722740,'isBuyerMaker':true,'isBestMatch':true},{'id':16522387,'price':'0.00893400','qty':'2.04000000','time':1538546724237,'isBuyerMaker':true,'isBestMatch':true},{'id':16522388,'price':'0.00893200','qty':'5.99000000','time':1538546724249,'isBuyerMaker':true,'isBestMatch':true},{'id':16522389,'price':'0.00893200','qty':'10.00000000','time':1538546726096,'isBuyerMaker':true,'isBestMatch':true},{'id':16522390,'price':'0.00893300','qty':'7.49000000','time':1538546729005,'isBuyerMaker':false,'isBestMatch':true},{'id':16522391,'price':'0.00893300','qty':'8.56000000','time':1538546729005,'isBuyerMaker':false,'isBestMatch':true},{'id':16522392,'price':'0.00893400','qty':'8.95000000','time':1538546729005,'isBuyerMaker':false,'isBestMatch':true},{'id':16522393,'price':'0.00892500','qty':'14.00000000','time':1538546731960,'isBuyerMaker':true,'isBestMatch':true},{'id':16522394,'price':'0.00892500','qty':'4.80000000','time':1538546732083,'isBuyerMaker':true,'isBestMatch':true},{'id':16522395,'price':'0.00893200','qty':'3.86000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522396,'price':'0.00893200','qty':'10.00000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522397,'price':'0.00893300','qty':'2.14000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522398,'price':'0.00893300','qty':'2.14000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522399,'price':'0.00893300','qty':'3.21000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522400,'price':'0.00893300','qty':'3.21000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522401,'price':'0.00893400','qty':'0.44000000','time':1538546732181,'isBuyerMaker':false,'isBestMatch':true},{'id':16522402,'price':'0.00892400','qty':'2.41000000','time':1538546734162,'isBuyerMaker':true,'isBestMatch':true},{'id':16522403,'price':'0.00893200','qty':'0.53000000','time':1538546735126,'isBuyerMaker':false,'isBestMatch':true},{'id':16522404,'price':'0.00893100','qty':'8.20000000','time':1538546735830,'isBuyerMaker':false,'isBestMatch':true},{'id':16522405,'price':'0.00893200','qty':'16.80000000','time':1538546735830,'isBuyerMaker':false,'isBestMatch':true},{'id':16522406,'price':'0.00892300','qty':'3.85000000','time':1538546736475,'isBuyerMaker':true,'isBestMatch':true},{'id':16522407,'price':'0.00892300','qty':'0.01000000','time':1538546739202,'isBuyerMaker':true,'isBestMatch':true},{'id':16522408,'price':'0.00892200','qty':'1.06000000','time':1538546739202,'isBuyerMaker':true,'isBestMatch':true},{'id':16522409,'price':'0.00892200','qty':'0.48000000','time':1538546739216,'isBuyerMaker':true,'isBestMatch':true},{'id':16522410,'price':'0.00892000','qty':'0.76000000','time':1538546740331,'isBuyerMaker':true,'isBestMatch':true},{'id':16522411,'price':'0.00891600','qty':'0.17000000','time':1538546742088,'isBuyerMaker':true,'isBestMatch':true},{'id':16522412,'price':'0.00891600','qty':'0.34000000','time':1538546743557,'isBuyerMaker':true,'isBestMatch':true},{'id':16522413,'price':'0.00892200','qty':'0.01000000','time':1538546745711,'isBuyerMaker':false,'isBestMatch':true},{'id':16522414,'price':'0.00892200','qty':'13.32000000','time':1538546749961,'isBuyerMaker':false,'isBestMatch':true},{'id':16522415,'price':'0.00892500','qty':'0.79000000','time':1538546753186,'isBuyerMaker':false,'isBestMatch':true},{'id':16522416,'price':'0.00892500','qty':'3.25000000','time':1538546756582,'isBuyerMaker':false,'isBestMatch':true},{'id':16522417,'price':'0.00892700','qty':'9.28000000','time':1538546762316,'isBuyerMaker':true,'isBestMatch':true},{'id':16522418,'price':'0.00892700','qty':'7.18000000','time':1538546762355,'isBuyerMaker':true,'isBestMatch':true},{'id':16522419,'price':'0.00892800','qty':'1.42000000','time':1538546774152,'isBuyerMaker':false,'isBestMatch':true},{'id':16522420,'price':'0.00892800','qty':'18.22000000','time':1538546774362,'isBuyerMaker':false,'isBestMatch':true},{'id':16522421,'price':'0.00892800','qty':'0.34000000','time':1538546774419,'isBuyerMaker':true,'isBestMatch':true},{'id':16522422,'price':'0.00892800','qty':'1.08000000','time':1538546775874,'isBuyerMaker':true,'isBestMatch':true},{'id':16522423,'price':'0.00892600','qty':'11.22000000','time':1538546776315,'isBuyerMaker':true,'isBestMatch':true},{'id':16522424,'price':'0.00892500','qty':'0.01000000','time':1538546788377,'isBuyerMaker':true,'isBestMatch':true},{'id':16522425,'price':'0.00892500','qty':'1.79000000','time':1538546789469,'isBuyerMaker':true,'isBestMatch':true},{'id':16522426,'price':'0.00892800','qty':'3.58000000','time':1538546790058,'isBuyerMaker':false,'isBestMatch':true},{'id':16522427,'price':'0.00892800','qty':'17.76000000','time':1538546790077,'isBuyerMaker':false,'isBestMatch':true},{'id':16522428,'price':'0.00892500','qty':'1.25000000','time':1538546796408,'isBuyerMaker':true,'isBestMatch':true},{'id':16522429,'price':'0.00892500','qty':'5.62000000','time':1538546796408,'isBuyerMaker':true,'isBestMatch':true},{'id':16522430,'price':'0.00892500','qty':'1.27000000','time':1538546797802,'isBuyerMaker':true,'isBestMatch':true},{'id':16522431,'price':'0.00892500','qty':'2.58000000','time':1538546800435,'isBuyerMaker':true,'isBestMatch':true},{'id':16522432,'price':'0.00891700','qty':'0.14000000','time':1538546803913,'isBuyerMaker':true,'isBestMatch':true},{'id':16522433,'price':'0.00891900','qty':'7.36000000','time':1538546829718,'isBuyerMaker':false,'isBestMatch':true},{'id':16522434,'price':'0.00892200','qty':'5.70000000','time':1538546829729,'isBuyerMaker':false,'isBestMatch':true},{'id':16522435,'price':'0.00892100','qty':'6.98000000','time':1538546841760,'isBuyerMaker':false,'isBestMatch':true},{'id':16522436,'price':'0.00892500','qty':'0.81000000','time':1538546841765,'isBuyerMaker':false,'isBestMatch':true},{'id':16522437,'price':'0.00892500','qty':'5.54000000','time':1538546841783,'isBuyerMaker':false,'isBestMatch':true},{'id':16522438,'price':'0.00892200','qty':'25.15000000','time':1538546858595,'isBuyerMaker':false,'isBestMatch':true},{'id':16522439,'price':'0.00892600','qty':'18.51000000','time':1538546858604,'isBuyerMaker':false,'isBestMatch':true},{'id':16522440,'price':'0.00892700','qty':'14.58000000','time':1538546858622,'isBuyerMaker':false,'isBestMatch':true},{'id':16522441,'price':'0.00892500','qty':'2.83000000','time':1538546866867,'isBuyerMaker':true,'isBestMatch':true},{'id':16522442,'price':'0.00892300','qty':'8.39000000','time':1538546877821,'isBuyerMaker':false,'isBestMatch':true},{'id':16522443,'price':'0.00892400','qty':'3.10000000','time':1538546877822,'isBuyerMaker':false,'isBestMatch':true},{'id':16522444,'price':'0.00892200','qty':'7.81000000','time':1538546895812,'isBuyerMaker':false,'isBestMatch':true},{'id':16522445,'price':'0.00892400','qty':'4.37000000','time':1538546895833,'isBuyerMaker':false,'isBestMatch':true},{'id':16522446,'price':'0.00892500','qty':'4.70000000','time':1538546895858,'isBuyerMaker':false,'isBestMatch':true},{'id':16522447,'price':'0.00892000','qty':'0.34000000','time':1538546898394,'isBuyerMaker':true,'isBestMatch':true},{'id':16522448,'price':'0.00892000','qty':'8.42000000','time':1538546902435,'isBuyerMaker':true,'isBestMatch':true},{'id':16522449,'price':'0.00892000','qty':'1.23000000','time':1538546908510,'isBuyerMaker':true,'isBestMatch':true},{'id':16522450,'price':'0.00892000','qty':'3.50000000','time':1538546922522,'isBuyerMaker':false,'isBestMatch':true},{'id':16522451,'price':'0.00891800','qty':'8.62000000','time':1538546926066,'isBuyerMaker':false,'isBestMatch':true},{'id':16522452,'price':'0.00892000','qty':'8.33000000','time':1538546926082,'isBuyerMaker':false,'isBestMatch':true},{'id':16522453,'price':'0.00892000','qty':'3.50000000','time':1538546926082,'isBuyerMaker':false,'isBestMatch':true},{'id':16522454,'price':'0.00892000','qty':'6.76000000','time':1538546926105,'isBuyerMaker':false,'isBestMatch':true},{'id':16522455,'price':'0.00892000','qty':'0.30000000','time':1538546926105,'isBuyerMaker':false,'isBestMatch':true},{'id':16522456,'price':'0.00892000','qty':'0.80000000','time':1538546926105,'isBuyerMaker':false,'isBestMatch':true},{'id':16522457,'price':'0.00892000','qty':'2.39000000','time':1538546926157,'isBuyerMaker':true,'isBestMatch':true},{'id':16522458,'price':'0.00891900','qty':'6.17000000','time':1538546926275,'isBuyerMaker':false,'isBestMatch':true},{'id':16522459,'price':'0.00892300','qty':'2.04000000','time':1538546926275,'isBuyerMaker':false,'isBestMatch':true},{'id':16522460,'price':'0.00892000','qty':'0.80000000','time':1538546931464,'isBuyerMaker':false,'isBestMatch':true},{'id':16522461,'price':'0.00892000','qty':'0.32000000','time':1538546931464,'isBuyerMaker':false,'isBestMatch':true},{'id':16522462,'price':'0.00891900','qty':'4.05000000','time':1538546938436,'isBuyerMaker':false,'isBestMatch':true},{'id':16522463,'price':'0.00892100','qty':'5.79000000','time':1538546938449,'isBuyerMaker':false,'isBestMatch':true},{'id':16522464,'price':'0.00892100','qty':'3.10000000','time':1538546938472,'isBuyerMaker':false,'isBestMatch':true},{'id':16522465,'price':'0.00891900','qty':'2.13000000','time':1538546950608,'isBuyerMaker':false,'isBestMatch':true},{'id':16522466,'price':'0.00892100','qty':'8.09000000','time':1538546950617,'isBuyerMaker':false,'isBestMatch':true},{'id':16522467,'price':'0.00891800','qty':'0.50000000','time':1538546967092,'isBuyerMaker':true,'isBestMatch':true},{'id':16522468,'price':'0.00891800','qty':'0.50000000','time':1538546973175,'isBuyerMaker':false,'isBestMatch':true},{'id':16522469,'price':'0.00892200','qty':'0.51000000','time':1538546973175,'isBuyerMaker':false,'isBestMatch':true},{'id':16522470,'price':'0.00891700','qty':'0.50000000','time':1538546977522,'isBuyerMaker':true,'isBestMatch':true},{'id':16522471,'price':'0.00891600','qty':'2.88000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522472,'price':'0.00891500','qty':'0.60000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522473,'price':'0.00891500','qty':'6.29000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522474,'price':'0.00891300','qty':'0.12000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522475,'price':'0.00891000','qty':'0.30000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522476,'price':'0.00891000','qty':'4.62000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522477,'price':'0.00891000','qty':'4.75000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522478,'price':'0.00890400','qty':'0.70000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522479,'price':'0.00890200','qty':'1.94000000','time':1538546994712,'isBuyerMaker':true,'isBestMatch':true},{'id':16522480,'price':'0.00891000','qty':'17.88000000','time':1538546998107,'isBuyerMaker':false,'isBestMatch':true},{'id':16522481,'price':'0.00891700','qty':'10.35000000','time':1538546998116,'isBuyerMaker':false,'isBestMatch':true},{'id':16522482,'price':'0.00891700','qty':'0.14000000','time':1538547002145,'isBuyerMaker':false,'isBestMatch':true},{'id':16522483,'price':'0.00891400','qty':'0.53000000','time':1538547008346,'isBuyerMaker':false,'isBestMatch':true},{'id':16522484,'price':'0.00891400','qty':'0.51000000','time':1538547009241,'isBuyerMaker':false,'isBestMatch':true},{'id':16522485,'price':'0.00891700','qty':'0.32000000','time':1538547013825,'isBuyerMaker':false,'isBestMatch':true},{'id':16522486,'price':'0.00891600','qty':'8.16000000','time':1538547021468,'isBuyerMaker':false,'isBestMatch':true},{'id':16522487,'price':'0.00891600','qty':'11.73000000','time':1538547022020,'isBuyerMaker':false,'isBestMatch':true},{'id':16522488,'price':'0.00892300','qty':'11.08000000','time':1538547035437,'isBuyerMaker':false,'isBestMatch':true},{'id':16522489,'price':'0.00891700','qty':'2.00000000','time':1538547036564,'isBuyerMaker':true,'isBestMatch':true},{'id':16522490,'price':'0.00892000','qty':'0.30000000','time':1538547037733,'isBuyerMaker':false,'isBestMatch':true},{'id':16522491,'price':'0.00892100','qty':'0.17000000','time':1538547037733,'isBuyerMaker':false,'isBestMatch':true},{'id':16522492,'price':'0.00891100','qty':'35.77000000','time':1538547048515,'isBuyerMaker':true,'isBestMatch':true},{'id':16522493,'price':'0.00892000','qty':'10.00000000','time':1538547062468,'isBuyerMaker':false,'isBestMatch':true},{'id':16522494,'price':'0.00891800','qty':'5.20000000','time':1538547065796,'isBuyerMaker':false,'isBestMatch':true},{'id':16522495,'price':'0.00892000','qty':'6.29000000','time':1538547065796,'isBuyerMaker':false,'isBestMatch':true},{'id':16522496,'price':'0.00892100','qty':'3.07000000','time':1538547065796,'isBuyerMaker':false,'isBestMatch':true},{'id':16522497,'price':'0.00892100','qty':'4.86000000','time':1538547065796,'isBuyerMaker':false,'isBestMatch':true},{'id':16522498,'price':'0.00891600','qty':'23.84000000','time':1538547081772,'isBuyerMaker':false,'isBestMatch':true},{'id':16522499,'price':'0.00891900','qty':'17.07000000','time':1538547081782,'isBuyerMaker':false,'isBestMatch':true},{'id':16522500,'price':'0.00891600','qty':'8.50000000','time':1538547117758,'isBuyerMaker':true,'isBestMatch':true},{'id':16522501,'price':'0.00891600','qty':'0.01000000','time':1538547117770,'isBuyerMaker':true,'isBestMatch':true},{'id':16522502,'price':'0.00891600','qty':'17.70000000','time':1538547117770,'isBuyerMaker':true,'isBestMatch':true},{'id':16522503,'price':'0.00892100','qty':'0.16000000','time':1538547119877,'isBuyerMaker':false,'isBestMatch':true},{'id':16522504,'price':'0.00891700','qty':'6.99000000','time':1538547126700,'isBuyerMaker':true,'isBestMatch':true},{'id':16522505,'price':'0.00891800','qty':'0.68000000','time':1538547133641,'isBuyerMaker':false,'isBestMatch':true},{'id':16522506,'price':'0.00891600','qty':'25.50000000','time':1538547151973,'isBuyerMaker':false,'isBestMatch':true},{'id':16522507,'price':'0.00891800','qty':'14.51000000','time':1538547151978,'isBuyerMaker':false,'isBestMatch':true},{'id':16522508,'price':'0.00891600','qty':'0.50000000','time':1538547155166,'isBuyerMaker':false,'isBestMatch':true},{'id':16522509,'price':'0.00891500','qty':'4.00000000','time':1538547155982,'isBuyerMaker':true,'isBestMatch':true},{'id':16522510,'price':'0.00891500','qty':'12.16000000','time':1538547155993,'isBuyerMaker':false,'isBestMatch':true},{'id':16522511,'price':'0.00891600','qty':'12.03000000','time':1538547156011,'isBuyerMaker':false,'isBestMatch':true},{'id':16522512,'price':'0.00891600','qty':'0.49000000','time':1538547156011,'isBuyerMaker':false,'isBestMatch':true},{'id':16522513,'price':'0.00891600','qty':'2.65000000','time':1538547156029,'isBuyerMaker':false,'isBestMatch':true},{'id':16522514,'price':'0.00891600','qty':'12.03000000','time':1538547156040,'isBuyerMaker':false,'isBestMatch':true},{'id':16522515,'price':'0.00891800','qty':'12.44000000','time':1538547168205,'isBuyerMaker':false,'isBestMatch':true},{'id':16522516,'price':'0.00892200','qty':'6.01000000','time':1538547174553,'isBuyerMaker':false,'isBestMatch':true},{'id':16522517,'price':'0.00892700','qty':'20.00000000','time':1538547175209,'isBuyerMaker':false,'isBestMatch':true},{'id':16522518,'price':'0.00893000','qty':'7.84000000','time':1538547176210,'isBuyerMaker':false,'isBestMatch':true},{'id':16522519,'price':'0.00894000','qty':'0.20000000','time':1538547179173,'isBuyerMaker':false,'isBestMatch':true},{'id':16522520,'price':'0.00894500','qty':'2.59000000','time':1538547179183,'isBuyerMaker':false,'isBestMatch':true},{'id':16522521,'price':'0.00894900','qty':'0.70000000','time':1538547179198,'isBuyerMaker':false,'isBestMatch':true},{'id':16522522,'price':'0.00894100','qty':'1.26000000','time':1538547179513,'isBuyerMaker':true,'isBestMatch':true},{'id':16522523,'price':'0.00894100','qty':'0.89000000','time':1538547179759,'isBuyerMaker':true,'isBestMatch':true},{'id':16522524,'price':'0.00894100','qty':'2.77000000','time':1538547179759,'isBuyerMaker':true,'isBestMatch':true},{'id':16522525,'price':'0.00894800','qty':'14.00000000','time':1538547181238,'isBuyerMaker':false,'isBestMatch':true},{'id':16522526,'price':'0.00894800','qty':'28.00000000','time':1538547181254,'isBuyerMaker':false,'isBestMatch':true},{'id':16522527,'price':'0.00894100','qty':'0.20000000','time':1538547181330,'isBuyerMaker':true,'isBestMatch':true},{'id':16522528,'price':'0.00894800','qty':'14.00000000','time':1538547183210,'isBuyerMaker':false,'isBestMatch':true},{'id':16522529,'price':'0.00894400','qty':'1.31000000','time':1538547183808,'isBuyerMaker':true,'isBestMatch':true},{'id':16522530,'price':'0.00894400','qty':'0.31000000','time':1538547183808,'isBuyerMaker':true,'isBestMatch':true},{'id':16522531,'price':'0.00894400','qty':'0.60000000','time':1538547184070,'isBuyerMaker':true,'isBestMatch':true},{'id':16522532,'price':'0.00894400','qty':'0.37000000','time':1538547184191,'isBuyerMaker':true,'isBestMatch':true},{'id':16522533,'price':'0.00894400','qty':'8.72000000','time':1538547184208,'isBuyerMaker':true,'isBestMatch':true},{'id':16522534,'price':'0.00893500','qty':'0.20000000','time':1538547184772,'isBuyerMaker':false,'isBestMatch':true},{'id':16522535,'price':'0.00893500','qty':'0.10000000','time':1538547185206,'isBuyerMaker':false,'isBestMatch':true},{'id':16522536,'price':'0.00893500','qty':'0.20000000','time':1538547185206,'isBuyerMaker':false,'isBestMatch':true},{'id':16522537,'price':'0.00894600','qty':'2.90000000','time':1538547185219,'isBuyerMaker':false,'isBestMatch':true},{'id':16522538,'price':'0.00893400','qty':'0.10000000','time':1538547201877,'isBuyerMaker':true,'isBestMatch':true},{'id':16522539,'price':'0.00893600','qty':'12.47000000','time':1538547215218,'isBuyerMaker':true,'isBestMatch':true},{'id':16522540,'price':'0.00893400','qty':'2.25000000','time':1538547215242,'isBuyerMaker':true,'isBestMatch':true},{'id':16522541,'price':'0.00892900','qty':'18.82000000','time':1538547235958,'isBuyerMaker':false,'isBestMatch':true},{'id':16522542,'price':'0.00892600','qty':'18.46000000','time':1538547235966,'isBuyerMaker':true,'isBestMatch':true},{'id':16522543,'price':'0.00893100','qty':'8.00000000','time':1538547236514,'isBuyerMaker':false,'isBestMatch':true},{'id':16522544,'price':'0.00892800','qty':'5.91000000','time':1538547252237,'isBuyerMaker':true,'isBestMatch':true},{'id':16522545,'price':'0.00892800','qty':'5.83000000','time':1538547252237,'isBuyerMaker':true,'isBestMatch':true},{'id':16522546,'price':'0.00892200','qty':'3.86000000','time':1538547256273,'isBuyerMaker':true,'isBestMatch':true},{'id':16522547,'price':'0.00892100','qty':'11.20000000','time':1538547256381,'isBuyerMaker':true,'isBestMatch':true},{'id':16522548,'price':'0.00892300','qty':'6.83000000','time':1538547269267,'isBuyerMaker':false,'isBestMatch':true},{'id':16522549,'price':'0.00891900','qty':'9.69000000','time':1538547277073,'isBuyerMaker':false,'isBestMatch':true},{'id':16522550,'price':'0.00892000','qty':'3.03000000','time':1538547277073,'isBuyerMaker':false,'isBestMatch':true},{'id':16522551,'price':'0.00892000','qty':'16.57000000','time':1538547280239,'isBuyerMaker':false,'isBestMatch':true},{'id':16522552,'price':'0.00892100','qty':'0.34000000','time':1538547282501,'isBuyerMaker':false,'isBestMatch':true},{'id':16522553,'price':'0.00892000','qty':'3.03000000','time':1538547284206,'isBuyerMaker':true,'isBestMatch':true},{'id':16522554,'price':'0.00892100','qty':'0.19000000','time':1538547285915,'isBuyerMaker':false,'isBestMatch':true},{'id':16522555,'price':'0.00891600','qty':'23.62000000','time':1538547286718,'isBuyerMaker':false,'isBestMatch':true},{'id':16522556,'price':'0.00892100','qty':'10.70000000','time':1538547286735,'isBuyerMaker':false,'isBestMatch':true},{'id':16522557,'price':'0.00892100','qty':'0.53000000','time':1538547288298,'isBuyerMaker':true,'isBestMatch':true},{'id':16522558,'price':'0.00891200','qty':'0.25000000','time':1538547288368,'isBuyerMaker':true,'isBestMatch':true},{'id':16522559,'price':'0.00892300','qty':'0.86000000','time':1538547300032,'isBuyerMaker':false,'isBestMatch':true},{'id':16522560,'price':'0.00892000','qty':'4.54000000','time':1538547304648,'isBuyerMaker':false,'isBestMatch':true},{'id':16522561,'price':'0.00892200','qty':'15.72000000','time':1538547314007,'isBuyerMaker':false,'isBestMatch':true},{'id':16522562,'price':'0.00892200','qty':'5.66000000','time':1538547331078,'isBuyerMaker':false,'isBestMatch':true},{'id':16522563,'price':'0.00892000','qty':'3.86000000','time':1538547334869,'isBuyerMaker':false,'isBestMatch':true},{'id':16522564,'price':'0.00891400','qty':'18.27000000','time':1538547335830,'isBuyerMaker':false,'isBestMatch':true},{'id':16522565,'price':'0.00892100','qty':'17.67000000','time':1538547335838,'isBuyerMaker':false,'isBestMatch':true},{'id':16522566,'price':'0.00891400','qty':'0.28000000','time':1538547349629,'isBuyerMaker':true,'isBestMatch':true},{'id':16522567,'price':'0.00891800','qty':'32.71000000','time':1538547358293,'isBuyerMaker':false,'isBestMatch':true},{'id':16522568,'price':'0.00892200','qty':'12.98000000','time':1538547358304,'isBuyerMaker':false,'isBestMatch':true},{'id':16522569,'price':'0.00892200','qty':'14.92000000','time':1538547358305,'isBuyerMaker':false,'isBestMatch':true},{'id':16522570,'price':'0.00892200','qty':'0.01000000','time':1538547358315,'isBuyerMaker':false,'isBestMatch':true},{'id':16522571,'price':'0.00892300','qty':'0.24000000','time':1538547358315,'isBuyerMaker':false,'isBestMatch':true},{'id':16522572,'price':'0.00892700','qty':'17.12000000','time':1538547365494,'isBuyerMaker':false,'isBestMatch':true},{'id':16522573,'price':'0.00892000','qty':'8.38000000','time':1538547382549,'isBuyerMaker':false,'isBestMatch':true},{'id':16522574,'price':'0.00892000','qty':'21.69000000','time':1538547382555,'isBuyerMaker':false,'isBestMatch':true},{'id':16522575,'price':'0.00892200','qty':'13.65000000','time':1538547382566,'isBuyerMaker':false,'isBestMatch':true},{'id':16522576,'price':'0.00892100','qty':'6.04000000','time':1538547386097,'isBuyerMaker':false,'isBestMatch':true},{'id':16522577,'price':'0.00892000','qty':'8.38000000','time':1538547388838,'isBuyerMaker':true,'isBestMatch':true},{'id':16522578,'price':'0.00892000','qty':'0.16000000','time':1538547388846,'isBuyerMaker':false,'isBestMatch':true},{'id':16522579,'price':'0.00892100','qty':'4.71000000','time':1538547388869,'isBuyerMaker':false,'isBestMatch':true},{'id':16522580,'price':'0.00892300','qty':'17.26000000','time':1538547391411,'isBuyerMaker':false,'isBestMatch':true},{'id':16522581,'price':'0.00892100','qty':'4.09000000','time':1538547392150,'isBuyerMaker':true,'isBestMatch':true},{'id':16522582,'price':'0.00893000','qty':'18.37000000','time':1538547399381,'isBuyerMaker':false,'isBestMatch':true},{'id':16522583,'price':'0.00892300','qty':'9.33000000','time':1538547399401,'isBuyerMaker':true,'isBestMatch':true},{'id':16522584,'price':'0.00892300','qty':'15.83000000','time':1538547408570,'isBuyerMaker':true,'isBestMatch':true},{'id':16522585,'price':'0.00892200','qty':'0.34000000','time':1538547410924,'isBuyerMaker':true,'isBestMatch':true},{'id':16522586,'price':'0.00892500','qty':'37.26000000','time':1538547433476,'isBuyerMaker':false,'isBestMatch':true},{'id':16522587,'price':'0.00893000','qty':'19.42000000','time':1538547433487,'isBuyerMaker':false,'isBestMatch':true},{'id':16522588,'price':'0.00892900','qty':'3.21000000','time':1538547436075,'isBuyerMaker':false,'isBestMatch':true},{'id':16522589,'price':'0.00893000','qty':'10.00000000','time':1538547436075,'isBuyerMaker':false,'isBestMatch':true},{'id':16522590,'price':'0.00893200','qty':'5.67000000','time':1538547436075,'isBuyerMaker':false,'isBestMatch':true},{'id':16522591,'price':'0.00893000','qty':'1.00000000','time':1538547436354,'isBuyerMaker':false,'isBestMatch':true},{'id':16522592,'price':'0.00893000','qty':'1.00000000','time':1538547437775,'isBuyerMaker':false,'isBestMatch':true},{'id':16522593,'price':'0.00893000','qty':'2.35000000','time':1538547441248,'isBuyerMaker':false,'isBestMatch':true},{'id':16522594,'price':'0.00892700','qty':'11.67000000','time':1538547447820,'isBuyerMaker':false,'isBestMatch':true},{'id':16522595,'price':'0.00892500','qty':'10.97000000','time':1538547447838,'isBuyerMaker':true,'isBestMatch':true},{'id':16522596,'price':'0.00892100','qty':'1.00000000','time':1538547453367,'isBuyerMaker':true,'isBestMatch':true},{'id':16522597,'price':'0.00891400','qty':'0.22000000','time':1538547472514,'isBuyerMaker':true,'isBestMatch':true},{'id':16522598,'price':'0.00892300','qty':'7.70000000','time':1538547478218,'isBuyerMaker':false,'isBestMatch':true},{'id':16522599,'price':'0.00892400','qty':'2.00000000','time':1538547483885,'isBuyerMaker':false,'isBestMatch':true},{'id':16522600,'price':'0.00892400','qty':'14.19000000','time':1538547485933,'isBuyerMaker':false,'isBestMatch':true},{'id':16522601,'price':'0.00892400','qty':'2.00000000','time':1538547498625,'isBuyerMaker':true,'isBestMatch':true},{'id':16522602,'price':'0.00891900','qty':'7.00000000','time':1538547498625,'isBuyerMaker':true,'isBestMatch':true},{'id':16522603,'price':'0.00892600','qty':'0.34000000','time':1538547504621,'isBuyerMaker':false,'isBestMatch':true},{'id':16522604,'price':'0.00891900','qty':'3.00000000','time':1538547508084,'isBuyerMaker':true,'isBestMatch':true},{'id':16522605,'price':'0.00891800','qty':'8.20000000','time':1538547508084,'isBuyerMaker':true,'isBestMatch':true},{'id':16522606,'price':'0.00892500','qty':'11.17000000','time':1538547508611,'isBuyerMaker':false,'isBestMatch':true},{'id':16522607,'price':'0.00892300','qty':'0.77000000','time':1538547518454,'isBuyerMaker':false,'isBestMatch':true},{'id':16522608,'price':'0.00891800','qty':'12.82000000','time':1538547519736,'isBuyerMaker':false,'isBestMatch':true},{'id':16522609,'price':'0.00892300','qty':'10.79000000','time':1538547519739,'isBuyerMaker':false,'isBestMatch':true},{'id':16522610,'price':'0.00892300','qty':'0.77000000','time':1538547519739,'isBuyerMaker':false,'isBestMatch':true},{'id':16522611,'price':'0.00892300','qty':'0.77000000','time':1538547520146,'isBuyerMaker':false,'isBestMatch':true},{'id':16522612,'price':'0.00891400','qty':'0.44000000','time':1538547534123,'isBuyerMaker':true,'isBestMatch':true},{'id':16522613,'price':'0.00892000','qty':'0.55000000','time':1538547534280,'isBuyerMaker':false,'isBestMatch':true},{'id':16522614,'price':'0.00892000','qty':'0.78000000','time':1538547534902,'isBuyerMaker':false,'isBestMatch':true},{'id':16522615,'price':'0.00892000','qty':'1.04000000','time':1538547535086,'isBuyerMaker':false,'isBestMatch':true},{'id':16522616,'price':'0.00892000','qty':'1.13000000','time':1538547535099,'isBuyerMaker':false,'isBestMatch':true},{'id':16522617,'price':'0.00892000','qty':'0.90000000','time':1538547535337,'isBuyerMaker':false,'isBestMatch':true},{'id':16522618,'price':'0.00892000','qty':'4.50000000','time':1538547535378,'isBuyerMaker':false,'isBestMatch':true},{'id':16522619,'price':'0.00892000','qty':'2.11000000','time':1538547538499,'isBuyerMaker':false,'isBestMatch':true},{'id':16522620,'price':'0.00892000','qty':'8.89000000','time':1538547538499,'isBuyerMaker':false,'isBestMatch':true},{'id':16522621,'price':'0.00891800','qty':'9.16000000','time':1538547551191,'isBuyerMaker':false,'isBestMatch':true},{'id':16522622,'price':'0.00891800','qty':'5.41000000','time':1538547551191,'isBuyerMaker':false,'isBestMatch':true},{'id':16522623,'price':'0.00892000','qty':'1.11000000','time':1538547551191,'isBuyerMaker':false,'isBestMatch':true},{'id':16522624,'price':'0.00891500','qty':'2.86000000','time':1538547554760,'isBuyerMaker':true,'isBestMatch':true},{'id':16522625,'price':'0.00891400','qty':'10.90000000','time':1538547554760,'isBuyerMaker':true,'isBestMatch':true},{'id':16522626,'price':'0.00891800','qty':'2.80000000','time':1538547571431,'isBuyerMaker':false,'isBestMatch':true},{'id':16522627,'price':'0.00891600','qty':'2.91000000','time':1538547571445,'isBuyerMaker':true,'isBestMatch':true},{'id':16522628,'price':'0.00891800','qty':'1.25000000','time':1538547592158,'isBuyerMaker':false,'isBestMatch':true},{'id':16522629,'price':'0.00891800','qty':'2.61000000','time':1538547593781,'isBuyerMaker':false,'isBestMatch':true},{'id':16522630,'price':'0.00891800','qty':'0.57000000','time':1538547593781,'isBuyerMaker':false,'isBestMatch':true},{'id':16522631,'price':'0.00891600','qty':'10.00000000','time':1538547594747,'isBuyerMaker':false,'isBestMatch':true},{'id':16522632,'price':'0.00891600','qty':'0.59000000','time':1538547595430,'isBuyerMaker':false,'isBestMatch':true},{'id':16522633,'price':'0.00891700','qty':'1.12000000','time':1538547595430,'isBuyerMaker':false,'isBestMatch':true},{'id':16522634,'price':'0.00891400','qty':'0.41000000','time':1538547595717,'isBuyerMaker':true,'isBestMatch':true},{'id':16522635,'price':'0.00891700','qty':'0.85000000','time':1538547597587,'isBuyerMaker':false,'isBestMatch':true},{'id':16522636,'price':'0.00891500','qty':'20.67000000','time':1538547598494,'isBuyerMaker':false,'isBestMatch':true},{'id':16522637,'price':'0.00891700','qty':'8.54000000','time':1538547598508,'isBuyerMaker':false,'isBestMatch':true},{'id':16522638,'price':'0.00891800','qty':'3.16000000','time':1538547601814,'isBuyerMaker':false,'isBestMatch':true},{'id':16522639,'price':'0.00892200','qty':'5.21000000','time':1538547601814,'isBuyerMaker':false,'isBestMatch':true},{'id':16522640,'price':'0.00892000','qty':'13.66000000','time':1538547603633,'isBuyerMaker':true,'isBestMatch':true},{'id':16522641,'price':'0.00892000','qty':'0.34000000','time':1538547603633,'isBuyerMaker':true,'isBestMatch':true},{'id':16522642,'price':'0.00892000','qty':'3.83000000','time':1538547603650,'isBuyerMaker':true,'isBestMatch':true},{'id':16522643,'price':'0.00891700','qty':'1.96000000','time':1538547606119,'isBuyerMaker':true,'isBestMatch':true},{'id':16522644,'price':'0.00891700','qty':'8.53000000','time':1538547606376,'isBuyerMaker':false,'isBestMatch':true},{'id':16522645,'price':'0.00891700','qty':'0.01000000','time':1538547614185,'isBuyerMaker':false,'isBestMatch':true},{'id':16522646,'price':'0.00892000','qty':'0.34000000','time':1538547614185,'isBuyerMaker':false,'isBestMatch':true},{'id':16522647,'price':'0.00891700','qty':'0.62000000','time':1538547619456,'isBuyerMaker':true,'isBestMatch':true},{'id':16522648,'price':'0.00892200','qty':'0.38000000','time':1538547621534,'isBuyerMaker':false,'isBestMatch':true},{'id':16522649,'price':'0.00892100','qty':'9.58000000','time':1538547628275,'isBuyerMaker':true,'isBestMatch':true},{'id':16522650,'price':'0.00892100','qty':'3.39000000','time':1538547628275,'isBuyerMaker':true,'isBestMatch':true},{'id':16522651,'price':'0.00892200','qty':'1.36000000','time':1538547633838,'isBuyerMaker':false,'isBestMatch':true},{'id':16522652,'price':'0.00892300','qty':'0.62000000','time':1538547633838,'isBuyerMaker':false,'isBestMatch':true},{'id':16522653,'price':'0.00892300','qty':'5.58000000','time':1538547633838,'isBuyerMaker':false,'isBestMatch':true},{'id':16522654,'price':'0.00893800','qty':'25.76000000','time':1538547633838,'isBuyerMaker':false,'isBestMatch':true},{'id':16522655,'price':'0.00893100','qty':'1.54000000','time':1538547634393,'isBuyerMaker':false,'isBestMatch':true},{'id':16522656,'price':'0.00892100','qty':'0.56000000','time':1538547655000,'isBuyerMaker':true,'isBestMatch':true},{'id':16522657,'price':'0.00892100','qty':'2.35000000','time':1538547656447,'isBuyerMaker':true,'isBestMatch':true},{'id':16522658,'price':'0.00891800','qty':'0.28000000','time':1538547657490,'isBuyerMaker':true,'isBestMatch':true},{'id':16522659,'price':'0.00891800','qty':'9.72000000','time':1538547657727,'isBuyerMaker':true,'isBestMatch':true},{'id':16522660,'price':'0.00891300','qty':'0.28000000','time':1538547657746,'isBuyerMaker':false,'isBestMatch':true},{'id':16522661,'price':'0.00891800','qty':'5.00000000','time':1538547658227,'isBuyerMaker':true,'isBestMatch':true},{'id':16522662,'price':'0.00892700','qty':'1.12000000','time':1538547658903,'isBuyerMaker':false,'isBestMatch':true},{'id':16522663,'price':'0.00891800','qty':'5.00000000','time':1538547659246,'isBuyerMaker':false,'isBestMatch':true},{'id':16522664,'price':'0.00891600','qty':'10.08000000','time':1538547688355,'isBuyerMaker':false,'isBestMatch':true},{'id':16522665,'price':'0.00891800','qty':'0.46000000','time':1538547697965,'isBuyerMaker':false,'isBestMatch':true},{'id':16522666,'price':'0.00891400','qty':'0.95000000','time':1538547698946,'isBuyerMaker':true,'isBestMatch':true},{'id':16522667,'price':'0.00891400','qty':'0.19000000','time':1538547698946,'isBuyerMaker':true,'isBestMatch':true},{'id':16522668,'price':'0.00891300','qty':'0.02000000','time':1538547698946,'isBuyerMaker':true,'isBestMatch':true},{'id':16522669,'price':'0.00891700','qty':'5.00000000','time':1538547706928,'isBuyerMaker':true,'isBestMatch':true},{'id':16522670,'price':'0.00891700','qty':'5.00000000','time':1538547707925,'isBuyerMaker':true,'isBestMatch':true},{'id':16522671,'price':'0.00891700','qty':'5.00000000','time':1538547708940,'isBuyerMaker':true,'isBestMatch':true},{'id':16522672,'price':'0.00892400','qty':'1.12000000','time':1538547709883,'isBuyerMaker':false,'isBestMatch':true},{'id':16522673,'price':'0.00891700','qty':'4.17000000','time':1538547709929,'isBuyerMaker':true,'isBestMatch':true},{'id':16522674,'price':'0.00891700','qty':'0.83000000','time':1538547709929,'isBuyerMaker':true,'isBestMatch':true},{'id':16522675,'price':'0.00891700','qty':'2.21000000','time':1538547709960,'isBuyerMaker':true,'isBestMatch':true},{'id':16522676,'price':'0.00891500','qty':'6.24000000','time':1538547709979,'isBuyerMaker':true,'isBestMatch':true},{'id':16522677,'price':'0.00891200','qty':'0.22000000','time':1538547718878,'isBuyerMaker':true,'isBestMatch':true},{'id':16522678,'price':'0.00891500','qty':'10.81000000','time':1538547726829,'isBuyerMaker':true,'isBestMatch':true},{'id':16522679,'price':'0.00891300','qty':'16.82000000','time':1538547726843,'isBuyerMaker':true,'isBestMatch':true},{'id':16522680,'price':'0.00893100','qty':'1.07000000','time':1538547734128,'isBuyerMaker':true,'isBestMatch':true},{'id':16522681,'price':'0.00893100','qty':'0.56000000','time':1538547734134,'isBuyerMaker':true,'isBestMatch':true},{'id':16522682,'price':'0.00892000','qty':'5.70000000','time':1538547738493,'isBuyerMaker':false,'isBestMatch':true},{'id':16522683,'price':'0.00891500','qty':'0.51000000','time':1538547744899,'isBuyerMaker':true,'isBestMatch':true},{'id':16522684,'price':'0.00891800','qty':'0.34000000','time':1538547749581,'isBuyerMaker':true,'isBestMatch':true},{'id':16522685,'price':'0.00891800','qty':'0.01000000','time':1538547749730,'isBuyerMaker':true,'isBestMatch':true},{'id':16522686,'price':'0.00892100','qty':'6.75000000','time':1538547759902,'isBuyerMaker':true,'isBestMatch':true},{'id':16522687,'price':'0.00892000','qty':'18.48000000','time':1538547765317,'isBuyerMaker':true,'isBestMatch':true},{'id':16522688,'price':'0.00892000','qty':'12.37000000','time':1538547766926,'isBuyerMaker':false,'isBestMatch':true},{'id':16522689,'price':'0.00891900','qty':'9.16000000','time':1538547766939,'isBuyerMaker':true,'isBestMatch':true},{'id':16522690,'price':'0.00891800','qty':'1.05000000','time':1538547766966,'isBuyerMaker':true,'isBestMatch':true},{'id':16522691,'price':'0.00891200','qty':'4.69000000','time':1538547778873,'isBuyerMaker':false,'isBestMatch':true},{'id':16522692,'price':'0.00891700','qty':'3.52000000','time':1538547778885,'isBuyerMaker':false,'isBestMatch':true},{'id':16522693,'price':'0.00891800','qty':'12.77000000','time':1538547778904,'isBuyerMaker':false,'isBestMatch':true},{'id':16522694,'price':'0.00891200','qty':'15.78000000','time':1538547783702,'isBuyerMaker':false,'isBestMatch':true},{'id':16522695,'price':'0.00891600','qty':'13.83000000','time':1538547783718,'isBuyerMaker':false,'isBestMatch':true},{'id':16522696,'price':'0.00891900','qty':'1.18000000','time':1538547783916,'isBuyerMaker':false,'isBestMatch':true},{'id':16522697,'price':'0.00891900','qty':'3.00000000','time':1538547784645,'isBuyerMaker':false,'isBestMatch':true},{'id':16522698,'price':'0.00891500','qty':'6.95000000','time':1538547798312,'isBuyerMaker':false,'isBestMatch':true},{'id':16522699,'price':'0.00891600','qty':'2.00000000','time':1538547799086,'isBuyerMaker':false,'isBestMatch':true},{'id':16522700,'price':'0.00891400','qty':'20.55000000','time':1538547801393,'isBuyerMaker':false,'isBestMatch':true},{'id':16522701,'price':'0.00891600','qty':'11.58000000','time':1538547801411,'isBuyerMaker':false,'isBestMatch':true},{'id':16522702,'price':'0.00891600','qty':'2.00000000','time':1538547801458,'isBuyerMaker':true,'isBestMatch':true},{'id':16522703,'price':'0.00891400','qty':'0.14000000','time':1538547804154,'isBuyerMaker':true,'isBestMatch':true},{'id':16522704,'price':'0.00891600','qty':'14.00000000','time':1538547807339,'isBuyerMaker':false,'isBestMatch':true},{'id':16522705,'price':'0.00891600','qty':'2.03000000','time':1538547807355,'isBuyerMaker':false,'isBestMatch':true},{'id':16522706,'price':'0.00891800','qty':'0.92000000','time':1538547812319,'isBuyerMaker':false,'isBestMatch':true},{'id':16522707,'price':'0.00891500','qty':'1.70000000','time':1538547815304,'isBuyerMaker':true,'isBestMatch':true},{'id':16522708,'price':'0.00891500','qty':'1.70000000','time':1538547816322,'isBuyerMaker':true,'isBestMatch':true},{'id':16522709,'price':'0.00891500','qty':'1.70000000','time':1538547816593,'isBuyerMaker':true,'isBestMatch':true},{'id':16522710,'price':'0.00891500','qty':'1.70000000','time':1538547816864,'isBuyerMaker':true,'isBestMatch':true},{'id':16522711,'price':'0.00891500','qty':'1.70000000','time':1538547817323,'isBuyerMaker':true,'isBestMatch':true},{'id':16522712,'price':'0.00892200','qty':'0.20000000','time':1538547818821,'isBuyerMaker':false,'isBestMatch':true},{'id':16522713,'price':'0.00892200','qty':'0.20000000','time':1538547819329,'isBuyerMaker':false,'isBestMatch':true},{'id':16522714,'price':'0.00892200','qty':'0.20000000','time':1538547819607,'isBuyerMaker':false,'isBestMatch':true},{'id':16522715,'price':'0.00892900','qty':'0.20000000','time':1538547819875,'isBuyerMaker':false,'isBestMatch':true},{'id':16522716,'price':'0.00892900','qty':'0.20000000','time':1538547820155,'isBuyerMaker':false,'isBestMatch':true},{'id':16522717,'price':'0.00891700','qty':'1.15000000','time':1538547824822,'isBuyerMaker':true,'isBestMatch':true},{'id':16522718,'price':'0.00892000','qty':'13.36000000','time':1538547829020,'isBuyerMaker':false,'isBestMatch':true},{'id':16522719,'price':'0.00892400','qty':'1.72000000','time':1538547829031,'isBuyerMaker':false,'isBestMatch':true},{'id':16522720,'price':'0.00892400','qty':'0.01000000','time':1538547829045,'isBuyerMaker':false,'isBestMatch':true},{'id':16522721,'price':'0.00892400','qty':'6.53000000','time':1538547829045,'isBuyerMaker':false,'isBestMatch':true},{'id':16522722,'price':'0.00891700','qty':'1.39000000','time':1538547831081,'isBuyerMaker':true,'isBestMatch':true},{'id':16522723,'price':'0.00892000','qty':'9.25000000','time':1538547840935,'isBuyerMaker':true,'isBestMatch':true},{'id':16522724,'price':'0.00891800','qty':'2.20000000','time':1538547862126,'isBuyerMaker':true,'isBestMatch':true},{'id':16522725,'price':'0.00891800','qty':'11.98000000','time':1538547862181,'isBuyerMaker':true,'isBestMatch':true},{'id':16522726,'price':'0.00891700','qty':'1.53000000','time':1538547869262,'isBuyerMaker':true,'isBestMatch':true},{'id':16522727,'price':'0.00891900','qty':'4.12000000','time':1538547876560,'isBuyerMaker':false,'isBestMatch':true},{'id':16522728,'price':'0.00892100','qty':'19.05000000','time':1538547876585,'isBuyerMaker':false,'isBestMatch':true},{'id':16522729,'price':'0.00892000','qty':'14.14000000','time':1538547889234,'isBuyerMaker':true,'isBestMatch':true},{'id':16522730,'price':'0.00891900','qty':'4.95000000','time':1538547889250,'isBuyerMaker':true,'isBestMatch':true},{'id':16522731,'price':'0.00891900','qty':'7.67000000','time':1538547902224,'isBuyerMaker':false,'isBestMatch':true},{'id':16522732,'price':'0.00891600','qty':'6.25000000','time':1538547902245,'isBuyerMaker':true,'isBestMatch':true},{'id':16522733,'price':'0.00891500','qty':'11.67000000','time':1538547902263,'isBuyerMaker':true,'isBestMatch':true},{'id':16522734,'price':'0.00892100','qty':'1.41000000','time':1538547915787,'isBuyerMaker':false,'isBestMatch':true},{'id':16522735,'price':'0.00892100','qty':'0.48000000','time':1538547916059,'isBuyerMaker':false,'isBestMatch':true},{'id':16522736,'price':'0.00892000','qty':'2.40000000','time':1538547916406,'isBuyerMaker':true,'isBestMatch':true},{'id':16522737,'price':'0.00891600','qty':'19.05000000','time':1538547921489,'isBuyerMaker':true,'isBestMatch':true},{'id':16522738,'price':'0.00891700','qty':'18.32000000','time':1538547930386,'isBuyerMaker':true,'isBestMatch':true},{'id':16522739,'price':'0.00891400','qty':'0.31000000','time':1538547939460,'isBuyerMaker':true,'isBestMatch':true},{'id':16522740,'price':'0.00892100','qty':'4.35000000','time':1538547948539,'isBuyerMaker':false,'isBestMatch':true},{'id':16522741,'price':'0.00892100','qty':'4.57000000','time':1538547949556,'isBuyerMaker':false,'isBestMatch':true},{'id':16522742,'price':'0.00892100','qty':'4.34000000','time':1538547949556,'isBuyerMaker':false,'isBestMatch':true},{'id':16522743,'price':'0.00891900','qty':'7.45000000','time':1538547956210,'isBuyerMaker':false,'isBestMatch':true},{'id':16522744,'price':'0.00892300','qty':'0.12000000','time':1538547957914,'isBuyerMaker':false,'isBestMatch':true},{'id':16522745,'price':'0.00892300','qty':'0.17000000','time':1538547959041,'isBuyerMaker':false,'isBestMatch':true},{'id':16522746,'price':'0.00892400','qty':'0.16000000','time':1538547971915,'isBuyerMaker':false,'isBestMatch':true},{'id':16522747,'price':'0.00892100','qty':'2.24000000','time':1538547973829,'isBuyerMaker':false,'isBestMatch':true},{'id':16522748,'price':'0.00891900','qty':'2.22000000','time':1538547973839,'isBuyerMaker':true,'isBestMatch':true},{'id':16522749,'price':'0.00891700','qty':'1.36000000','time':1538547987021,'isBuyerMaker':false,'isBestMatch':true},{'id':16522750,'price':'0.00892000','qty':'3.27000000','time':1538547987027,'isBuyerMaker':false,'isBestMatch':true},{'id':16522751,'price':'0.00892100','qty':'1.29000000','time':1538547987053,'isBuyerMaker':false,'isBestMatch':true},{'id':16522752,'price':'0.00892100','qty':'1.07000000','time':1538547999528,'isBuyerMaker':false,'isBestMatch':true},{'id':16522753,'price':'0.00891900','qty':'1.41000000','time':1538547999550,'isBuyerMaker':true,'isBestMatch':true},{'id':16522754,'price':'0.00892200','qty':'0.93000000','time':1538548008321,'isBuyerMaker':false,'isBestMatch':true},{'id':16522755,'price':'0.00891800','qty':'0.76000000','time':1538548013381,'isBuyerMaker':true,'isBestMatch':true},{'id':16522756,'price':'0.00892000','qty':'25.87000000','time':1538548015194,'isBuyerMaker':false,'isBestMatch':true},{'id':16522757,'price':'0.00891800','qty':'11.23000000','time':1538548015202,'isBuyerMaker':true,'isBestMatch':true},{'id':16522758,'price':'0.00891800','qty':'0.76000000','time':1538548015734,'isBuyerMaker':false,'isBestMatch':true},{'id':16522759,'price':'0.00891800','qty':'0.73000000','time':1538548035108,'isBuyerMaker':true,'isBestMatch':true},{'id':16522760,'price':'0.00891400','qty':'1.69000000','time':1538548035299,'isBuyerMaker':true,'isBestMatch':true},{'id':16522761,'price':'0.00890800','qty':'0.45000000','time':1538548041566,'isBuyerMaker':true,'isBestMatch':true},{'id':16522762,'price':'0.00891200','qty':'22.22000000','time':1538548048045,'isBuyerMaker':false,'isBestMatch':true},{'id':16522763,'price':'0.00891600','qty':'4.83000000','time':1538548048066,'isBuyerMaker':false,'isBestMatch':true},{'id':16522764,'price':'0.00891600','qty':'16.46000000','time':1538548048086,'isBuyerMaker':false,'isBestMatch':true},{'id':16522765,'price':'0.00891600','qty':'3.00000000','time':1538548052074,'isBuyerMaker':false,'isBestMatch':true},{'id':16522766,'price':'0.00891600','qty':'3.00000000','time':1538548053583,'isBuyerMaker':false,'isBestMatch':true},{'id':16522767,'price':'0.00891400','qty':'2.78000000','time':1538548056207,'isBuyerMaker':false,'isBestMatch':true},{'id':16522768,'price':'0.00891400','qty':'2.49000000','time':1538548056600,'isBuyerMaker':false,'isBestMatch':true},{'id':16522769,'price':'0.00891400','qty':'0.34000000','time':1538548059486,'isBuyerMaker':false,'isBestMatch':true},{'id':16522770,'price':'0.00891400','qty':'13.81000000','time':1538548059632,'isBuyerMaker':false,'isBestMatch':true},{'id':16522771,'price':'0.00891500','qty':'19.51000000','time':1538548059742,'isBuyerMaker':false,'isBestMatch':true},{'id':16522772,'price':'0.00891600','qty':'0.47000000','time':1538548059971,'isBuyerMaker':false,'isBestMatch':true},{'id':16522773,'price':'0.00891200','qty':'0.21000000','time':1538548062731,'isBuyerMaker':true,'isBestMatch':true},{'id':16522774,'price':'0.00891500','qty':'13.56000000','time':1538548078871,'isBuyerMaker':false,'isBestMatch':true},{'id':16522775,'price':'0.00891300','qty':'16.14000000','time':1538548078890,'isBuyerMaker':true,'isBestMatch':true},{'id':16522776,'price':'0.00891100','qty':'10.00000000','time':1538548088532,'isBuyerMaker':true,'isBestMatch':true},{'id':16522777,'price':'0.00891600','qty':'1.66000000','time':1538548102741,'isBuyerMaker':false,'isBestMatch':true},{'id':16522778,'price':'0.00891700','qty':'0.17000000','time':1538548106071,'isBuyerMaker':false,'isBestMatch':true},{'id':16522779,'price':'0.00891700','qty':'0.12000000','time':1538548106661,'isBuyerMaker':false,'isBestMatch':true},{'id':16522780,'price':'0.00891300','qty':'0.05000000','time':1538548117584,'isBuyerMaker':true,'isBestMatch':true},{'id':16522781,'price':'0.00891500','qty':'28.36000000','time':1538548117938,'isBuyerMaker':false,'isBestMatch':true},{'id':16522782,'price':'0.00891300','qty':'15.85000000','time':1538548117951,'isBuyerMaker':true,'isBestMatch':true},{'id':16522783,'price':'0.00891300','qty':'0.05000000','time':1538548119731,'isBuyerMaker':false,'isBestMatch':true},{'id':16522784,'price':'0.00891300','qty':'0.03000000','time':1538548121493,'isBuyerMaker':true,'isBestMatch':true},{'id':16522785,'price':'0.00891600','qty':'0.34000000','time':1538548125518,'isBuyerMaker':true,'isBestMatch':true},{'id':16522786,'price':'0.00891600','qty':'1.60000000','time':1538548128654,'isBuyerMaker':true,'isBestMatch':true},{'id':16522787,'price':'0.00891400','qty':'16.05000000','time':1538548128677,'isBuyerMaker':true,'isBestMatch':true},{'id':16522788,'price':'0.00891500','qty':'6.62000000','time':1538548145053,'isBuyerMaker':false,'isBestMatch':true},{'id':16522789,'price':'0.00891400','qty':'4.95000000','time':1538548145072,'isBuyerMaker':true,'isBestMatch':true},{'id':16522790,'price':'0.00891600','qty':'0.33000000','time':1538548145096,'isBuyerMaker':false,'isBestMatch':true}]";
        //    responce.RecentTrades = JsonConvert.DeserializeObject<List<RecentTrade>>(dummyResponce);
        //    responce.ReturnCode = enResponseCode.Success;
        //    return Ok(responce);
        //}

        //[HttpGet("GetOrderBook", Name = "Active Order")] //Binance https://api.binance.com//api/v1/depth?symbol=LTCBTC
        //public IActionResult GetOrderBook(OrderBookRequest request)
        //{
        //    dummyResponce = "{'lastUpdateId':114753795,'bids':[['0.00893100','21.40000000',[]],['0.00893000','55.90000000',[]],['0.00892900','10.00000000',[]],['0.00892700','0.87000000',[]],['0.00891800','61.00000000',[]],['0.00891500','40.01000000',[]],['0.00890800','66.00000000',[]],['0.00890700','0.17000000',[]],['0.00890500','3.75000000',[]],['0.00890400','1.22000000',[]],['0.00890200','12.58000000',[]],['0.00890000','1.47000000',[]],['0.00889900','4.70000000',[]],['0.00889800','133.12000000',[]],['0.00889700','183.25000000',[]],['0.00889400','1.00000000',[]],['0.00889300','20.12000000',[]],['0.00889100','20.00000000',[]],['0.00888900','38.23000000',[]],['0.00888500','37.81000000',[]],['0.00888100','3.17000000',[]],['0.00888000','31.82000000',[]],['0.00887800','1.19000000',[]],['0.00887200','45.00000000',[]],['0.00887100','1.02000000',[]],['0.00886800','0.13000000',[]],['0.00886700','4.91000000',[]],['0.00886600','0.56000000',[]],['0.00885600','11.30000000',[]],['0.00885500','19.12000000',[]],['0.00885300','20.91000000',[]],['0.00885100','1.82000000',[]],['0.00885000','2.30000000',[]],['0.00884900','0.31000000',[]],['0.00884800','6.20000000',[]],['0.00884700','0.40000000',[]],['0.00884500','0.59000000',[]],['0.00884300','678.99000000',[]],['0.00884200','11.52000000',[]],['0.00884100','0.22000000',[]],['0.00884000','3.82000000',[]],['0.00883900','0.59000000',[]],['0.00883800','0.22000000',[]],['0.00883700','1.02000000',[]],['0.00883600','2.65000000',[]],['0.00883500','15.96000000',[]],['0.00883400','7.23000000',[]],['0.00883300','3.35000000',[]],['0.00883200','6.25000000',[]],['0.00883100','16.79000000',[]],['0.00883000','109.22000000',[]],['0.00882900','0.50000000',[]],['0.00882800','0.70000000',[]],['0.00882700','0.58000000',[]],['0.00882600','4.17000000',[]],['0.00882500','6.20000000',[]],['0.00882400','564.42000000',[]],['0.00882300','4.93000000',[]],['0.00882200','89.24000000',[]],['0.00882100','3.13000000',[]],['0.00882000','32.58000000',[]],['0.00881900','9.11000000',[]],['0.00881800','1.16000000',[]],['0.00881700','2.70000000',[]],['0.00881600','3.36000000',[]],['0.00881500','1.85000000',[]],['0.00881400','1.99000000',[]],['0.00881300','9.13000000',[]],['0.00881100','8.40000000',[]],['0.00881000','0.67000000',[]],['0.00880900','0.70000000',[]],['0.00880800','0.68000000',[]],['0.00880600','1.12000000',[]],['0.00880500','0.18000000',[]],['0.00880300','0.70000000',[]],['0.00880100','1.07000000',[]],['0.00880000','7.71000000',[]],['0.00879900','14.50000000',[]],['0.00879700','1.69000000',[]],['0.00879600','0.14000000',[]],['0.00878800','0.53000000',[]],['0.00878700','5.62000000',[]],['0.00878600','0.24000000',[]],['0.00878500','27.28000000',[]],['0.00878300','0.83000000',[]],['0.00878200','12.48000000',[]],['0.00878100','0.28000000',[]],['0.00878000','1.70000000',[]],['0.00877900','2.47000000',[]],['0.00877700','10.60000000',[]],['0.00877600','0.67000000',[]],['0.00877500','3.07000000',[]],['0.00877400','0.81000000',[]],['0.00877200','0.55000000',[]],['0.00877100','9.35000000',[]],['0.00877000','0.44000000',[]],['0.00876900','0.31000000',[]],['0.00876800','0.64000000',[]],['0.00876700','1.28000000',[]],['0.00876500','0.57000000',[]]],'asks':[['0.00893700','13.73000000',[]],['0.00893800','3.86000000',[]],['0.00893900','7.67000000',[]],['0.00895400','0.25000000',[]],['0.00895700','0.40000000',[]],['0.00895900','17.80000000',[]],['0.00896000','26.00000000',[]],['0.00896200','2.18000000',[]],['0.00896500','57.52000000',[]],['0.00896700','0.46000000',[]],['0.00896800','0.12000000',[]],['0.00897000','17.28000000',[]],['0.00897300','7.56000000',[]],['0.00897400','22.13000000',[]],['0.00897500','74.65000000',[]],['0.00897600','0.63000000',[]],['0.00897800','0.12000000',[]],['0.00898000','0.38000000',[]],['0.00898100','3.76000000',[]],['0.00898500','50.79000000',[]],['0.00898600','1.00000000',[]],['0.00898700','1.79000000',[]],['0.00898800','0.12000000',[]],['0.00898900','23.80000000',[]],['0.00899000','183.25000000',[]],['0.00899300','9.77000000',[]],['0.00899400','0.70000000',[]],['0.00899700','13.82000000',[]],['0.00899800','0.12000000',[]],['0.00900000','3.92000000',[]],['0.00900100','0.29000000',[]],['0.00900200','0.96000000',[]],['0.00900300','29.86000000',[]],['0.00900600','0.59000000',[]],['0.00900800','0.12000000',[]],['0.00900900','0.38000000',[]],['0.00901000','102.46000000',[]],['0.00901100','555.62000000',[]],['0.00901700','0.42000000',[]],['0.00901800','0.48000000',[]],['0.00902000','2.81000000',[]],['0.00902800','0.15000000',[]],['0.00902900','20.62000000',[]],['0.00903000','90.57000000',[]],['0.00903100','1.26000000',[]],['0.00903200','39.39000000',[]],['0.00903300','44.79000000',[]],['0.00903400','37.90000000',[]],['0.00903500','9.91000000',[]],['0.00903600','26.98000000',[]],['0.00903800','0.58000000',[]],['0.00903900','0.17000000',[]],['0.00904000','1.74000000',[]],['0.00904100','0.92000000',[]],['0.00904500','1.39000000',[]],['0.00904600','0.14000000',[]],['0.00904800','18.17000000',[]],['0.00905000','2.24000000',[]],['0.00905100','6.93000000',[]],['0.00905300','0.55000000',[]],['0.00905400','5.76000000',[]],['0.00905600','0.67000000',[]],['0.00905800','6.85000000',[]],['0.00906000','1.84000000',[]],['0.00906300','1.90000000',[]],['0.00906400','636.74000000',[]],['0.00906600','0.66000000',[]],['0.00906800','2.03000000',[]],['0.00907000','3.50000000',[]],['0.00907700','0.69000000',[]],['0.00907800','0.17000000',[]],['0.00907900','5.20000000',[]],['0.00908000','1.54000000',[]],['0.00908500','1.19000000',[]],['0.00908600','30.26000000',[]],['0.00909100','95.22000000',[]],['0.00909800','1.10000000',[]],['0.00909900','6.94000000',[]],['0.00910000','19.63000000',[]],['0.00910100','1.17000000',[]],['0.00910300','1.62000000',[]],['0.00910400','18.70000000',[]],['0.00910500','0.81000000',[]],['0.00910700','0.98000000',[]],['0.00910800','2.01000000',[]],['0.00911300','0.82000000',[]],['0.00911400','0.54000000',[]],['0.00911700','49.76000000',[]],['0.00911800','34.22000000',[]],['0.00912000','37.98000000',[]],['0.00912300','2.25000000',[]],['0.00912400','89.74000000',[]],['0.00912600','1.60000000',[]],['0.00912700','0.93000000',[]],['0.00912800','0.17000000',[]],['0.00912900','0.49000000',[]],['0.00913000','1.91000000',[]],['0.00913200','0.87000000',[]],['0.00913400','2.12000000',[]],['0.00913600','18.90000000',[]]]}";
        //    OrderBookResponce responce = new OrderBookResponce();
        //    responce.OrderBook = JsonConvert.DeserializeObject<OrderBook>(dummyResponce);
        //    responce.ReturnCode = enResponseCode.Success;
        //    return Ok(responce);
        //}

        //[HttpPost("GetWalletBalance")]
        //public IActionResult GetWalletBalance([FromHeader, BindRequired] string ApiKey)
        //{
        //    WalletBalanceResponce response = new WalletBalanceResponce();

        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    dummyResponce = "{response : [{ 'type':'deposit', 'currency':'btc', 'amount':'0.0', 'available':'0.0' },{ 'type':'deposit', 'currency':'usd', 'amount':'1.0', 'available':'1.0' },{ 'type':'exchange', 'currency':'btc', 'amount':'1', 'available':'1' },{ 'type':'exchange', 'currency':'usd', 'amount':'1', 'available':'1' },{ 'type':'trading', 'currency':'btc', 'amount':'1', 'available':'1' },{ 'type':'trading', 'currency':'usd', 'amount':'1', 'available':'1' }]}";

        //    response = JsonConvert.DeserializeObject<WalletBalanceResponce>(dummyResponce);
        //    return Ok(response);
        //}

        //[HttpPost("GetTradeBalance")]
        //public IActionResult GetTradeBalance([FromHeader, BindRequired] string ApiKey)
        //{
        //    GetPairBalanceResponce response = new GetPairBalanceResponce();

        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }

        //    dummyResponce = "{response : {'Currency':'BTC','Balance':4.21549076,'Available':4.21549076,'Pending':0,'CryptoAddress':'1MacMr6715hjds342dXuLqXcju6fgwHA31','Requested':false,'Uuid':null}}";

        //    response = JsonConvert.DeserializeObject<GetPairBalanceResponce>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        //[HttpPost("GetTradeLedger")]
        //public IActionResult GetTradeLedger([FromHeader, BindRequired] string ApiKey, TradeLedgerRequest request)
        //{
        //    TradeLedgerResponce response = new TradeLedgerResponce();

        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "{response : {'symbol':'BNBBTC','id':28457,'orderId':100234,'price':'4.00000100','qty':'12.00000000','commission':'10.10000000','commissionAsset':'BNB','time':1499865549590,'isBuyer':true,'isMaker':false,'isBestMatch':true}}";

        //    response = JsonConvert.DeserializeObject<TradeLedgerResponce>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        //[HttpPost("CancelOffer")] //bitfinex https://api.bitfinex.com/v1/offer/cancel
        //public IActionResult CancelOffer([FromHeader, BindRequired] string ApiKey, CancelOfferRequest request)
        //{
        //    CancelOfferReasponce response = new CancelOfferReasponce();

        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "{response : {'id':446915287,'symbol':'btcusd','exchange':null,'price':'239.0','avg_execution_price':'0.0','side':'sell','type':'trailing stop','timestamp':'1444141982.0','is_live':true,'is_cancelled':false,'is_hidden':false,'was_forced':false,'original_amount':'1.0','remaining_amount':'1.0','executed_amount':'0.0'}}";

        //    response = JsonConvert.DeserializeObject<CancelOfferReasponce>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        //[HttpPost("ActiveCredits")] //bitfinex https://api.bitfinex.com/v1/credits or Active Offer https://api.bitfinex.com/v1/offers
        //public IActionResult ActiveCredits([FromHeader, BindRequired] string ApiKey)
        //{
        //    ActiveCreditsResponce response = new ActiveCreditsResponce();

        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "{response : { 'id':13800719, 'currency':'USD', 'rate':'31.39', 'period':2, 'direction':'lend', 'timestamp':'1444280237.0', 'is_live':true, 'is_cancelled':false, 'original_amount':'50.0', 'remaining_amount':'50.0', 'executed_amount':'0.0' }}";

        //    response = JsonConvert.DeserializeObject<ActiveCreditsResponce>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        //[HttpPost("MyTradesFunding")] //bitfinex https://api.bitfinex.com/v1/mytrades_funding
        //public IActionResult MyTradesFunding([FromHeader, BindRequired] string ApiKey, MyTradesFundingRequest request)
        //{
        //    MyTradesFundingResponce response = new MyTradesFundingResponce();
        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "{ 'rate':'0.01', 'period':30, 'amount':'1.0', 'timestamp':'1444141857.0', 'type':'Buy', 'tid':11970839, 'offer_id':446913929 }";

        //    response.MyTradesFunding = JsonConvert.DeserializeObject<MyTradesFunding>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        ////Active Funding Used in a margin position
        //[HttpPost("TakenFunds")]//bitfinex https://api.bitfinex.com/v1/taken_funds
        //public IActionResult TakenFunds([FromHeader, BindRequired] string ApiKey)
        //{
        //    MarginFundingResponce response = new MarginFundingResponce();
        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "[{'id':11576737,'position_id':944309,'currency':'USD','rate':'9.8874','period':2,'amount':'34.24603414','timestamp':'1444280948.0','auto_close':false}]";

        //    response.MarginFunding = JsonConvert.DeserializeObject<List<MarginFunding>>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        ////Active Funding Not Used in a margin position
        //[HttpPost("UnusedTakenFunds")]//bitfinex https://api.bitfinex.com/v1/unused_taken_funds
        //public IActionResult UnusedTakenFunds([FromHeader, BindRequired] string ApiKey)
        //{
        //    MarginFundingResponce response = new MarginFundingResponce();
        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "[{'id':11576737,'position_id':944309,'currency':'USD','rate':'9.8874','period':2,'amount':'34.24603414','timestamp':'1444280948.0','auto_close':false}]";

        //    response.MarginFunding = JsonConvert.DeserializeObject<List<MarginFunding>>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}

        ////Close Margin Funding 
        //[HttpPost("CloseMarginFunding")]//bitfinex https://api.bitfinex.com/v1/funding/close?swap_id=11576737
        //public IActionResult CloseMarginFunding([FromHeader, BindRequired] string ApiKey, CloseMarginFundingRequest request)
        //{
        //    CloseMarginFundingResponce response = new CloseMarginFundingResponce();
        //    if (string.IsNullOrEmpty(ApiKey))
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (ApiKey != "123456")
        //    {
        //        response.ReturnCode = enResponseCode.Fail;
        //        response.ReturnMsg = "Unauthorize ApiKey";
        //        return Ok(response);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    dummyResponce = "{'id':11576737,'position_id':944309,'currency':'USD','rate':'9.8874','period':2,'amount':'34.24603414','timestamp':'1444280948.0','auto_close':false}";

        //    response.MarginFunding = JsonConvert.DeserializeObject<MarginFunding>(dummyResponce);
        //    response.ReturnCode = enResponseCode.Success;
        //    return Ok(response);
        //}
        //#endregion

        //#region "uday method"

        //[HttpPost("GetAssetInformation")]
        //public ActionResult GetAssetInformation([FromBody]GetAssetInfoRequest Request)
        //{
        //    try
        //    {
        //        //For Testing Purpose
        //        string dummyreposne = "{response : [{'asset_name':'BCH',asset_detail :{'altname':'BCH','aclass':'currency','decimals':10,'display_decimals':5}},{'asset_name':'BCH',asset_detail :{'altname':'BCH','aclass':'currency','decimals':10,'display_decimals':5}}]}";

        //        var Response = JsonConvert.DeserializeObject<GetAssetInfoResponse>(dummyreposne);
        //        Response.ReturnCode = enResponseCode.Success;
        //        return returnDynamicResult(Response);
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("GetMarketSummary")]
        //public ActionResult GetMarketSummary([FromBody]GetMarketSummaryRequest Request)
        //{
        //    //For Testing Purpose
        //    string dummyreposne = "{response : [{'pair_name':'BCHEUR',pair_detail :{'high':0.00892105,'low':0.00859275,'volume':24045.37035813,'last_price':0.00861713,'bid':0.00861618,'ask':0.00861713,'open_buy_order':37,'open_sell_order':2080,prev_day:0.00880495}},{'pair_name':'BCHEUR',pair_detail :{'high':0.00892105,'low':0.00859275,'volume':24045.37035813,'last_price':0.00861713,'bid':0.00861618,'ask':0.00861713,'open_buy_order':37,'open_sell_order':2080,prev_day:0.00880495}}]}";

        //    var Response = JsonConvert.DeserializeObject<GetMarketSummaryResponse>(dummyreposne);
        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetRecentTrades")]
        //public ActionResult GetRecentTrades([FromBody]GetRecentTradesRequest Request)
        //{
        //    //For Testing Purpose
        //    string dummyreposne = "{'pair_name':'ltcusd','recent_trades':[{'timestamp':1538399543,'tid':300697552,'price':'60.94','amount':'7.06677169','exchange':'bitfinex','type':'sell'},{'timestamp':1538399543,'tid':300697551,'price':'60.94','amount':'0.53322831','exchange':'bitfinex','type':'sell'},{'timestamp':1538399540,'tid':300697546,'price':'60.94','amount':'0.6','exchange':'bitfinex','type':'sell'},{'timestamp':1538399538,'tid':300697542,'price':'60.94','amount':'1.0','exchange':'bitfinex','type':'sell'},{'timestamp':1538399524,'tid':300697512,'price':'60.98','amount':'2.13174163','exchange':'bitfinex','type':'sell'},{'timestamp':1538399506,'tid':300697486,'price':'60.963','amount':'0.83','exchange':'bitfinex','type':'sell'}]}";

        //    var Response = JsonConvert.DeserializeObject<GetRecentTradesResponse>(dummyreposne);
        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}
        //[HttpPost("CreateMultipleOrder")]
        //public ActionResult CreateMultipleOrder([FromBody]List<CreateOrderRequest> Request)
        //{
        //    //Do Process for CreateMultipleOrder
        //    //For Testing Purpose
        //    CreateMultipleOrderResponse Response = new CreateMultipleOrderResponse();
        //    Response.response = new List<CreateOrderInfo>()
        //    {
        //       //new CreateOrderInfo(){ order_id = 1000001,pair_name = "ltcusd",price = 10,side = "buy",type = "stop-loss",volume = 10},
        //       //new CreateOrderInfo(){ order_id = 1000001,pair_name = "BCHEUR",price = 20,side = "sell",type = "take-profit",volume = 5},
        //       //new CreateOrderInfo(){ order_id = 1000001,pair_name = "ltcusd",price = 20.25M,side = "buy",type = "stop-loss-profit",volume = 100},
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("CancelMultipleOrder")]
        //public ActionResult CancelMultipleOrder([FromBody]List<CancelOrderRequest> Request)
        //{
        //    //For Testing Purpose
        //    CancelMultipleOrderResponse Response = new CancelMultipleOrderResponse();
        //    Response.response = new List<CancelOrderInfo>()
        //    {
        //        new CancelOrderInfo(){order_id = 100001,message = "Order Cancelled"},
        //        new CancelOrderInfo(){order_id = 100002,message = "Order Cancelled"},
        //        new CancelOrderInfo(){order_id = 100003,message = "Order Cancelled"},
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetOpenOrders")]
        //public ActionResult GetOpenOrders()
        //{
        //    //For Testing Purpose
        //    GetOpenOrderResponse Response = new GetOpenOrderResponse();
        //    Response.response = new List<GetOpenOrderInfo>()
        //    {
        //       new GetOpenOrderInfo() {order_id=10001,pair_name="ltcusd",price=10.20M,side="buy",timestamp=152424515,type="market-limit",volume=10},
        //       new GetOpenOrderInfo() {order_id=10001,pair_name="BCHEUR",price=10.20M,side="sell",timestamp=152424515,type="stop-loss",volume=10},
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetActiveOrder")]  //GetActiveOrder
        //public ActionResult GetActiveOrder()
        //{
        //    //For Testing Purpose
        //    long MemberID = 0;
        //    var dataresponse = _frontTrnService.GetActiveOrder(MemberID);
        //    GetActiveOrderResponse Response = new GetActiveOrderResponse();
        //    if (dataresponse.Count == 0)
        //    {
        //        Response.response = new List<GetActiveOrderInfo>()
        //        {
        //           new GetActiveOrderInfo() {order_id=10001,pair_name="ltcusd",price=10.20M,side="buy",timestamp=152424515,type="market-limit",volume=10},
        //           new GetActiveOrderInfo() {order_id=10001,pair_name="BCHEUR",price=10.20M,side="sell",timestamp=152424515,type="stop-loss",volume=10},
        //        };
        //    }

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetOrderHistory")]
        //public ActionResult GetOrderHistory()
        //{
        //    //For Testing Purpose
        //    GetOrderHistoryResponse Response = new GetOrderHistoryResponse();
        //    Response.response = new List<GetOrderHistoryInfo>()
        //    {
        //       new GetOrderHistoryInfo() {order_id=10001,pair_name="ltcusd",price=10.20M,side="buy",timestamp=152424515,type="market-limit",volume=10,is_live=true},
        //       new GetOrderHistoryInfo() {order_id=10001,pair_name="BCHEUR",price=10.20M,side="sell",timestamp=152424515,type="stop-loss",volume=10,is_live=false},
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetTradeHistory")]
        //public ActionResult GetTradeHistory()
        //{
        //    //For Testing Purpose
        //    GetTradeHistoryResponse Response = new GetTradeHistoryResponse();
        //    Response.response = new List<GetTradeHistoryInfo>()
        //    {
        //       new GetTradeHistoryInfo() {order_id=446913929,tid=11970839,type="buy",price=246.94M,amount=1.0M,fee_currency="USD",fee_amount=-0.49388M,timestamp=1444141857 },
        //       new GetTradeHistoryInfo() {order_id=446913930,tid=11970839,type="sell",price=250.94M,amount=1.5M,fee_currency="USD",fee_amount=-0.49388M,timestamp=1444141858 },
        //       new GetTradeHistoryInfo() {order_id=446913931,tid=11970839,type="buy",price=251.94M,amount=2.1M,fee_currency="USD",fee_amount=-0.49389M,timestamp=1444141859 },
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("CreateMargingTredingOffer")]
        //public ActionResult CreateMargingTredingOffer([FromBody]CreateMarginTradingRequest Request)
        //{
        //    //For Testing Purpose
        //    CreateMarginTradingResponse Response = new CreateMarginTradingResponse();
        //    Response.response = new CreateMarginTradingInfo()
        //    {
        //        offer_id = 13800585,
        //        currency = "USD",
        //        rate = 20.0M,
        //        period = 2,
        //        direction = "lend",
        //        is_live = true,
        //        is_cancelled = false,
        //        timestamp = 1444279698,
        //        original_amount = 50.0M,
        //        executed_amount = 0.0M,
        //        remaining_amount = 50.0M
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetMargingTradingOfferStatus")]
        //public ActionResult GetMargingTradingOfferStatus([FromBody]MargingTradingOfferStatusRequest Request)
        //{
        //    //For Testing Purpose
        //    MargingTradingOfferStatusResponse Response = new MargingTradingOfferStatusResponse();
        //    Response.response = new MargingTradingOfferStatusInfo()
        //    {
        //        offer_id = 13800585,
        //        currency = "USD",
        //        rate = 20.0M,
        //        period = 2,
        //        direction = "lend",
        //        is_live = true,
        //        is_cancelled = false,
        //        timestamp = 1444279698,
        //        original_amount = 50.0M,
        //        executed_amount = 0.0M,
        //        remaining_amount = 50.0M
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("GetMarginFundingTotalTakenFunds")]
        //public ActionResult GetMarginFundingTotalTakenFunds()
        //{
        //    //For Testing Purpose
        //    GetMarginFundingTotalTakenFundsResponse Response = new GetMarginFundingTotalTakenFundsResponse();
        //    Response.resposne = new List<GetMarginFundingTotalTakenFundsInfo>()
        //    {
        //        new GetMarginFundingTotalTakenFundsInfo() {position_pair="BTCUSD",total_swaps=34.24603414M},
        //        new GetMarginFundingTotalTakenFundsInfo() {position_pair="BCHUSD",total_swaps=30.24603414M}
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("MarginTradingBasketManage")]
        //public ActionResult MarginTradingBasketManage([FromBody]MarginTradingBasketManageRequest Request)
        //{
        //    //For Testing Purpose
        //    MarginTradingBasketManageResponse Response = new MarginTradingBasketManageResponse();
        //    Response.response = new MarginTradingBasketManageInfo()
        //    {
        //        message = "Basket btc_btu success"
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}

        //[HttpPost("MarginTradingClosePosition")]
        //public ActionResult MarginTradingClosePosition([FromBody]MarginTradingClosePositionRequest Request)
        //{
        //    //For Testing Purpose
        //    MarginTradingClosePositionResponse Response = new MarginTradingClosePositionResponse();
        //    Response.response = new MarginTradingClosePositionInfo()
        //    {
        //        message = "test",
        //        order = new MarginTradingOrder(),
        //        position = new MarginTradingPosition()
        //    };

        //    Response.ReturnCode = enResponseCode.Success;
        //    return returnDynamicResult(Response);
        //}
    }
}