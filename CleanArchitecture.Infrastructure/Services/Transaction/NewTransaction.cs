using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    //[Guid("9245fe4a-d402-451c-b9ed-9c1a04247482")]
    public class NewTransaction : ITransactionProcess
    {
        private readonly ICommonRepository<TradePairMaster> _TradePairMaster;
        private readonly ICommonRepository<TradePairDetail> _TradePairDetail;
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly EFCommonRepository<TradeStopLoss> _TradeStopLoss;
        private readonly ICommonRepository<ServiceMaster> _ServiceConfi;
        private readonly ICommonRepository<AddressMaster> _AddressMasterRepository;
        private readonly ILogger<NewTransaction> _log;
        private readonly IWalletService _WalletService;
        private readonly IWebApiRepository _WebApiRepository;
        private readonly ICommonRepository<TransactionRequest> _TransactionRequest;
        private readonly IGetWebRequest _IGetWebRequest;
        private readonly IWebApiSendRequest _IWebApiSendRequest;
        private readonly IWebApiData _IWebApiData;
        WebApiParseResponse _WebApiParseResponseObj;

        public BizResponse _Resp;        
        public BizResponse _CreateTransactionResp;
        public TradePairMaster _TradePairObj;
        public TradePairDetail _TradePairDetailObj;
        public List<TransactionProviderResponse> TxnProviderList;

        TransactionQueue Newtransaction;
        TradeTransactionQueue NewTradetransaction;
        NewTransactionRequestCls Req;
        NewTradeTransactionRequestCls _TradeTransactionObj = new NewTradeTransactionRequestCls();
        ProcessTransactionCls _TransactionObj;
        ServiceMaster _BaseCurrService;
        ServiceMaster _SecondCurrService;
        ServiceMaster _TrnService;
        TransactionRequest NewtransactionReq;
        private string ControllerName = "TradingTransaction";

        public NewTransaction(ILogger<NewTransaction> log, ICommonRepository<TradePairMaster> TradePairMaster,
            EFCommonRepository<TransactionQueue> TransactionRepository,
            EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository,
            ICommonRepository<ServiceMaster> ServiceConfi, ICommonRepository<AddressMaster> AddressMasterRepository,
            EFCommonRepository<TradeStopLoss> tradeStopLoss, IWalletService WalletService,
            ICommonRepository<TradePairDetail> TradePairDetail, IWebApiRepository WebApiRepository,
            ICommonRepository<TransactionRequest> TransactionRequest, IGetWebRequest IGetWebRequest,
            IWebApiSendRequest WebApiSendRequest, WebApiParseResponse WebApiParseResponseObj, IWebApiData IWebApiData)
        {
            _log = log;
            _TradePairMaster = TradePairMaster;
            _TransactionRepository = TransactionRepository;
            _TradeTransactionRepository = TradeTransactionRepository;
            _ServiceConfi = ServiceConfi;
            _AddressMasterRepository = AddressMasterRepository;
            _TradeStopLoss = tradeStopLoss;
            _WalletService = WalletService;
            _TradePairDetail = TradePairDetail;
            _WebApiRepository = WebApiRepository;
            _TransactionRequest = TransactionRequest;
            _IGetWebRequest = IGetWebRequest;
            _IWebApiSendRequest = WebApiSendRequest;
            _WebApiParseResponseObj = WebApiParseResponseObj;
            _IWebApiData = IWebApiData;
        }
        public async Task<BizResponse> ProcessNewTransactionAsync(NewTransactionRequestCls Req1)
        {
            Req = Req1;
            //_Resp = new BizResponse();            

            _Resp = CreateTransaction();
            if (_Resp.ReturnCode != enResponseCodeService.Success)
            {
                HelperForLog.WriteLogIntoFile(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, _Resp.ReturnMsg + "##TrnNo:" + Req.TrnNo);
                return _Resp;
            }
            //var myResp = new Task(async () => CombineAllInitTransactionAsync());
            Task.Run(() => CombineAllInitTransactionAsync());

            //CombineAllInitTransactionAsync();

            //return await Task.FromResult(new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess });
            //_Resp = await MethodRespTsk;            
            return new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess };
            //return _Resp;
        }

        public BizResponse CombineAllInitTransactionAsync()
        {
            _Resp = new BizResponse();
            try
            {
                //Helpers.JsonSerialize(null);
                //=========================PROCESS
                //Check balance here
                var Validation = ValidateTransaction(_Resp);                

                if (!Validation.Result) //validation and balance check success
                {
                    MarkTransactionSystemFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                    return _Resp;
                }
                var BalResult = _WalletService.WalletBalanceCheck(Req.Amount, Req.DebitAccountID); //DI of Wallet for balance check
                if (!BalResult) //validation and balance check success
                {
                    _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_InsufficientBalanceMsg;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_InsufficientBalance;
                    MarkTransactionSystemFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                    return _Resp;
                }
                //Deduct balance here
                if(Req.TrnType == enTrnType.Transaction)
                {
                    //ServiceType
                    Req.ServiceType = (enServiceType)_TrnService.ServiceType;
                    //Req.WalletTrnType = enWalletTrnType.;

                }
                else if (Req.TrnType == enTrnType.Withdraw)
                {
                    //ServiceType
                    Req.ServiceType = (enServiceType)_TrnService.ServiceType;
                    Req.WalletTrnType = enWalletTrnType.Dr_Withdrawal;

                }
                else//trading
                {
                    Req.ServiceType = enServiceType.Trading;
                    Req.WalletTrnType = enWalletTrnType.Dr_Sell_Trade;
                }

                var DebitResult = _WalletService.GetWalletDeductionNew(Req.SMSCode, "", enWalletTranxOrderType.Debit, Req.Amount, Req.MemberID,
                    Req.DebitAccountID, Req.TrnNo, Req.ServiceType, Req.WalletTrnType);

                if (DebitResult.ReturnCode == enResponseCode.Fail)
                {
                    _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_WalletDebitFailMsg;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_WalletDebitFail;
                    MarkTransactionSystemFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                    return _Resp;
                }
                //Make txn hold as balance debited
                MarkTransactionHold(EnResponseMessage.ProcessTrn_HoldMsg, enErrorCode.ProcessTrn_Hold);
                if (Req.TrnType == enTrnType.Transaction || Req.TrnType == enTrnType.Withdraw)
                {
                    CallWebAPI(_Resp);
                    return _Resp;
                }
                else//Trading process here
                {

                }

                //=========================UPDATE
                return null;
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog("CombineAllInitTransactionAsync", ControllerName, ex);                
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionProcessInternalError });
            }            
        }

        #region RegionInitTransaction    
        public BizResponse CreateTransaction()
        {
            //long DeliveryWalletID = 0;           
            //string INRSMSCode = "INR";
            decimal Amount2 = 0;
            //string MemberMobile1 = "";            
            //NewTradeTransactionRequestCls _TradeTransactionObj = new NewTradeTransactionRequestCls();
            //long TrnNo = 0;
            try
            {
                if (Req.TrnType == enTrnType.Buy_Trade)
                {
                    _TradeTransactionObj.TrnTypeName = "BUY";                    
                }
                else if (Req.TrnType == enTrnType.Sell_Trade)
                {
                    _TradeTransactionObj.TrnTypeName = "SELL";
                }
                Req.DebitWalletID = Convert.ToInt64(Req.DebitAccountID);//Assign from wallet team
                Req.CreditWalletID = Convert.ToInt64(Req.CreditAccountID);
                //IF @PairID <> 0 ntrivedi 18-04-2018  check inside @TrnType (4,5) @TradeWalletMasterID will be 0 or null
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    //_TradePairObj = new TradePairMaster();
                    _TradePairObj = _TradePairMaster.GetSingle(item => item.Id == Req.PairID && item.Status == Convert.ToInt16(ServiceStatus.Active));                   
                    if (_TradePairObj == null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnNoPairSelectedMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrn_NoPairSelected);
                    }
                    _TradeTransactionObj.PairName = _TradePairObj.PairName;
                    _TradePairDetailObj = _TradePairDetail.GetSingle(item => item.PairId == Req.PairID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                    if (_TradePairDetailObj == null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnNoPairSelectedMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrn_NoPairSelected);
                    }
                    if (Req.Qty <= 0 || Req.Price <= 0)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidQtyPriceMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrnInvalidQtyPrice);
                    }
                    _BaseCurrService = _ServiceConfi.GetSingle(item => item.Id == _TradePairObj.BaseCurrencyId);
                    _SecondCurrService = _ServiceConfi.GetSingle(item => item.Id == _TradePairObj.SecondaryCurrencyId);
                    if (Req.TrnType == enTrnType.Buy_Trade)
                    {
                        _TradeTransactionObj.BuyQty = Req.Qty;
                        _TradeTransactionObj.BidPrice = Req.Price;
                        _TradeTransactionObj.DeliveryTotalQty = Req.Qty;
                        _TradeTransactionObj.OrderTotalQty = Helpers.DoRoundForTrading(Req.Qty * Req.Price, 8);//235.415001286,8 =  235.41500129                         
                        _TradeTransactionObj.Order_Currency = _BaseCurrService.SMSCode;
                        _TradeTransactionObj.Delivery_Currency = _SecondCurrService.SMSCode;
                    }
                    if (Req.TrnType == enTrnType.Sell_Trade)
                    {
                        _TradeTransactionObj.SellQty = Req.Qty;
                        _TradeTransactionObj.AskPrice = Req.Price;
                        _TradeTransactionObj.OrderTotalQty = Req.Qty;
                        _TradeTransactionObj.DeliveryTotalQty = Helpers.DoRoundForTrading(Req.Qty * Req.Price, 8);//235.415001286,8 =  235.41500129                        
                        _TradeTransactionObj.Order_Currency = _SecondCurrService.SMSCode;
                        _TradeTransactionObj.Delivery_Currency = _BaseCurrService.SMSCode;
                    }
                    if (_TradeTransactionObj.OrderTotalQty < (decimal)(0.00000001) || _TradeTransactionObj.DeliveryTotalQty < (decimal)(0.00000001))
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidQtyNAmountMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrnInvalidQtyNAmount);
                    }
                    Req.SMSCode = _TradeTransactionObj.Order_Currency;
                    //Balace check Here , take DeliveryWalletID output
                    _TradeTransactionObj.DeliveryWalletID = Req.CreditWalletID;
                    _TradeTransactionObj.OrderWalletID = Req.DebitWalletID;

                    Req.Amount = _TradeTransactionObj.OrderTotalQty;
                    if (_TradeTransactionObj.DeliveryWalletID == 0)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrn_NoCreditAccountFoundMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrn_NoCreditAccountFound);
                    }
                }
                if (Req.TrnType == enTrnType.Shoping_Cart)//IF TRNTYPE = 7 THEN FETCH BALANCE OF INR WALLET 
                {
                    Amount2 = Convert.ToDecimal(Req.AdditionalInfo);
                    if(Amount2 < Req.Amount)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidAmountMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrnInvalidAmount);
                    }
                    //Take balance base on @INRSMSCode and take OrderWalletID,Balance
                }
                else
                {
                    _TrnService = _ServiceConfi.GetSingle(item => item.SMSCode == Req.SMSCode && item.Status == Convert.ToInt16(ServiceStatus.Active));
                    if (_TrnService == null)
                    {
                        Req.StatusMsg = EnResponseMessage.ProcessTrn_ServiceProductNotAvailableMsg;
                        return MarkSystemFailTransaction(enErrorCode.ProcessTrn_ServiceProductNotAvailable);
                    }
                    //Take balance base on @SMSCode and take OrderWalletID,Balance
                }
                //if(_TradeTransactionObj.OrderWalletID==0)
                //{
                //    Req.StatusMsg = EnResponseMessage.CreateTrn_NoDebitAccountFoundMsg;
                //    return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrn_NoDebitAccountFound);
                //}
                if (Req.TrnType != enTrnType.Buy_Trade && Req.TrnType != enTrnType.Sell_Trade && Req.SMSCode != "IMP") //not for trade and MNTR txn
                {
                    var DuplicateTxn = _TransactionRepository.GetSingle(item => item.MemberMobile == Req.MemberMobile && item.TransactionAccount == Req.TransactionAccount && item.Amount == Req.Amount && item.TrnDate.AddMinutes(10) > Helpers.UTC_To_IST());
                    if (DuplicateTxn != null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnDuplicateTrnMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrnDuplicateTrn);
                    }
                }
                 
                if(Req.Amount<0)
                {
                    Req.StatusMsg = EnResponseMessage.CreateTrnInvalidAmountMsg;
                    return MarkSystemFailTransaction(enErrorCode.CreateTrnInvalidAmount);
                }
                if (Req.TrnRefNo != "0" || !string.IsNullOrEmpty(Req.TrnRefNo))
                {
                    var DuplicateTxnWithRefNo = _TransactionRepository.GetSingle(item => item.MemberID == Req.MemberID && item.TrnRefNo == Req.TrnRefNo && item.TrnType == Convert.ToInt16(enTrnType.Transaction));
                    if (DuplicateTxnWithRefNo != null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnDuplicateTrnMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrnDuplicateTrn);
                    }
                }
                if (Req.TrnType == enTrnType.Withdraw)
                {
                    var _AddressMasterObj = _AddressMasterRepository.GetSingle(item => item.WalletId == Req.DebitWalletID && item.Address == Req.TransactionAccount && item.Status == Convert.ToInt16(ServiceStatus.Active));//in withdraw , TransactionAccount has address
                    if (_AddressMasterObj != null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrn_NoSelfAddressWithdrawAllowMsg;
                        return MarkSystemFailTransaction(enErrorCode.CreateTrn_NoSelfAddressWithdrawAllow);
                    }
                }
                Req.Status = enTransactionStatus.Initialize;
                InsertTransactionInQueue();               
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    InsertTradeTransactionInQueue();
                    InsertTradeStopLoss();
                }
                   
                return (new BizResponse { ReturnMsg = "", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }

        }
        public BizResponse MarkSystemFailTransaction(enErrorCode ErrorCode)
        {
            try
            {
                Req.Status = enTransactionStatus.SystemFail;
                InsertTransactionInQueue();              
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    InsertTradeTransactionInQueue();
                    InsertTradeStopLoss();
                }
                //DI of SMS here
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.Fail, ErrorCode = ErrorCode });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }
        }
        public BizResponse InsertTransactionInQueue()//ref long TrnNo
        {
            _Resp = new BizResponse();
            try
            {
                Newtransaction = new TransactionQueue()
                {
                    TrnDate = Helpers.UTC_To_IST(),
                    GUID = Guid.NewGuid(),
                    TrnMode = Req.TrnMode,
                    TrnType = Convert.ToInt16(Req.TrnType),
                    MemberID = Req.MemberID,
                    MemberMobile = Req.MemberMobile,
                    TransactionAccount = Req.TransactionAccount,
                    SMSCode = Req.SMSCode,
                    Amount = Req.Amount,
                    Status = Convert.ToInt16(Req.Status),
                    StatusCode = 0,
                    StatusMsg = Req.StatusMsg,
                    TrnRefNo = Req.TrnRefNo,
                    AdditionalInfo = Req.AdditionalInfo
                };
                Newtransaction = _TransactionRepository.Add(Newtransaction);
                Req.TrnNo = Newtransaction.Id;
                Req.GUID = Newtransaction.GUID;

                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public BizResponse InsertTradeTransactionInQueue()
        {
            try
            {
                NewTradetransaction = new TradeTransactionQueue()
                {
                    TrnDate= Helpers.UTC_To_IST(),
                    TrnNo = Req.TrnNo,//NewTradetransactionReq.TrnNo,
                    TrnType = Convert.ToInt16(Req.TrnType),
                    TrnTypeName = _TradeTransactionObj.TrnTypeName,
                    MemberID = Req.MemberID,
                    PairID = Req.PairID,
                    PairName = _TradeTransactionObj.PairName,
                    OrderWalletID = _TradeTransactionObj.OrderWalletID,
                    DeliveryWalletID = _TradeTransactionObj.DeliveryWalletID,
                    BuyQty = _TradeTransactionObj.BuyQty,
                    BidPrice = _TradeTransactionObj.BidPrice,
                    SellQty = _TradeTransactionObj.SellQty,
                    AskPrice = _TradeTransactionObj.AskPrice,
                    Order_Currency = _TradeTransactionObj.Order_Currency,
                    OrderTotalQty = _TradeTransactionObj.OrderTotalQty,
                    Delivery_Currency = _TradeTransactionObj.Delivery_Currency,
                    DeliveryTotalQty = _TradeTransactionObj.DeliveryTotalQty,
                    SettledBuyQty = _TradeTransactionObj.SettledBuyQty,
                    SettledSellQty = _TradeTransactionObj.SettledSellQty,
                    Status = Convert.ToInt16(Req.Status),
                    StatusCode = Req.StatusCode,
                    StatusMsg = Req.StatusMsg
                };
                _TradeTransactionRepository.Add(NewTradetransaction);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public BizResponse InsertTradeStopLoss()
        {
            try
            {
                var Newtransaction = new TradeStopLoss()
                {
                    TrnNo = Req.TrnNo,
                    ordertype = Convert.ToInt16(Req.ordertype)                   
                };
                _TradeStopLoss.Add(Newtransaction);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        #endregion        

        #region RegionProcessTransaction
        public Task<Boolean> ValidateTransaction(BizResponse _Resp)
        {
            _TransactionObj =new ProcessTransactionCls();
            //Member Service Disable check here for regular txn
            try
            {
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade) //Only for trade
                {
                    if (Req.TrnType == enTrnType.Buy_Trade)
                    {
                        _TransactionObj.BidPrice_TQ = _TradeTransactionObj.BidPrice;
                        _TransactionObj.BidPrice_BuyReq = Helpers.DoRoundForTrading(1 / _TradeTransactionObj.BidPrice, 8);
                        //_TradePairDetailObj
                        if (_TradeTransactionObj.BuyQty < _TradePairDetailObj.BuyMinQty || _TradeTransactionObj.BuyQty > _TradePairDetailObj.BuyMaxQty)
                        {
                            _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_AmountBetweenMinMaxMsg.Replace("@MIN", _TradePairDetailObj.BuyMinQty.ToString()).Replace("@MAX", _TradePairDetailObj.BuyMaxQty.ToString());
                            _Resp.ReturnCode = enResponseCodeService.Fail;
                            _Resp.ErrorCode = enErrorCode.ProcessTrn_AmountBetweenMinMax;
                            return Task.FromResult(false);
                        }
                        if (_TradeTransactionObj.BidPrice < _TradePairDetailObj.BuyMinPrice || _TradeTransactionObj.BidPrice > _TradePairDetailObj.BuyMaxPrice)
                        {
                            _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_PriceBetweenMinMaxMsg.Replace("@MIN", _TradePairDetailObj.BuyMinPrice.ToString()).Replace("@MAX", _TradePairDetailObj.BuyMaxPrice.ToString());
                            _Resp.ReturnCode = enResponseCodeService.Fail;
                            _Resp.ErrorCode = enErrorCode.ProcessTrn_PriceBetweenMinMax;
                            return Task.FromResult(false);
                        }
                        _TransactionObj.Delivery_ServiceID = _SecondCurrService.Id;

                    }
                    else if (Req.TrnType == enTrnType.Sell_Trade)
                    {
                        _TransactionObj.BidPrice_TQ = Helpers.DoRoundForTrading(1 / _TradeTransactionObj.AskPrice, 8);
                        _TransactionObj.BidPrice_BuyReq = _TradeTransactionObj.AskPrice;
                        //_TradePairDetailObj
                        if (_TradeTransactionObj.SellQty < _TradePairDetailObj.SellMinQty || _TradeTransactionObj.SellQty > _TradePairDetailObj.SellMaxQty)
                        {
                            _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_AmountBetweenMinMaxMsg.Replace("@MIN", _TradePairDetailObj.SellMinQty.ToString()).Replace("@MAX", _TradePairDetailObj.SellMaxQty.ToString());
                            _Resp.ReturnCode = enResponseCodeService.Fail;
                            _Resp.ErrorCode = enErrorCode.ProcessTrn_AmountBetweenMinMax;
                            return Task.FromResult(false);
                        }
                        if (_TradeTransactionObj.AskPrice < _TradePairDetailObj.SellMinPrice || _TradeTransactionObj.AskPrice > _TradePairDetailObj.SellMaxPrice)
                        {
                            _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_PriceBetweenMinMaxMsg.Replace("@MIN", _TradePairDetailObj.SellMinPrice.ToString()).Replace("@MAX", _TradePairDetailObj.SellMaxPrice.ToString());
                            _Resp.ReturnCode = enResponseCodeService.Fail;
                            _Resp.ErrorCode = enErrorCode.ProcessTrn_PriceBetweenMinMax;
                            return Task.FromResult(false);
                        }
                        _TransactionObj.Delivery_ServiceID = _BaseCurrService.Id;

                    }
                    if (_TransactionObj.BidPrice_TQ == 0 || _TransactionObj.BidPrice_BuyReq == 0)
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_InvalidBidPriceValueMsg;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_InvalidBidPriceValue;
                        _Resp.ReturnCode = enResponseCodeService.Fail;
                        return Task.FromResult(false);
                    }
                    //_TransactionObj.Delivery_ServiceID = _ServiceConfi.GetSingle(item => item.SMSCode == _TradeTransactionObj.Delivery_Currency).Id;
                    //NOTE : Make Pool Order here
                    if (_TransactionObj.Pool_OrderID == 0)
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_PoolOrderCreateFailMsg;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_PoolOrderCreateFail;
                        _Resp.ReturnCode = enResponseCodeService.Fail;
                        return Task.FromResult(false);
                    }
                }
                TxnProviderList = _WebApiRepository.GetProviderDataList(new TransactionApiConfigurationRequest { amount = Req.Amount, SMSCode = Req.SMSCode, APIType = enWebAPIRouteType.TransactionAPI, trnType = Convert.ToInt32(Req.TrnType) });
                if (TxnProviderList == null)
                {
                    _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_ServiceProductNotAvailableMsg;
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_ServiceProductNotAvailable;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    return Task.FromResult(false);
                }
                //Make Transaction Initialise
                //var Txn = _TransactionRepository.GetById(Req.TrnNo);
                Newtransaction.MakeTransactionInProcess();
                Newtransaction.SetTransactionStatusMsg(EnResponseMessage.ProcessTrn_InitializeMsg);
                Newtransaction.SetTransactionCode(Convert.ToInt64(enErrorCode.ProcessTrn_Initialize));
                _TransactionRepository.Update(Newtransaction);
                //Take Provider Configuration           

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                return Task.FromResult(false);
            }

           
        }
        public void MarkTransactionSystemFail(string StatusMsg,enErrorCode ErrorCode)
        {
            try
            {
                //var Txn = _TransactionRepository.GetById(Req.TrnNo);
                Newtransaction.MakeTransactionSystemFail();
                Newtransaction.SetTransactionStatusMsg(StatusMsg);
                Newtransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                _TransactionRepository.Update(Newtransaction);

                //var TradeTxn = _TradeTransactionRepository.GetById(Req.TrnNo);
                NewTradetransaction.MakeTransactionSystemFail();
                NewTradetransaction.SetTransactionStatusMsg(StatusMsg);
                NewTradetransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                _TradeTransactionRepository.Update(NewTradetransaction);                
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                throw ex;
            } 
        }
        public void MarkTransactionHold(string StatusMsg, enErrorCode ErrorCode)
        {
            try
            {
                //var Txn = _TransactionRepository.GetById(Req.TrnNo);
                Newtransaction.MakeTransactionHold();
                Newtransaction.SetTransactionStatusMsg(StatusMsg);
                Newtransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                _TransactionRepository.Update(Newtransaction);              
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                throw ex;
            }
        }
        public void MarkTransactionOperatorFail(string StatusMsg, enErrorCode ErrorCode)
        {
            CreditWalletDrArryTrnID[] CreditWalletDrArryTrnIDObj = new CreditWalletDrArryTrnID[1];
            try
            {
                //var Txn = _TransactionRepository.GetById(Req.TrnNo);
                Newtransaction.MakeTransactionOperatorFail();
                Newtransaction.SetTransactionStatusMsg(StatusMsg);
                Newtransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                _TransactionRepository.Update(Newtransaction);

                //Cr Amount to member back
                CreditWalletDrArryTrnIDObj[0].DrTrnRefNo = Req.TrnNo;
                CreditWalletDrArryTrnIDObj[0].Amount = Req.Amount;

                _WalletService.GetWalletCreditNew(Req.SMSCode, "", enWalletTrnType.Cr_Refund, Req.Amount, Req.MemberID,
                Req.DebitAccountID, CreditWalletDrArryTrnIDObj, Req.TrnNo,1, enWalletTranxOrderType.Credit, enServiceType.Recharge);
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                throw ex;
            }
        }
        public void CallWebAPI(BizResponse _Resp)
        {
            //TransactionRequest TransactionRequestObj=new TransactionRequest(); 
            ThirdPartyAPIRequest ThirdPartyAPIRequestOnj;
            WebApiConfigurationResponse WebApiConfigurationResponseObj;
            WebAPIParseResponseCls WebAPIParseResponseClsObj=new WebAPIParseResponseCls();
            //long TxnRequestID = 0;
            short IsTxnProceed = 0;
            try
            {
                foreach (TransactionProviderResponse Provider in TxnProviderList)//Make txn on every API
                {

                    WebApiConfigurationResponseObj=_IWebApiData.GetAPIConfiguration(Provider.ThirPartyAPIID);
                    if(WebApiConfigurationResponseObj==null)
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_ThirdPartyDataNotFoundMsg;
                        _Resp.ReturnCode = enResponseCodeService.Fail;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_ThirdPartyDataNotFound;
                        MarkTransactionOperatorFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                        continue;
                    }
                    ThirdPartyAPIRequestOnj =_IGetWebRequest.MakeWebRequest(Provider.RouteID,Provider.ThirPartyAPIID,Provider.SerProDetailID, Newtransaction);
                    Newtransaction.SetServiceProviderData(Provider.ServiceID, Provider.ServiceProID, Provider.ProductID, Provider.RouteID);
                    //Insert API request Data
                    _TransactionObj.TransactionRequestID = InsertTransactionRequest(Provider, ThirdPartyAPIRequestOnj.RequestURL + "::" + ThirdPartyAPIRequestOnj.RequestBody);

                    switch (Provider.AppTypeID)
                    {
                        case (long)enAppType.WebSocket:

                        case (long)enAppType.JsonRPC:
                            //_IWebApiSendRequest.SendJsonRpcAPIRequestAsync(ThirdPartyAPIRequestOnj.RequestURL,);
                                 break;
                        case (long)enAppType.TCPSocket:

                        case (long)enAppType.RestAPI:
                            _TransactionObj.APIResponse = _IWebApiSendRequest.SendAPIRequestAsync(ThirdPartyAPIRequestOnj.RequestURL, ThirdPartyAPIRequestOnj.RequestBody, WebApiConfigurationResponseObj.ContentType, 30000, ThirdPartyAPIRequestOnj.keyValuePairsHeader, WebApiConfigurationResponseObj.MethodType);
                            break;
                        case (long)enAppType.HttpApi:
                            _TransactionObj.APIResponse =_IWebApiSendRequest.SendAPIRequestAsync(ThirdPartyAPIRequestOnj.RequestURL, ThirdPartyAPIRequestOnj.RequestBody, WebApiConfigurationResponseObj.ContentType, 30000, ThirdPartyAPIRequestOnj.keyValuePairsHeader, WebApiConfigurationResponseObj.MethodType);
                            break;
                        case (long)enAppType.SocketApi:

                        case (long)enAppType.BitcoinDeamon:

                        default:
                            Console.WriteLine("Default case");
                            break;
                    }                    
                    NewtransactionReq.SetResponse(_TransactionObj.APIResponse);
                    NewtransactionReq.SetResponseTime(Helpers.UTC_To_IST());
                    if (string.IsNullOrEmpty(_TransactionObj.APIResponse))
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_HoldMsg;
                        _Resp.ReturnCode = enResponseCodeService.Success;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_GettingResponseBlank;
                        MarkTransactionHold(_Resp.ReturnMsg, _Resp.ErrorCode);
                        IsTxnProceed = 1;//no further call next API
                        break;
                    }

                    WebAPIParseResponseClsObj= _WebApiParseResponseObj.TransactionParseResponse(_TransactionObj.APIResponse, Provider.ThirPartyAPIID);
                    NewtransactionReq.SetTrnID(WebAPIParseResponseClsObj.TrnRefNo);
                    NewtransactionReq.SetOprTrnID(WebAPIParseResponseClsObj.OperatorRefNo);
                    _TransactionRequest.Update(NewtransactionReq);

                    if (WebAPIParseResponseClsObj.Status == enTransactionStatus.Success)
                    {
                        Newtransaction.MakeTransactionSuccess();
                        Newtransaction.SetTransactionStatusMsg(WebAPIParseResponseClsObj.StatusMsg);                        
                        _TransactionRepository.Update(Newtransaction);
                        IsTxnProceed = 1;//no further call next API
                        break;
                    }
                    else if (WebAPIParseResponseClsObj.Status == enTransactionStatus.OperatorFail)
                    {
                        continue;
                    }
                    else
                    {
                        Newtransaction.SetTransactionStatusMsg(WebAPIParseResponseClsObj.StatusMsg);
                        _TransactionRepository.Update(Newtransaction);
                        IsTxnProceed = 1;//no further call next API
                        break;
                    }
                }
                if(IsTxnProceed==0)
                {
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_OprFail;
                    MarkTransactionOperatorFail(WebAPIParseResponseClsObj.StatusMsg, _Resp.ErrorCode);
                }

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);

            }
        }
        public long InsertTransactionRequest(TransactionProviderResponse listObj, string Request)
        {
            try
            {
                NewtransactionReq = new TransactionRequest()
                {
                    TrnNo = Req.TrnNo,
                    ServiceID = listObj.ServiceID,
                    SerProID = listObj.ServiceProID,
                    SerProDetailID = listObj.SerProDetailID,
                    CreatedDate = Helpers.UTC_To_IST(),
                    RequestData = Request
                };
                _TransactionRequest.Add(NewtransactionReq);
                return NewtransactionReq.Id;

            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ControllerName, ex);
                return 0;
            }
        }       
        #endregion


    }
}
