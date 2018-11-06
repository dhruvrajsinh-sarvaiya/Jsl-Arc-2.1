using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ApiModels.Chat;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class SignalRService :ISignalRService
    {
        private readonly ILogger<SignalRService> _logger;
        private readonly IMediator _mediator;
        //private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        //private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly IFrontTrnRepository _frontTrnRepository;
        private RedisConnectionFactory _fact;
        public String Token=null;
        public string ControllerName = "SignalRService";
        public SignalRService(ILogger<SignalRService> logger, IMediator mediator, IFrontTrnRepository frontTrnRepository,
             RedisConnectionFactory Factory)
        {
            _fact = Factory;
            _logger = logger;
            _mediator = mediator;
            // _TransactionRepository = TransactionRepository;
            _frontTrnRepository = frontTrnRepository;
            //_TradeTransactionRepository = TradeTransactionRepository;
           
        }
        #region Pairwise
        public void BuyerBook(GetBuySellBook Data, string Pair)
        {
            try
            {
                SignalRComm<GetBuySellBook> CommonData = new SignalRComm<GetBuySellBook>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.BuyerBook);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerBook);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                //SignalRDataBuyerBook SendData = new SignalRDataBuyerBook();
                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerBook;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
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
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                //SignalRDataSellerBook SendData = new SignalRDataSellerBook();
                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerBook;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void OrderHistory(GetTradeHistoryInfo Data, string Pair)
        {
            try
            {
                SignalRComm<GetTradeHistoryInfo> CommonData = new SignalRComm<GetTradeHistoryInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.OrderHistory);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOrderHistory);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OrderHistory;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void ChartData(GetGraphDetailInfo Data, string Pair)
        {
            try
            {
                SignalRComm<GetGraphDetailInfo> CommonData = new SignalRComm<GetGraphDetailInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.ChartData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveChartData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
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
                //throw ex;
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
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
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
                //throw ex;
            }
        }

        public void LastPrice(LastPriceViewModel Data, string Pair)
        {
            try
            {
                SignalRComm<LastPriceViewModel> CommonData = new SignalRComm<LastPriceViewModel>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.Price);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveLastPrice);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.PairName);
                CommonData.Data = Data;
                CommonData.Parameter = Pair;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.Price;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        #endregion

        #region UserSpecific
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
                CommonData.Parameter = null;

                //SignalRDataOpenOrder SendData = new SignalRDataOpenOrder();
                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.OpenOrder;
                SendData.Parameter = Token;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void TradeHistory(GetTradeHistoryInfo Data, string Token)
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
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.TradeHistory;
                SendData.Parameter = Token;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void RecentOrder(RecentOrderInfo Data, string Token)
        {
            try
            {
                SignalRComm<RecentOrderInfo> CommonData = new SignalRComm<RecentOrderInfo>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.RecentOrder);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveRecentOrder);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Data;
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.RecentOrder;
                SendData.Parameter = Token;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void BuyerSideWalletBal(WalletMasterResponse Data, string Wallet, string Token)
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
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerSideWallet;
                SendData.Parameter = Token;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                SendData.WalletName = Wallet;

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void SellerSideWalletBal(WalletMasterResponse Data, string Wallet, string Token)
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
                CommonData.Parameter = null;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerSideWallet;
                SendData.Parameter = Token;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                SendData.WalletName = Wallet;

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void ActivityNotification(string Msg, string Token)
        {
            try
            {
                SignalRComm<String> CommonData = new SignalRComm<String>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.ActivityNotification);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveNotification);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.OneToOne);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.AccessToken);
                CommonData.Data = Msg;
                CommonData.Parameter = null;

                //SignalRDataNotify SendData = new SignalRDataNotify();
                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.ActivityNotification;
                SendData.Parameter = Token;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                //SendData.WalletName = Wallet;
                HelperForLog.WriteLogIntoFile("ActivityNotification", ControllerName, " MSG :" + Msg);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        #endregion

        #region BaseMarket
        public void PairData(VolumeDataRespose Data, string Base)
        {
            try
            {
                SignalRComm<VolumeDataRespose> CommonData = new SignalRComm<VolumeDataRespose>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.PairData);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecievePairData);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.Base);
                CommonData.Data = Data;
                CommonData.Parameter = Base;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.PairData;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void MarketTicker(VolumeDataRespose Data, string Base)
        {
            try
            {
                SignalRComm<VolumeDataRespose> CommonData = new SignalRComm<VolumeDataRespose>();
                CommonData.EventType = Enum.GetName(typeof(enSignalREventType), enSignalREventType.Channel);
                CommonData.Method = Enum.GetName(typeof(enMethodName), enMethodName.MarketTicker);
                CommonData.ReturnMethod = Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveMarketTicker);
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
                CommonData.ParamType = Enum.GetName(typeof(enSignalRParmType), enSignalRParmType.Base);
                CommonData.Data = Data;
                CommonData.Parameter = Base;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.MarketTicker;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }
        #endregion
        public void OnVolumeChange(VolumeDataRespose volumeData, MarketCapData capData)
        {
            try
            {
                HelperForLog.WriteLogIntoFile("OnVolumeChange", ControllerName, "Call OnVolumeChangeMethod : volumeData : " + JsonConvert.SerializeObject(volumeData) + " : Market Data : "+JsonConvert.SerializeObject(capData));
                if (volumeData!=null && capData!= null)
                {
                    LastPriceViewModel lastPriceData = new LastPriceViewModel();
                    lastPriceData.LastPrice = capData.LastPrice;
                    lastPriceData.UpDownBit = volumeData.UpDownBit;

                    string Base = volumeData.PairName.Split("_")[1];
                    PairData(volumeData, Base);
                    HelperForLog.WriteLogIntoFile("OnVolumeChange", ControllerName, "After Pair Data Call Base :"+ Base+ "  DATA :" + JsonConvert.SerializeObject(volumeData));
                    MarketData(capData, volumeData.PairName);
                    HelperForLog.WriteLogIntoFile("OnVolumeChange", ControllerName, "After Market Data Call Pair :" + volumeData.PairName +"  DATA :" + JsonConvert.SerializeObject(capData));
                    LastPrice(lastPriceData, volumeData.PairName);
                    HelperForLog.WriteLogIntoFile("OnVolumeChange", ControllerName, "After Last price Call Pair :" + volumeData.PairName + "  DATA :" + JsonConvert.SerializeObject(lastPriceData));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }

        public void OnWalletBalChange(WalletMasterResponse Data, string WalletTypeName, string Token, short TokenType=1)
        {
            try
            {
                if(TokenType ==Convert.ToInt16(enTokenType.ByUserID))
                {
                    Token = GetTokenByUserID(Token);
                }
                if (!string.IsNullOrEmpty(Token))
                {
                    BuyerSideWalletBal(Data, WalletTypeName, Token);
                    SellerSideWalletBal(Data, WalletTypeName, Token);
                    HelperForLog.WriteLogIntoFile("OnWalletBalChange", ControllerName, " Wallet Name : "+ WalletTypeName + " Data :"+ JsonConvert.SerializeObject(Data));
                }
                    
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
            }
        }

        public void GetAndSendOpenOrderData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType, short IsPop = 0)
        {
            try
            {
                
                ActiveOrderInfo OpenOrderModel = new ActiveOrderInfo();
                OpenOrderModel.Id = Newtransaction.Id;
                OpenOrderModel.TrnDate = Newtransaction.TrnDate;
                OpenOrderModel.Type = (NewTradeTransaction.TrnType == 4) ? "BUY" : "SELL";
                OpenOrderModel.Order_Currency = NewTradeTransaction.Order_Currency;
                OpenOrderModel.Delivery_Currency = NewTradeTransaction.Delivery_Currency;
                if (IsPop == 1)
                    OpenOrderModel.Amount = 0;
                else
                    OpenOrderModel.Amount = (NewTradeTransaction.BuyQty == 0) ? NewTradeTransaction.SellQty : (NewTradeTransaction.SellQty == 0) ? NewTradeTransaction.BuyQty : NewTradeTransaction.BuyQty;
                OpenOrderModel.Price = (NewTradeTransaction.BidPrice == 0) ? NewTradeTransaction.AskPrice : (NewTradeTransaction.AskPrice == 0) ? NewTradeTransaction.BidPrice : NewTradeTransaction.BidPrice;
                OpenOrderModel.IsCancelled = NewTradeTransaction.IsCancelled;
                OpenOrderModel.OrderType= Enum.GetName(typeof(enTransactionMarketType), OrderType);
                OpenOrderModel.PairId = NewTradeTransaction.PairID;
                OpenOrderModel.PairName = NewTradeTransaction.PairName;
                OpenOrder(OpenOrderModel, Token);
                HelperForLog.WriteLogIntoFile("GetAndSendOpenOrderData", ControllerName, " After OpenOrder call TRNNO:" + Newtransaction.Id);
                if (IsPop != 1)//send notification
                {
                    var msg = EnResponseMessage.SignalRTrnSuccessfullyCreated;
                    msg = msg.Replace("#Price#", OpenOrderModel.Price.ToString());
                    msg = msg.Replace("#Qty#", OpenOrderModel.Amount.ToString());
                    ActivityNotification(msg, Token);
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
            }
        }

        public GetTradeHistoryInfo GetAndSendTradeHistoryInfoData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short OrderType, short IsPop = 0)
        {
            try
            {
                //var OrderHistoryList = _frontTrnRepository.GetTradeHistory(0, "", "", "", 0, 0, Newtransaction.Id);
                GetTradeHistoryInfo model = new GetTradeHistoryInfo();
                model.TrnNo = NewTradeTransaction.TrnNo;
                model.Type = (NewTradeTransaction.TrnType == 4) ? "BUY" : "SELL";
                model.Price = (NewTradeTransaction.BidPrice == 0) ? NewTradeTransaction.AskPrice : (NewTradeTransaction.AskPrice == 0) ? NewTradeTransaction.BidPrice : NewTradeTransaction.BidPrice;
                model.Amount = (NewTradeTransaction.TrnType == 4) ? NewTradeTransaction.SettledBuyQty : NewTradeTransaction.SettledSellQty;
                model.Total = model.Type == "BUY" ? ((model.Price * model.Amount) - model.ChargeRs) : ((model.Price * model.Amount));
                model.DateTime = Convert.ToDateTime(NewTradeTransaction.SettledDate);
                model.Status = NewTradeTransaction.Status;
                model.StatusText = Enum.GetName(typeof(enTransactionStatus),model.Status);
                model.PairName = NewTradeTransaction.PairName;
                model.ChargeRs = Convert.ToDecimal(Newtransaction.ChargeRs);
                model.IsCancel = NewTradeTransaction.IsCancelled;
                model.OrderType= Enum.GetName(typeof(enTransactionMarketType), OrderType);
                return model;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
                return null;
            }
        }

        public void GetAndSendRecentOrderData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType, short IsPop = 0)
        {
            try
            {
                RecentOrderInfo model = new RecentOrderInfo();
                model.TrnNo = NewTradeTransaction.TrnNo;
                model.Type = (NewTradeTransaction.TrnType == 4) ? "BUY" : "SELL";
                model.Price = (NewTradeTransaction.BidPrice == 0) ? NewTradeTransaction.AskPrice : (NewTradeTransaction.AskPrice == 0) ? NewTradeTransaction.BidPrice : NewTradeTransaction.BidPrice;
                model.Qty = (NewTradeTransaction.BuyQty == 0) ? NewTradeTransaction.SellQty : (NewTradeTransaction.SellQty == 0) ? NewTradeTransaction.BuyQty : NewTradeTransaction.BuyQty;
                model.DateTime = Newtransaction .TrnDate ;
                model.Status = NewTradeTransaction.StatusMsg;
                model.PairId = NewTradeTransaction.PairID;
                model.PairName = NewTradeTransaction.PairName;
                model.OrderType = Enum.GetName(typeof(enTransactionMarketType), OrderType);
                RecentOrder(model, Token);
                HelperForLog.WriteLogIntoFile("GetAndSendRecentOrderData", ControllerName, "After Socket call RecentOrder TRNNO:" + Newtransaction.Id);
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
            }
        }

        public void SendActivityNotification(string Msg, string Token, short TokenType = 1)
        {
            try
            {
                if (TokenType == Convert.ToInt16(enTokenType.ByUserID))
                {
                    Token = GetTokenByUserID(Token);
                }
                if (!string.IsNullOrEmpty(Token))
                {
                    ActivityNotification(Msg, Token);
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
            }
        }

        public string GetTokenByUserID(string ID)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                string AccessToken = Redis.GetHashData("Tokens:" + ID.ToString(), "Token");
                return AccessToken;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
                return "";
            }
        }

        public void OnStatusSuccess(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType)
        {
            try
            {
                GetTradeHistoryInfo historyInfo = new GetTradeHistoryInfo();
                GetBuySellBook BuySellmodel = new GetBuySellBook();
                //update Recent Order
                //pop OpenOrder
                //add tradehistory
                //add orderhistory
                //pop buyer/seller book;
                //HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, "Call ---- TransactionQueue :" + JsonConvert.SerializeObject(Newtransaction) + " TradeTransactionQueue :" + JsonConvert.SerializeObject(NewTradeTransaction));
                if (string.IsNullOrEmpty(Token))
                {
                    Token = GetTokenByUserID(NewTradeTransaction.MemberID.ToString());
                }
                if (!string.IsNullOrEmpty(Token))
                {
                    BuySellmodel.Amount = 0;
                    BuySellmodel.OrderId = new Guid();
                    BuySellmodel.RecordCount = 0;
                    if (NewTradeTransaction.TrnType == 4)//Buy
                    {
                        BuySellmodel.Price = NewTradeTransaction.BidPrice;
                        BuyerBook(BuySellmodel, NewTradeTransaction.PairName);
                        HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, "BuyerBook call TRNNO:" + Newtransaction.Id);
                    }
                    else//Sell
                    {
                        BuySellmodel.Price = NewTradeTransaction.AskPrice;
                        SellerBook(BuySellmodel, NewTradeTransaction.PairName);
                        HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, "SellerBook call TRNNO:" + Newtransaction.Id);
                    }
                    GetAndSendRecentOrderData(Newtransaction, NewTradeTransaction,Token, OrderType);//Update Recent
                    HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, " Aftre Recent Order Socket call TRNNO:" + Newtransaction.Id);
                    GetAndSendOpenOrderData(Newtransaction, NewTradeTransaction,Token, OrderType,1);//update OpenOrder
                    HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, " Aftre Open Order Socket call TRNNO:" + Newtransaction.Id);
                    historyInfo = GetAndSendTradeHistoryInfoData(Newtransaction, NewTradeTransaction, OrderType);
                    OrderHistory(historyInfo, historyInfo.PairName);//Order
                    HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, " Aftre Order History Socket call  : TRNNO:" + Newtransaction.Id);
                    TradeHistory(historyInfo, Token);//TradeHistory
                    HelperForLog.WriteLogIntoFile("OnStatusSuccess", ControllerName, " Aftre Trade History Socket call  : TRNNO:" + Newtransaction.Id);
                    var msg = EnResponseMessage.SignalRTrnSuccessfullySettled;
                    msg = msg.Replace("#Price#", historyInfo.Price.ToString());
                    msg = msg.Replace("#Qty#", historyInfo.Amount.ToString());
                    msg = msg.Replace("#Total#", historyInfo.Total.ToString());
                    ActivityNotification(msg, Token);
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
            }
        }
        public void OnStatusPartialSuccess(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType)
        {
            try
            {
                GetBuySellBook BuySellmodel = new GetBuySellBook();
                //update Buyer/seller book
                //HelperForLog.WriteLogIntoFile("OnStatusPartialSuccess", ControllerName, " TransactionQueue :" + JsonConvert.SerializeObject(Newtransaction) + " TradeTransactionQueue :" + JsonConvert.SerializeObject(NewTradeTransaction));
                if (string.IsNullOrEmpty(Token))
                {
                    Token = GetTokenByUserID(NewTradeTransaction.MemberID.ToString());
                }
                if (!string.IsNullOrEmpty(Token))
                {
                    List<GetBuySellBook> list = new List<GetBuySellBook>();
                    if (NewTradeTransaction.TrnType == 4)//Buy
                    {
                        list = _frontTrnRepository.GetBuyerBook(NewTradeTransaction.PairID, NewTradeTransaction.BidPrice);
                        foreach (var model in list)
                        {
                            BuySellmodel = model;
                            break;
                        }
                        if (BuySellmodel.OrderId.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            
                            BuyerBook(BuySellmodel, NewTradeTransaction.PairName);
                            HelperForLog.WriteLogIntoFile("OnStatusPartialSuccess", ControllerName, "BuyerBook call TRNNO:" + Newtransaction.Id);
                        }
                    }
                    else//Sell
                    {
                        list = _frontTrnRepository.GetSellerBook(NewTradeTransaction.PairID, NewTradeTransaction.AskPrice);
                        foreach (var model in list)
                        {
                           
                            BuySellmodel = model;
                            break;
                        }
                        if (BuySellmodel.OrderId.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            SellerBook(BuySellmodel, NewTradeTransaction.PairName);
                            HelperForLog.WriteLogIntoFile("OnStatusPartialSuccess", ControllerName, "SellerBook call TRNNO:" + Newtransaction.Id);
                        }
                    }
                }   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
        }
        public void OnStatusHold(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType)
        {
            try
            {
                GetTradeHistoryInfo historyInfo = new GetTradeHistoryInfo();
                GetBuySellBook BuySellmodel = new GetBuySellBook();
                //add buyer/seller book
                //add OpenOrder
                //add recent order
               // HelperForLog.WriteLogIntoFile("OnStatusHold", ControllerName, " TransactionQueue :" );
                if (string.IsNullOrEmpty(Token))
                {
                    Token = GetTokenByUserID(NewTradeTransaction.MemberID.ToString());
                }
                List<GetBuySellBook> list = new List<GetBuySellBook>();
                if (!string.IsNullOrEmpty(Token))
                {
                    if (NewTradeTransaction.TrnType == 4)//Buy
                    {
                        list = _frontTrnRepository.GetBuyerBook(NewTradeTransaction.PairID, NewTradeTransaction.BidPrice);
                        foreach (var model in list)
                        {
                            BuySellmodel = model;
                            break;
                        }
                        if (BuySellmodel.OrderId .ToString()!= "00000000-0000-0000-0000-000000000000")
                        {
                            BuyerBook(BuySellmodel, NewTradeTransaction.PairName);
                            HelperForLog.WriteLogIntoFile("OnStatusHold", ControllerName, "BuyerBook call TRNNO:" + Newtransaction.Id +" Pair :"+ NewTradeTransaction.PairName);
                        }
                        
                    }
                    else//Sell
                    {
                        list = _frontTrnRepository.GetSellerBook(NewTradeTransaction.PairID, NewTradeTransaction.AskPrice);
                        foreach (var model in list)
                        {
                            BuySellmodel = model;
                            break;
                        }
                        if(BuySellmodel.OrderId.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            SellerBook(BuySellmodel, NewTradeTransaction.PairName);
                            HelperForLog.WriteLogIntoFile("OnStatusHold", ControllerName, "SellerBook call TRNNO:" + Newtransaction.Id + " Pair :" + NewTradeTransaction.PairName);
                        }
                        
                    }
                    //var msg = EnResponseMessage.SignalRTrnSuccessfullyCreated;
                    //msg = msg.Replace("#Price#", historyInfo.Price.ToString());
                    //msg = msg.Replace("#Qty#", historyInfo.Amount.ToString());
                    //ActivityNotification(msg, Token);
                   
                    GetAndSendOpenOrderData(Newtransaction, NewTradeTransaction,Token, OrderType);
                    HelperForLog.WriteLogIntoFile("OnStatusHold", ControllerName, " Aftre Open Order Socket call  TRNNO:" + Newtransaction.Id);
                    GetAndSendRecentOrderData(Newtransaction, NewTradeTransaction,Token, OrderType);
                    HelperForLog.WriteLogIntoFile("OnStatusHold", ControllerName, " Aftre Recent Order Socket call  TRNNO:" + Newtransaction.Id);
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                ////throw ex;
            }
        }
        public void OnStatusCancel(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType)
        {
            try
            {
                GetTradeHistoryInfo historyInfo = new GetTradeHistoryInfo();

                //pop from OpenOrder
                //update Recent order
                //add Trade history
                //HelperForLog.WriteLogIntoFile("OnStatusCancel", ControllerName, " TransactionQueue :" + JsonConvert.SerializeObject(Newtransaction) + " TradeTransactionQueue :" + JsonConvert.SerializeObject(NewTradeTransaction));
                if (string.IsNullOrEmpty(Token))
                {
                    Token = GetTokenByUserID(NewTradeTransaction.MemberID.ToString());
                }
                if (!string.IsNullOrEmpty(Token))
                {
                    GetAndSendOpenOrderData(Newtransaction, NewTradeTransaction,Token, OrderType, 1);//with amount 0, remove from OpenOrder
                    HelperForLog.WriteLogIntoFile("OnStatusCancel", ControllerName, " Aftre Open Order Socket call : TRNNO:" + Newtransaction.Id);
                    GetAndSendRecentOrderData(Newtransaction, NewTradeTransaction,Token, OrderType);//Update Recent
                    HelperForLog.WriteLogIntoFile("OnStatusCancel", ControllerName, " Aftre Recent Order Socket call : TRNNO:" + Newtransaction.Id);
                    historyInfo = GetAndSendTradeHistoryInfoData(Newtransaction, NewTradeTransaction, OrderType);
                    TradeHistory(historyInfo, Token);//TradeHistory
                    HelperForLog.WriteLogIntoFile("OnStatusCancel", ControllerName, " Aftre Trade History Socket call : TRNNO:" + Newtransaction.Id);
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //throw ex;
            }
               //public void OnWalletBalChangeByUserID(WalletMasterResponse Data, string WalletTypeName, long UserID)
        //{
        //    try
        //    {
        //        //string str = "clientId=cleanarchitecture&grant_type=password&username=user@user.com&password=P@ssw0rd!&scope=openid profile email offline_access client_id roles phone";
        //        //str = str.Replace("=","\":\"");
        //        //str = str.Replace("&", "\",\"");
        //        //str = "{\"" + str + "\"}";
        //        //var obj = JsonConvert.DeserializeObject(str);
        //        //var jsonData= JsonConvert.SerializeObject(obj);
        //        var Redis = new RadisServices<ConnetedClientToken>(this._fact);
        //        string AccessToken = Redis.GetHashData("Tokens:" + UserID.ToString(), "Token");
        //        Token = AccessToken;
        //        BuyerSideWalletBal(Data, WalletTypeName, Token);
        //        SellerSideWalletBal(Data, WalletTypeName, Token);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
        //        //throw ex;
        //    }
        }
    }
    
}