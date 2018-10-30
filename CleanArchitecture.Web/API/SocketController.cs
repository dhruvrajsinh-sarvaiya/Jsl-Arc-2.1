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
        //private readonly ISignalRTestService _signalRTestService; 

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

                GetBuySellBook temp = JsonConvert.DeserializeObject<GetBuySellBook>(Data);

                SignalRComm<GetBuySellBook> CommonData = new SignalRComm<GetBuySellBook>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerBook);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod .RecieveBuyerBook);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne );
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType .PairName );
                CommonData.Data = temp;
                CommonData.Parameter = "LTC_BTC";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerBook;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                await _mediator.Send(SendData);
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
                model.Amount = 3;
                model.Price = 150;

                GetBuySellBook temp = JsonConvert.DeserializeObject<GetBuySellBook>(Data);

                SignalRComm<GetBuySellBook> CommonData = new SignalRComm<GetBuySellBook>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.SellerBook);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerBook );
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = temp;
                CommonData.Parameter = "LTC_BTC";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerBook;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                await _mediator.Send(SendData);
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
                GetTradeHistoryInfo model = new GetTradeHistoryInfo();
                model.Amount = 20;
                model.ChargeRs = 1;
                model.DateTime = DateTime.Now; 
                model.IsCancel = 1;
                model.PairName = "LTC_BTC";
                model.Price = 150;
                model.Status = 1;
                model.StatusText = "Success";
                model.TrnNo = 90;
                model.Type = "SELL";

                GetTradeHistoryInfo temp = JsonConvert.DeserializeObject<GetTradeHistoryInfo>(Data);

                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.TradeHistoryByPair);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveTradingHistory );
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = temp;
                CommonData.Parameter = "LTC_BTC";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.TradeHistoryByPair;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                await _mediator.Send(SendData);
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
                GetGraphResponse model = new GetGraphResponse();
                model.ChangePer = 20;
                model.DataDate = 20180203073000;
                model.High = 1199;
                model.Low = 1177;
                model.TodayClose = 1452;
                model.TodayOpen = 1477;
                model.Volume = 173;

                GetGraphResponse temp = JsonConvert.DeserializeObject<GetGraphResponse>(Data);

                SignalRComm<GetGraphResponse> CommonData = new SignalRComm<GetGraphResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.ChartData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveChartData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = temp;
                CommonData.Parameter = "LTC_BTC";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.ChartData;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                await _mediator.Send(SendData);
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
                MarketCapData model = new MarketCapData();
                model.Change24 = 1;
                model.ChangePer = 3;
                model.High24 = 1153;
                model.Low24 = 1125;
                model.LastPrice = 1137;
                model.Volume24 = 253;

                MarketCapData temp = JsonConvert.DeserializeObject<MarketCapData>(Data);

                SignalRComm<MarketCapData> CommonData = new SignalRComm<MarketCapData>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.MarketData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveMarketData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = temp;
                CommonData.Parameter = "LTC_BTC";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.MarketData;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                await _mediator.Send(SendData);
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
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                ActiveOrderInfo temp = JsonConvert.DeserializeObject<ActiveOrderInfo>(Data);

                SignalRComm<ActiveOrderInfo> CommonData = new SignalRComm<ActiveOrderInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OpenOrder);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOpenOrder);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = accessToken;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OpenOrder;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
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
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                GetTradeHistoryInfo temp = JsonConvert.DeserializeObject<GetTradeHistoryInfo>(Data);

                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OrderHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOrderHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = accessToken;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OrderHistory;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
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
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                GetTradeHistoryInfo temp = JsonConvert.DeserializeObject<GetTradeHistoryInfo>(Data);

                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.TradeHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveTradeHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = accessToken;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.TradeHistory;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
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
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                WalletMasterResponse temp = JsonConvert.DeserializeObject<WalletMasterResponse>(Data);

                SignalRComm<WalletMasterResponse> CommonData = new SignalRComm<WalletMasterResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerSideWallet);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerSideWalletBal);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = accessToken;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerSideWallet;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
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
                 var accessToken = await HttpContext.GetTokenAsync("access_token");

                WalletMasterResponse temp = JsonConvert.DeserializeObject<WalletMasterResponse>(Data);

                SignalRComm<WalletMasterResponse> CommonData = new SignalRComm<WalletMasterResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.SellerSideWallet);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerSideWalletBal);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = accessToken;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerSideWallet;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
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
