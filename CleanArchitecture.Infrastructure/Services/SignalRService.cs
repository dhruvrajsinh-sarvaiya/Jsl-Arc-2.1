using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly ILogger<SignalRService> _logger;
        private readonly IMediator _mediator;
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly IFrontTrnRepository _frontTrnRepository;
        public String Token;
        public SignalRService(ILogger<SignalRService> logger, IMediator mediator, EFCommonRepository<TransactionQueue> TransactionRepository, IFrontTrnRepository frontTrnRepository,
            EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _TransactionRepository = TransactionRepository;
            _frontTrnRepository = frontTrnRepository;
            _TradeTransactionRepository = TradeTransactionRepository;
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
                CommonData.Subscription = Enum.GetName(typeof(enSubscriptionType), enSubscriptionType.Broadcast);
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
                throw ex;
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
                throw ex;
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
                CommonData.Parameter = Token;

                //SignalRDataOpenOrder SendData = new SignalRDataOpenOrder();
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
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.BuyerSideWallet;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                SendData.WalletName = Wallet;

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
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
                CommonData.Parameter = Token;

                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.SellerSideWallet;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                SendData.WalletName = Wallet;

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
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
                CommonData.Parameter = Token;

                //SignalRDataNotify SendData = new SignalRDataNotify();
                SignalRData SendData = new SignalRData();
                SendData.Method = enMethodName.ActivityNotification;
                SendData.Parameter = CommonData.Parameter;
                SendData.DataObj = JsonConvert.SerializeObject(CommonData);
                //SendData.WalletName = Wallet;

                _mediator.Send(SendData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
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
                throw ex;
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
                throw ex;
            }
        }
        #endregion

        public void OnStatusChange(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token)
        {
            try
            {
                if (Status == Convert.ToInt16(enTransactionStatus.Hold))
                {
                    GetBuySellBook BuySellmodel = new GetBuySellBook();
                    List<GetBuySellBook> list = new List<GetBuySellBook>();
                    if (!string.IsNullOrEmpty(Token))
                        if (NewTradeTransaction.TrnType == 4)//Buy
                        {
                            list = _frontTrnRepository.GetBuyerBook(NewTradeTransaction.PairID, NewTradeTransaction.BidPrice);
                            foreach (var model in list)
                            {
                                BuySellmodel = model;
                                break;
                            }
                            BuyerBook(BuySellmodel, NewTradeTransaction.PairName);
                        }
                        else//Sell
                        {
                            list = _frontTrnRepository.GetSellerBook(NewTradeTransaction.PairID, NewTradeTransaction.AskPrice);
                            foreach (var model in list)
                            {
                                BuySellmodel = model;
                                break;
                            }
                            SellerBook(BuySellmodel, NewTradeTransaction.PairName);
                        }
                    GetAndSendOpenOrderData(Newtransaction, NewTradeTransaction);
                    ActivityNotification(EnResponseMessage.SignalRTrnSuccessfullyCreated, Token);
                }
                else if (Status == Convert.ToInt16(enTransactionStatus.Success))
                {
                    GetAndSendOpenOrderData(Newtransaction, NewTradeTransaction, 1);


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void GetAndSendOpenOrderData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short IsPop = 0)
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

                OpenOrder(OpenOrderModel, Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void GetAndSendOrderHistory(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short IsPop = 0)
        {
            try
            {
                var OrderHistoryList = _frontTrnRepository.GetTradeHistory(0, "", "", "", 0, 0, Newtransaction.Id);
                GetTradeHistoryInfo model = new GetTradeHistoryInfo();
                model.TrnNo = NewTradeTransaction.TrnNo;
                model.Type = (NewTradeTransaction.TrnType == 4) ? "BUY" : "SELL";
                model.Price = (NewTradeTransaction.BidPrice == 0) ? NewTradeTransaction.AskPrice : (NewTradeTransaction.AskPrice == 0) ? NewTradeTransaction.BidPrice : NewTradeTransaction.BidPrice;
                model.Amount = (NewTradeTransaction.TrnType == 4) ? NewTradeTransaction.SettledBuyQty : NewTradeTransaction.SettledSellQty;
                model.Total = model.Type == "BUY" ? ((model.Price * model.Amount) - model.ChargeRs) : ((model.Price * model.Amount));
                model.DateTime = Convert.ToDateTime(NewTradeTransaction.SettledDate);
                model.Status = NewTradeTransaction.Status;
                model.StatusText = NewTradeTransaction.StatusMsg;
                model.PairName = NewTradeTransaction.PairName;
                model.ChargeRs = Convert.ToDecimal(Newtransaction.ChargeRs);
                model.IsCancel = NewTradeTransaction.IsCancelled;

                OrderHistory(model, Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void GetAndSendTradeHistoryByUser(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction)
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
                model.StatusText = NewTradeTransaction.StatusMsg;
                model.PairName = NewTradeTransaction.PairName;
                model.ChargeRs = Convert.ToDecimal(Newtransaction.ChargeRs);
                model.IsCancel = NewTradeTransaction.IsCancelled;

                TradeHistoryByUser(model, Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void GetAndSendTradeHistoryByPair(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction)
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
                model.StatusText = NewTradeTransaction.StatusMsg;
                model.PairName = NewTradeTransaction.PairName;
                model.ChargeRs = Convert.ToDecimal(Newtransaction.ChargeRs);
                model.IsCancel = NewTradeTransaction.IsCancelled;

                TradingHistoryByPair(model, model.PairName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}