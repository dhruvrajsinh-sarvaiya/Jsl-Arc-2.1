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
        
        [HttpGet("BuyerBook/{Data}/{Pair}")]
        public async Task<IActionResult> BuyerBook(string Data,String Pair)
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
                CommonData.Parameter = Pair;// "INR_BTC";

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

        [HttpGet("SellerBook/{Data}/{Pair}")]
        public async Task<IActionResult> SellerBook(string Data, String Pair)
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
                CommonData.Parameter = Pair;// "INR_BTC";

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

        [HttpGet("TradingHistory/{Data}/{Pair}")]
        public async Task<IActionResult> TradingHistory(string Data, String Pair)
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
                CommonData.Parameter = Pair;// "INR_BTC";

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

        [HttpGet("ChartData/{Data}/{Pair}")]
        public async Task<IActionResult> ChartData(string Data, String Pair)
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
                CommonData.Parameter = Pair;// "INR_BTC";

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

        [HttpGet("MarketData/{Data}/{Pair}")]
        public async Task<IActionResult> MarketData(string Data, String Pair)
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
                CommonData.Parameter = Pair;// "INR_BTC";

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

        [HttpGet("LastPrice/{Data}/{Pair}")]
        public async Task<IActionResult> LastPrice(decimal Data, String Pair)
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
                CommonData.Parameter = Pair;// "INR_BTC";

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
                SendData.WalletName = temp.CoinName;

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
                SendData.WalletName = temp.CoinName;
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
                string a = temp.PairName.Split("_")[1];
                SignalRComm<VolumeDataRespose> CommonData = new SignalRComm<VolumeDataRespose>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.PairData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecievePairData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.Base);
                CommonData.Data = temp;
                CommonData.Parameter =a;

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
        //        var accessToken = "CfDJ8MB8DXV9k79JrEe_PIsRHuoXYoktXaP7EjFqGuVjEiiaoDD4xLApH2bywCk5ehhm8--MqkgUUfQs5aZG7CHzBOaKL6PRj_D807rvIwwaAFtAntn9ZIdvSKXnPOoBgqJ2eKHn23BuXL7mhB046b6dssHLWqKoWPnqzR98h5whdupq3FnlbKIqggRZ_4TDO60_h4Qnan7TWq4Keki3QysUwBK2Ozc2QsrqTevt9lG7Zo-1kV_8AB4XVb86cvuioiRQUFrHyF6o6VqNimPW882kzoktQFc1gI0qlCkHD8z0f7iFWs1e56r94mUOIqjXtIbL-yWQpoo5edgzcMZpool-L-cttcx-enWXcx91ziL_PBGC3Uko1EcuqFPGRqNKVc6ABFv4igSkyV1cnScMrertxYAU_ito2LlFqSlwhiLjDP2WghLMDPYI7KX76VVeSDKaLWJcYlKX3f272229tMEkyGVNfcqYkVvndLF5mgEtvk6XCXQ22w51pArBwtRkxxD3u2MYDN51VIpCvi7vkD1pyrQEidBhttBB5ySs0cF8aV8YXRBAYqLa9Dqyb1fuqu4kbrmHKgpEVhbT9H5d2ERnhEzowF7Q_doO-rPmc9RM2_h_28GTH1M0OD8BDiSpxaRIhKROm34Lof6DcmcHHF68biccOTjAk78sficf6NEVSj0zamO1KvvYcgtXX2ueTgXCcx7QUzQwG8mVEVgNVYwXs3Hzi7ycCcImwsvoihGDtgCb";
        //        //await HttpContext.GetTokenAsync("access_token");
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
