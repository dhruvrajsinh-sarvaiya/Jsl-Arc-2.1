using CleanArchitecture.Core.ApiModels;
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
    class SignalRService :ISignalRService
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 5);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 6);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = Pair;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 6);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 7);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = Pair;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 7);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 8);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = Pair;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 12);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 10);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = Pair;
                 _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 8);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 9);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data =JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 1);
                modelData.Parameter = Pair;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 1);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 1);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data =  JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = Token;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 2);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 2);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = Token;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 3);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 3);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = Token;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 10);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 4);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data =JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = Token;
                _mediator.Send(modelData);
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
                SignalRData modelData = new SignalRData();
                modelData.EventType = Enum.GetName(typeof(enSignalREventType), 4);
                modelData.Method = Enum.GetName(typeof(enMethodName), 11);
                modelData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), 5);
                modelData.Subscription = Enum.GetName(typeof(enSubscriptionType), 1);
                modelData.Data = JsonConvert.SerializeObject(Data);
                modelData.ParamType = Enum.GetName(typeof(enSignalRParmType), 3);
                modelData.Parameter = Token;
                _mediator.Send(modelData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }


        
    }
}
