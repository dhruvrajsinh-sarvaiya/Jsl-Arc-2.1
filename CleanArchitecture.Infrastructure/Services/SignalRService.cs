﻿using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.ViewModels.Wallet;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    class SignalRService : ISignalRService
    {
        private readonly ILogger<SignalRService> _logger;
        private readonly IMediator _mediator;

        public SignalRService(ILogger<SignalRService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public void BuyerBook(GetBuySellBook Data, string Pair)
        {
            try
            {
                SignalRComm<GetBuySellBook> CommonData = new SignalRComm<GetBuySellBook>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerBook);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerBook);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerBook;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void SellerBook(GetBuySellBook Data, string Pair)
        {
            try
            {
                SignalRComm<GetBuySellBook> CommonData = new SignalRComm<GetBuySellBook>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.SellerBook);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerBook);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerBook;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                 _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void TradingHistoryByPair(GetTradeHistoryInfo Data, string Pair)
        {
            try
            {
                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.TradeHistoryByPair);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveTradingHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.TradeHistoryByPair;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void ChartData(List<GetGraphResponse> Data, string Pair)
        {
            try
            {
                SignalRComm<List<GetGraphResponse>> CommonData = new SignalRComm<List<GetGraphResponse>>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.ChartData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveChartData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.ChartData;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void MarketData(MarketCapData Data, string Pair)
        {
            try
            {
                SignalRComm<MarketCapData> CommonData = new SignalRComm<MarketCapData>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.MarketData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveMarketData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.MarketData;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void OpenOrder(ActiveOrderInfo Data, string Token)
        {
            try
            {
                SignalRComm<ActiveOrderInfo> CommonData = new SignalRComm<ActiveOrderInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OpenOrder);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOpenOrder);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OpenOrder;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                 _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void OrderHistory(GetTradeHistoryInfo Data, string Token)
        {
            try
            {
                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OrderHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOrderHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OrderHistory;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void TradeHistoryByUser(GetTradeHistoryInfo Data, string Token)
        {
            try
            {
                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.TradeHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveTradeHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.TradeHistory;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void BuyerSideWalletBal(WalletMasterResponse Data, string Token)
        {
            try
            {
                SignalRComm<WalletMasterResponse> CommonData = new SignalRComm<WalletMasterResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerSideWallet);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerSideWalletBal);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerSideWallet;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void SellerSideWalletBal(WalletMasterResponse Data, string Token)
        {
            try
            {
                SignalRComm<WalletMasterResponse> CommonData = new SignalRComm<WalletMasterResponse>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.SellerSideWallet);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerSideWalletBal);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerSideWallet;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }



    }
}
