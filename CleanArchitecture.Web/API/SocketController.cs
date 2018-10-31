using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
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
        private readonly ISignalRService _signalRService;
        //public SocketController(ILogger<SocketController> logger, IMediator mediator, ISignalRTestService signalRTestService)
        public SocketController(ILogger<SocketController> logger, IMediator mediator, ISignalRService signalRService)
        {
            _logger = logger;
            //_chat = chat;
            _mediator = mediator;
            _signalRService = signalRService;
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
                CommonData.Parameter = "INR_BTC";

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
                CommonData.Parameter = "INR_BTC";

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
                model.PairName = "INR_BTC";
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
                CommonData.Parameter = "INR_BTC";

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

                List<GetGraphResponse> temp = JsonConvert.DeserializeObject<List<GetGraphResponse>>(Data);

                SignalRComm<List<GetGraphResponse>> CommonData = new SignalRComm<List<GetGraphResponse>>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.ChartData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveChartData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = temp;
                CommonData.Parameter = "INR_BTC";

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
                CommonData.Parameter = "INR_BTC";

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

        [HttpGet("LastPrice/{Data}")]
        public async Task<IActionResult> LastPrice(decimal Data)
        {
            string ReciveMethod = "";
            try
            {
                SignalRComm<Decimal> CommonData = new SignalRComm<Decimal>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.Price);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveLastPrice);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = "INR_BTC";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.Price;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
                ReciveMethod = "RecieveLastPrice";
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
                ActiveOrderInfo model = new ActiveOrderInfo();
                model.Id = 96;
                model.TrnDate = DateTime.UtcNow;
                model.Type = "BUY";
                model.Order_Currency = "BTC";
                model.Delivery_Currency = "LTC";
                model.Amount = 100;
                model.Price = 1400;
                model.IsCancelled = 1;

                ActiveOrderInfo temp = JsonConvert.DeserializeObject<ActiveOrderInfo>(Data);

                SignalRComm<ActiveOrderInfo> CommonData = new SignalRComm<ActiveOrderInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OpenOrder);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOpenOrder);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OpenOrder;
                SendData.Parameter = accessToken;// CommonData.Parameter;
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

                GetTradeHistoryInfo model = new GetTradeHistoryInfo();
                model.TrnNo = 90;
                model.Type = "SELL";
                model.Price = 1400;
                model.Amount = 1000;
                model.Total = 140000;
                model.DateTime = DateTime.UtcNow;
                model.Status = 1;
                model.StatusText = "Success";
                model.PairName = "INR_BTC";
                model.ChargeRs = 10;
                model.IsCancel = 0;

                GetTradeHistoryInfo temp = JsonConvert.DeserializeObject<GetTradeHistoryInfo>(Data);

                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OrderHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOrderHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OrderHistory;
                SendData.Parameter = accessToken;// CommonData.Parameter;
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
                GetTradeHistoryInfo model = new GetTradeHistoryInfo();
                model.TrnNo = 90;
                model.Type = "SELL";
                model.Price = 1400;
                model.Amount = 1000;
                model.Total = 140000;
                model.DateTime = DateTime.UtcNow;
                model.Status = 1;
                model.StatusText = "Success";
                model.PairName = "INR_BTC";
                model.ChargeRs = 10;

                GetTradeHistoryInfo temp = JsonConvert.DeserializeObject<GetTradeHistoryInfo>(Data);

                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.TradeHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveTradeHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.TradeHistory;
                SendData.Parameter = accessToken;// CommonData.Parameter;
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

                WalletMasterResponse model = new WalletMasterResponse();
                model.WalletName = "BTC Default";
                model.AccWalletID = "1029399266000200";
                model.Balance = 201200;
                model.CoinName = "BTC";
                model.IsDefaultWallet = 0;
                model.PublicAddress = "";
                WalletMasterResponse temp = JsonConvert.DeserializeObject<WalletMasterResponse>(Data);

                SignalRComm<WalletMasterResponse> CommonData = new SignalRComm<WalletMasterResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerSideWallet);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerSideWalletBal);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerSideWallet;
                SendData.Parameter = accessToken;// CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                SendData.WalletName = "BTC";

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
                WalletMasterResponse model = new WalletMasterResponse();
                model.WalletName = "LTC Default";
                model.AccWalletID = "1053841474000201";
                model.Balance = 201200;
                model.CoinName = "LTC";
                model.IsDefaultWallet = 1;
                model.PublicAddress = "";

                WalletMasterResponse temp = JsonConvert.DeserializeObject<WalletMasterResponse>(Data);
                
                SignalRComm<WalletMasterResponse> CommonData = new SignalRComm<WalletMasterResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.SellerSideWallet);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerSideWalletBal);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = temp;
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerSideWallet;
                SendData.Parameter = accessToken;// CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                SendData.WalletName = "INR";
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

        [HttpGet("ActivityNotification/{Data}")]
        [Authorize]
        public async Task<IActionResult> ActivityNotification(string Data)
        {
            string ReciveMethod = "";
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                SignalRComm<String> CommonData = new SignalRComm<String>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.ActivityNotification);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveNotification);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter =null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.ActivityNotification;
                SendData.Parameter = accessToken;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                await _mediator.Send(SendData);
                ReciveMethod = "RecieveNotification";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        [HttpGet("PairData/{Data}")]
        public async Task<IActionResult> PairData(string Data)
        {
            string ReciveMethod = "";
            try
            {
                VolumeDataRespose model = new VolumeDataRespose();
                model.ChangePer = 20;
                model.Currentrate = 1;
                model.High24Hr = 1814;
                model.High52Week = 1744;
                model.HighWeek = 1800;
                model.Low24Hr = 1812;
                model.Low52Week = 1725;
                model.LowWeek = 1700;
                model.PairId = 10021001;
                model.PairName = "INR_BTC";
                model.UpDownBit = 0;
                model.Volume24 = 1406;


                VolumeDataRespose temp = JsonConvert.DeserializeObject<VolumeDataRespose>(Data);

                SignalRComm<VolumeDataRespose> CommonData = new SignalRComm<VolumeDataRespose>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.PairData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecievePairData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.Base);
                CommonData.Data = temp;
                CommonData.Parameter = "XRP";

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.PairData;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                await _mediator.Send(SendData);
                ReciveMethod = "RecievePairData";
                return Ok(new { ReciveMethod = ReciveMethod });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

                return Ok();
            }
        }

        //[HttpGet("MarketTicker/{Data}")]
        //public async Task<IActionResult> MarketTicker(string Data)
        //{
        //    string ReciveMethod = "";
        //    try
        //    {
        //        VolumeDataRespose model = new VolumeDataRespose();
        //        model.ChangePer = 20;
        //        model.Currentrate = 1;
        //        model.High24Hr = 1814;
        //        model.High52Week = 1744;
        //        model.HighWeek = 1800;
        //        model.Low24Hr = 1812;
        //        model.Low52Week = 1725;
        //        model.LowWeek = 1700;
        //        model.PairId = 10021001;
        //        model.PairName = "INR_BTC";
        //        model.UpDownBit = 0;
        //        model.Volume24 = 1406;


        //        //VolumeDataRespose temp = JsonConvert.DeserializeObject<VolumeDataRespose>(Data);

        //        SignalRComm<VolumeDataRespose> CommonData = new SignalRComm<VolumeDataRespose>();
        //        CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
        //        CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.MarketTicker);
        //        CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveMarketTicker);
        //        CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
        //        CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.Base);
        //        CommonData.Data = model;
        //        CommonData.Parameter = "BTC";

        //        SignalRData SendData = new SignalRData();
        //        SendData.Method = enMethodName.MarketTicker;
        //        SendData.Parameter = CommonData.Parameter;
        //        SendData.DataObj = JsonConvert.SerializeObject(CommonData);
        //        await _mediator.Send(SendData);
        //        ReciveMethod = "RecieveMarketTicker";
        //        return Ok(new { ReciveMethod = ReciveMethod });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);

        //        return Ok();
        //    }
        //}

        //[HttpGet("CreateTranTest")]
        ////[Authorize]
        //public async Task<IActionResult> CreateTranTest(long ID)
        //{
        //    string ReciveMethod = "";
        //    try
        //    {
        //        var accessToken = "CfDJ8GX72IE05DhMsT4VIPEe9lq_VFZzbmJIlw2XMjA1HrvNrTLM7tc0vzM6XCBmVdiBnZt0OddELrA_kaKm53McC54HxwtXipiQhl3m_ty1YYTYQ-Pmre3UxZhlR2bQ3jYPLUVLRiFZ0FOZTAGEwXesE2aiBsKcA3nZ7PWs7pUeGqpA5zCAON-O2YkGBWzO4wIaqUQX7knyrQNX7eQHXhr3EGxr5VJz9EyjOs_TRNsBd8zb9H8lzOSnUVUW_e6dvvYjFkPopuiTN3g1vDTL2Sde0pUIZllBm5w7emieidcgsaiiSWj_tQld4gd-Lcv0yRDgtM83ur3kvih_sNdw0dSywKQQUd7z7xq87hG3Zkpgu271eKkcNuB7vum5bamROi3m-ESDSdn_SdKl1vlrVUrSBeUSzsxWy20jbH4YMzCM-hFB3FmhHdybzuGMtu4y0uB-ROGuoXK5RjFAC_72-VqCQSFgWkhsuIlVrvNFKFk279HtmMWL9JU2IXqtl_50dJHol46DG5x0TGNDqa0IfW6onc7EQhYv2DSeORAY9QLQVdgCI761qJ3rNnWZ1-bGbo-6t-PWI9v8G9XnZ7mne8xpeSgf8zMos7NAmOOjnGa3-jB3mNZbmhaYKL_y9knG63YRw8AW3h8doYnk6W9Wwehi0ECCiccKdmc_ohfaLTXPjUL5Miz_wXdb_fKQolgIy4dT76kVE5wKDKFtzLT_YfuvQQN8xzUoZH6ybihIrFBrecJj"; await HttpContext.GetTokenAsync("access_token");
        //        //_signalRTestService.MarkTransactionHold(ID, accessToken);
        //        TransactionQueue Newtransaction = new TransactionQueue();
        //        Newtransaction.TrnType = 4;
        //        Newtransaction.Id = 91;
        //        Newtransaction.TrnDate = DateTime.Now;

        //        TradeTransactionQueue NewTradeTransaction = new TradeTransactionQueue();
        //        NewTradeTransaction.PairName = "INR_BTC";
        //        NewTradeTransaction.BidPrice = 1450;
        //        NewTradeTransaction.PairID = 10021001;
        //        NewTradeTransaction.AskPrice = 1500;
        //        NewTradeTransaction.TrnType = 4;
        //        NewTradeTransaction.Order_Currency = "BTC";
        //        NewTradeTransaction.Delivery_Currency = "INR";
        //        NewTradeTransaction.BuyQty = 2;
        //        NewTradeTransaction.SellQty = 1;
        //        NewTradeTransaction.IsCancelled = 0;
        //        _signalRService.OnStatusChange(4, Newtransaction, NewTradeTransaction, accessToken);
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
