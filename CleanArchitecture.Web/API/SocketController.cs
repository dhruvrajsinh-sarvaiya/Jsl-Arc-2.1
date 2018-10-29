using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SignalR;
using CleanArchitecture.Core.ViewModels.Transaction;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        public SocketController(ILogger<SocketController> logger, IMediator mediator)
        {
            _logger = logger;
            //_chat = chat;
            _mediator = mediator;
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

                var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType =enSignalREventType.Channel;
                modelData.Method = enMethodName.BuyerBook;
                modelData.ReturnMethod = enReturnMethod.RecieveBuyerBook;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType =enSignalRParmType.PairName;
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
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.SellerBook;
                modelData.ReturnMethod = enReturnMethod.RecieveSellerBook;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.PairName;
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
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.TradeHistoryByPair;
                modelData.ReturnMethod = enReturnMethod.RecieveTradingHistory;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;//  JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.PairName;
                modelData.Parameter = "LTC_BTC";
                await _mediator.Send(modelData);
                ReciveMethod = "RecieveTradingHistory";
                return Ok(new { ReciveMethod= ReciveMethod });
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

                var temp= JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.ChartData;
                modelData.ReturnMethod = enReturnMethod.RecieveChartData;
                modelData.Subscription = enSubscriptionType.OneToOne;

                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.PairName;
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
                MarketCapData model = new MarketCapData();
                model.Change24 = 1;
                model.ChangePer = 3;
                model.High24 = 1153;
                model.Low24 = 1125;
                model.LastPrice = 1137;
                model.Volume24 = 253;

                SignalRData modelData = new SignalRData();
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.MarketData;
                modelData.ReturnMethod = enReturnMethod.RecieveMarketData;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.PairName;
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
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 3;
                model.Price = 150;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.OpenOrder;
                modelData.ReturnMethod = enReturnMethod.RecieveOpenOrder;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.AccessToken;
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
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 3;
                model.Price = 150;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.OrderHistory;
                modelData.ReturnMethod = enReturnMethod.RecieveOrderHistory;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.AccessToken;
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
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 3;
                model.Price = 150;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var temp = JsonConvert.SerializeObject(model);
                SignalRData modelData = new SignalRData();
                modelData.EventType = enSignalREventType.Channel;
                modelData.Method = enMethodName.TradeHistory;
                modelData.ReturnMethod = enReturnMethod.RecieveTradeHistory;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = Data;// JsonConvert.SerializeObject(model);
                modelData.ParamType = enSignalRParmType.AccessToken;
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
    }
}
