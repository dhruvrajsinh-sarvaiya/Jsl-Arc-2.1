using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SignalR;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : Controller
    {
        //private SocketHub _chat;
        private readonly ILogger<SocketController> _logger;
        private readonly IMediator _mediator;
        //private readonly UserManager<ApplicationUser> _userManager;
       // private readonly ISignalRTestService _signalRTestService; 

        //public SocketController(ILogger<SocketController> logger, IMediator mediator, ISignalRTestService signalRTestService)
        public SocketController(ILogger<SocketController> logger, IMediator mediator)
        {
            _logger = logger;
            //_chat = chat;
            _mediator = mediator;
            //_signalRTestService = signalRTestService;
        }
        
        [HttpGet("BuyerBook/{Data}")]
        public async Task<IActionResult> BuyerBook(string Data)
        {
            string ReciveMethod = "";
            try
            {
                
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 3;
                model.Price = 150;

                //var temp = JsonConvert.DeserializeObject< GetBuySellBook>(Data);
                //SignalRData<GetBuySellBook> modelData = new SignalRData<GetBuySellBook>();
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 5);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 6);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(temp);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = "LTC_BTC";
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveBuyerBook";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("SellerBook/{Data}")]
        public async Task<IActionResult> SellerBook(string Data)
        {
            string ReciveMethod = "";
            try
            {
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 1755;
                model.Price = 40;

                var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 6);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 7);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = "LTC_BTC";
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveSellerBook";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("TradingHistory/{Data}")]
        public async Task<IActionResult> TradingHistory(string Data)
        {
            string ReciveMethod = "";
            try
            {
                TradeHistoryResponce model = new TradeHistoryResponce();
                model.Amount = 20;
                model.ChargeRs = 1;
                model.DateTime = DateTime.Now; ;
                model.IsCancelled = 1;
                model.PairName = "LTC_BTC";
                model.Price = 150;
                model.Status = 1;
                model.StatusText = "Success";
                model.TrnNo = 90;
                model.Type = "SELL";
                var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 7);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 8);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = "LTC_BTC";
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveTradingHistory";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("ChartData/{Data}")]
        public async Task<IActionResult> ChartData(string Data)
        {
            string ReciveMethod = "";
            try
            {
                //GetGraphResponse model = new GetGraphResponse();
                //model.ChangePer = 20;
                //model.DataDate = 20180203073000;
                //model.High = 1199;
                //model.Low = 1177;
                //model.TodayClose = 1452;
                //model.TodayOpen = 1477;
                //model.Volume = 173;

                //var temp= JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 12);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 10);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = "LTC_BTC";
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveChartData";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("MarketData/{Data}")]
        public async Task<IActionResult> MarketData(string Data)
        {
            string ReciveMethod = "";
            try
            {
                //MarketCapData model = new MarketCapData();
                //model.Change24 = 1;
                //model.ChangePer = 3;
                //model.High24 = 1153;
                //model.Low24 = 1125;
                //model.LastPrice = 1137;
                //model.Volume24 = 253;

                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 8);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 9);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = "LTC_BTC";
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveMarketData";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("OpenOrder/{Data}")]
        [Authorize]
        public async Task<IActionResult> OpenOrder(string Data)
        {
            string ReciveMethod = "";
            try
            {
                //GetBuySellBook model = new GetBuySellBook();
                //model.Amount = 3;
                //model.Price = 150;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                //var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 1);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 1);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = accessToken;
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveOpenOrder";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("OrderHistory/{Data}")]
        [Authorize]
        public async Task<IActionResult> OrderHistory(string Data)
        {
            string ReciveMethod = "";
            try
            {
                //GetBuySellBook model = new GetBuySellBook();
                //model.Amount = 3;
                //model.Price = 150;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                //var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 2);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 2);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = accessToken;
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveOrderHistory";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("TradeHistoryByUser/{Data}")]
        [Authorize]
        public async Task<IActionResult> TradeHistoryByUser(string Data)
        {
            string ReciveMethod = "";
            try
            {
                //GetBuySellBook model = new GetBuySellBook();
                //model.Amount = 3;
                //model.Price = 150;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                //var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 3);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 3);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = accessToken;
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveTradeHistory";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("BuyerSideWalletBal/{Data}")]
        [Authorize]
        public async Task<IActionResult> BuyerSideWalletBal(string Data)
        {
            string ReciveMethod = "";
            try
            {
                WalletMasterResponse model = new WalletMasterResponse();

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 10);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 4);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = accessToken;
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveBuyerSideWalletBal";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("SellerSideWalletBal/{Data}")]
        [Authorize]
        public async Task<IActionResult> SellerSideWalletBal(string Data)
        {
            string ReciveMethod = "";
            try
            {
                WalletMasterResponse model = new WalletMasterResponse();

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 11);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 5);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = accessToken;
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveSellerSideWalletBal";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        //[HttpGet("CreateTranTest")]
        //public async Task<IActionResult> CreateTranTest(long ID)
        //{
        //    string ReciveMethod = "";
        //    try
        //    {

        //        _signalRTestService.MarkTransactionHold(ID);
        //        //var accessToken = await HttpContext.GetTokenAsync("access_token");
        //        //SignalRData modelData = new SignalRData();
        //        //modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
        //        //modelData.Method = Enum.GetName(typeof(enMethodName), 11);
        //        //modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 5);
        //        //modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
        //        //modelData.Data = JsonConvert.SerializeObject(model);
        //        //modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
        //        //modelData.Parameter = accessToken;
        //        //await _mediator.Send(modelData);
        //        //ReciveMethod = "RecieveSellerSideWalletBal";
        //        return Ok(new { ReciveMethod = ReciveMethod });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

        //        return Ok();
        //    }
        //}
    }
}
