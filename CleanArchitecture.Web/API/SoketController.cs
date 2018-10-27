﻿using System;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    public class SoketController : Controller
    {
        //private SocketHub _chat;
        private readonly ILogger<SoketController> _logger;
        private readonly IMediator _mediator;
        public SoketController(ILogger<SoketController> logger, IMediator mediator)
        {
            _logger = logger;
            //_chat = chat;
            _mediator = mediator;
        }
        
        [HttpGet("BuyerBook")]
        public async Task<IActionResult> BuyerBook()
        {
            string ReciveMethod = "";
            try
            {
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 3;
                model.Price = 150;


                SignalRData modelData = new SignalRData();
                modelData.Type =enSignalREventType.Channel;
                modelData.Method = enPairWiseMethodName.BuyerBook;
                modelData.ReturnMethod = enPairWiseMethodName.BuyerBook;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData .Data= JsonConvert.SerializeObject(model);
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

        [HttpGet("SellerBook")]
        public async Task<IActionResult> SellerBook()
        {
            string ReciveMethod = "";
            try
            {
                GetBuySellBook model = new GetBuySellBook();
                model.Amount = 1755;
                model.Price = 40;


                SignalRData modelData = new SignalRData();
                modelData.Type = enSignalREventType.Channel;
                modelData.Method = enPairWiseMethodName.SellerBook;
                modelData.ReturnMethod = enPairWiseMethodName.SellerBook;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = JsonConvert.SerializeObject(model);
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

        [HttpGet("TradingHistory")]
        public async Task<IActionResult> TradingHistory()
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

                SignalRData modelData = new SignalRData();
                modelData.Type = enSignalREventType.Channel;
                modelData.Method = enPairWiseMethodName.TradeHistoryByPair;
                modelData.ReturnMethod = enPairWiseMethodName.TradeHistoryByPair;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = JsonConvert.SerializeObject(model);
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

        [HttpGet("ChartData")]
        public async Task<IActionResult> ChartData()
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

                SignalRData modelData = new SignalRData();
                modelData.Type = enSignalREventType.Channel;
                modelData.Method = enPairWiseMethodName.ChartData;
                modelData.ReturnMethod = enPairWiseMethodName.ChartData;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = JsonConvert.SerializeObject(model);
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

        [HttpGet("MarketData")]
        public async Task<IActionResult> MarketData()
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
                modelData.Type = enSignalREventType.Channel;
                modelData.Method = enPairWiseMethodName.MarketData;
                modelData.ReturnMethod = enPairWiseMethodName.MarketData;
                modelData.Subscription = enSubscriptionType.OneToOne;
                modelData.Data = JsonConvert.SerializeObject(model);
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
    }
}