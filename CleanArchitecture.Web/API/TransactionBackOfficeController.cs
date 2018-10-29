using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Transaction.BackOffice;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Web.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class TransactionBackOfficeController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IBackOfficeTrnService _backOfficeService;
        private readonly IFrontTrnService _frontTrnService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBasePage _basePage;

        public TransactionBackOfficeController(
            ILogger<TransactionController> logger,
            IBackOfficeTrnService backOfficeService,
            IFrontTrnService frontTrnService,
            UserManager<ApplicationUser> userManager,
            IBasePage basePage)
        {
            _logger = logger;
            _backOfficeService = backOfficeService;
            _frontTrnService = frontTrnService;
            _userManager = userManager;
            _basePage = basePage;
        }

        [HttpPost("TradingSummary")]
        public async Task<IActionResult> TradingSummary([FromBody]TradingSummaryRequest request)
        {
            TradingSummaryResponse Response = new TradingSummaryResponse();
            Int16 trnType = 999, marketType = 999, status = 999;
            long PairId = 999;
            string sCondition = "1=1";
            try
            {
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
                }
                if(request.Status != 0 && request.Status != 91 && request.Status != 92 && request.Status != 93 && request.Status != 94 && request .Status != 95)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ErrorCode = enErrorCode.InvalidStatusType;
                    return BadRequest(Response);
                }

                Response.Response = _backOfficeService.GetTradingSummary(request.MemberID,request.FromDate, request.ToDate, request.TrnNo, request.Status,request.SMSCode, PairId,trnType );

                return Ok(Response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return Ok(Response);
            }
            
        }

        [Authorize]
        [HttpPost("TradeRecon")]
        public async Task<IActionResult> TradeRecon([FromBody]TradeReconRequest request)
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
                if (request.TranNo == 0)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.TradeRecon_InvalidTransactionNo;
                    Response.ErrorCode = enErrorCode.TradeRecon_InvalidTransactionNo;
                }
                if(request.ActionType != enTradeReconActionType.Cancel)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.TradeRecon_InvalidActionType;
                    Response.ErrorCode = enErrorCode.TradeRecon_InvalidActionType;
                }
                else
                {
                    var UserId = user.Id;
                    Response = _backOfficeService.TradeRecon(request.TranNo, request.ActionMessage, UserId);
                }
                
                return Ok(Response);
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest();
            }
        }
    }
}
