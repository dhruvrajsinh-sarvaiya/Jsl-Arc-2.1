﻿using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Data.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICommonRepository<PoolOrder> _PoolOrder;
        private readonly ICommonRepository<TradePoolMaster> _TradePoolMaster; 
        private readonly ICommonRepository<TradeBuyRequest> _TradeBuyRequest; 
        private readonly ICommonRepository<TradeSellerList> _TradeSellerList; 
        private readonly ICommonRepository<TradeBuyerList> _TradeBuyerList;
        private readonly ICommonRepository<TradePairStastics> _tradePairStastics;
        //private readonly ICommonRepository<TradePoolQueue> _TradePoolQueue; 
        private readonly ILogger<NewTransaction> _log;
        private readonly IWalletService _WalletService;
        private readonly IWebApiRepository _WebApiRepository;
        private readonly ICommonRepository<TransactionRequest> _TransactionRequest;
        private readonly IGetWebRequest _IGetWebRequest;
        private readonly IWebApiSendRequest _IWebApiSendRequest;
        private readonly IWebApiData _IWebApiData; 
        private readonly ISettlementRepository<BizResponse> _SettlementRepository;
        private readonly ISignalRService _ISignalRService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IMessageConfiguration _messageConfiguration;

        WebApiParseResponse _WebApiParseResponseObj;

        public BizResponse _Resp;        
        //public BizResponse _CreateTransactionResp;
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
        PoolOrder PoolOrderObj;
        TradePoolMaster TradePoolMasterObj;
        TradeBuyRequest TradeBuyRequestObj;
        TradeSellerList TradeSellerListObj;
        TradeStopLoss TradeStopLossObj;
        //TradeBuyerList TradeBuyerListObj;
        //TradePoolConfiguration TradePoolConfigurationObj;
        //TradePoolQueue TradePoolQueueObj;
        private string ControllerName = "TradingTransaction";

        public NewTransaction(ILogger<NewTransaction> log, ICommonRepository<TradePairMaster> TradePairMaster,
            EFCommonRepository<TransactionQueue> TransactionRepository,
            EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository,
            ICommonRepository<ServiceMaster> ServiceConfi, ICommonRepository<AddressMaster> AddressMasterRepository,
            EFCommonRepository<TradeStopLoss> tradeStopLoss, IWalletService WalletService,
            ICommonRepository<TradePairDetail> TradePairDetail, IWebApiRepository WebApiRepository,
            ICommonRepository<TransactionRequest> TransactionRequest, IGetWebRequest IGetWebRequest,
            IWebApiSendRequest WebApiSendRequest, WebApiParseResponse WebApiParseResponseObj, IWebApiData IWebApiData,
            ICommonRepository<PoolOrder> PoolOrder, ICommonRepository<TradePoolMaster> TradePoolMaster,
            ICommonRepository<TradeBuyRequest> TradeBuyRequest, ICommonRepository<TradeSellerList> TradeSellerList,
            ICommonRepository<TradeBuyerList> TradeBuyerList, ISettlementRepository<BizResponse> SettlementRepository, 
            ISignalRService ISignalRService, ICommonRepository<TradePairStastics> tradePairStastics,UserManager<ApplicationUser> userManager,
            IMediator mediator, IMessageConfiguration messageConfiguration)
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
            _PoolOrder = PoolOrder;
            _TradePoolMaster = TradePoolMaster;
            _TradeBuyRequest = TradeBuyRequest;
            _TradeSellerList = TradeSellerList;
            _TradeBuyerList = TradeBuyerList;
            _SettlementRepository = SettlementRepository;
            _ISignalRService = ISignalRService;
            _tradePairStastics = tradePairStastics;
            _userManager = userManager;
            _mediator = mediator;
            _messageConfiguration = messageConfiguration;
        }
        public async Task<BizResponse> ProcessNewTransactionAsync(NewTransactionRequestCls Req1)
        {
            Req = Req1;
            //_Resp = new BizResponse();            

            _Resp =CreateTransaction();
            if (_Resp.ReturnCode != enResponseCodeService.Success)
            {
                HelperForLog.WriteLogIntoFile("ProcessNewTransactionAsync", ControllerName, _Resp.ReturnMsg + "##TrnNo:" + Req.TrnNo);
                return _Resp;
            }
            //var myResp = new Task(async () => CombineAllInitTransactionAsync());
            //await Task.Run(() => CombineAllInitTransactionAsync());

            //MiddleWare();
            return await Task.Run(() => CombineAllInitTransactionAsync());

            //BackgroundJob.Enqueue(() => EmailService(Request));

            //return await Task.FromResult(new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess });
            //_Resp = await MethodRespTsk;            
            //return new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess };
            //return _Resp;
        }
        //public async Task<BizResponse> MiddleWare()
        //{
        //   return await Task.Run(() => CombineAllInitTransactionAsync());
        //}

        public async Task<BizResponse> CombineAllInitTransactionAsync()
        {
            _Resp = new BizResponse();
            try
            {
                //Helpers.JsonSerialize(null);
                //=========================PROCESS
                //Check balance here
                var Validation = await ValidateTransaction(_Resp);                

                if (!Validation) //Validation.Result//validation and balance check success
                {
                    HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Validation fail" + _Resp.ReturnMsg + "##TrnNo:" + Req.TrnNo);
                    MarkTransactionSystemFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                    //return Task.FromResult(_Resp);
                    return _Resp;
                }
                var BalResult = _WalletService.WalletBalanceCheck(Req.Amount, Req.DebitAccountID); //DI of Wallet for balance check
                if (!BalResult) //validation and balance check success
                {
                    _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_InsufficientBalanceMsg;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_InsufficientBalance;
                    MarkTransactionSystemFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                    HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Balance check Fail" + _Resp.ReturnMsg + "##TrnNo:" + Req.TrnNo);
                    return _Resp;
                }
                //Deduct balance here
                if (Req.TrnType == enTrnType.Transaction)
                {
                    Req.ServiceType = (enServiceType)_TrnService.ServiceType;
                    //Req.WalletTrnType = enWalletTrnType.;
                }
                else if (Req.TrnType == enTrnType.Withdraw)
                {
                    Req.ServiceType = (enServiceType)_TrnService.ServiceType;
                    Req.WalletTrnType = enWalletTrnType.Dr_Withdrawal;
                }
                else//trading
                {
                    Req.ServiceType = enServiceType.Trading;
                    Req.WalletTrnType = enWalletTrnType.Dr_Sell_Trade;
                }

                var DebitResult = _WalletService.GetWalletDeductionNew(Req.SMSCode, Helpers.GetTimeStamp(), enWalletTranxOrderType.Debit, Req.Amount, Req.MemberID,
                    Req.DebitAccountID, Req.TrnNo, Req.ServiceType, Req.WalletTrnType, Req.TrnType, Req.accessToken);

                if (DebitResult.ReturnCode == enResponseCode.Fail)
                {
                    _Resp.ReturnMsg = DebitResult.ReturnMsg;//EnResponseMessage.ProcessTrn_WalletDebitFailMsg;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ErrorCode = DebitResult.ErrorCode;//enErrorCode.ProcessTrn_WalletDebitFail;
                    MarkTransactionSystemFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                    HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Balance Deduction Fail" + _Resp.ReturnMsg + "##TrnNo:" + Req.TrnNo);
                    return _Resp;
                }
                //===================================Make txn HOLD as balance debited=======================
                MarkTransactionHold(EnResponseMessage.ProcessTrn_HoldMsg, enErrorCode.ProcessTrn_Hold);
                if (Req.TrnType == enTrnType.Transaction || Req.TrnType == enTrnType.Withdraw)
                {
                    HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Transaction/Withdraw Process Start" + "##TrnNo:" + Req.TrnNo);
                    _Resp = await CallWebAPI(_Resp);
                    return _Resp;
                }
                else//Trading process here
                {
                    HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Trading Data Entry Start " + "##TrnNo:" + Req.TrnNo);
                    //Make Trading Data Entry
                    _Resp = await TradingDataInsert(_Resp);
                    HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Trading Data Entry Done " +_Resp.ReturnMsg + "##TrnNo:" + Req.TrnNo);

                    //Start Settlement Here
                    var HoldTrnNos = new List<long> { };                   
                    if (_Resp.ReturnCode==enResponseCodeService.Success)
                    {
                        _Resp = await _SettlementRepository.PROCESSSETLLEMENT(_Resp, TradeBuyRequestObj,ref HoldTrnNos,Req.accessToken);
                        try
                        {//This try catch create wrapper for current transaction
                            BizResponse _Resp1 = new BizResponse();
                            foreach (long HoldTrnNo in HoldTrnNos)
                            {                               
                                var NewBuyRequestObj = _TradeBuyRequest.GetSingle(item => item.TrnNo == HoldTrnNo && item.IsProcessing == 0 && item.IsCancel==0 &&
                                                                                (item.Status == Convert.ToInt16(enTransactionStatus.Hold) ||
                                                                                item.Status == Convert.ToInt16(enTransactionStatus.Pending)));
                                if (NewBuyRequestObj != null)
                                {
                                    var HoldTrnNosNotExec = new List<long> { };
                                    _Resp1 = await _SettlementRepository.PROCESSSETLLEMENT(_Resp1, NewBuyRequestObj, ref HoldTrnNosNotExec);// no access tocken as no login this member in this request
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            HelperForLog.WriteErrorLog("CombineAllInitTransactionAsync:##TrnNo " + Req.TrnNo, ControllerName, ex);
                        }
                    }                   
                   
                    return _Resp;
                }

                //=========================UPDATE
                //return null;
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog("CombineAllInitTransactionAsync:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return Task.FromResult((new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionProcessInternalError }));
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
                HelperForLog.WriteLogIntoFile("CreateTransaction", ControllerName, "Transaction Process For" + Req.TrnType + "##TrnNo:" + Req.TrnNo);
                if (Req.TrnType == enTrnType.Buy_Trade)
                {
                    _TradeTransactionObj.TrnTypeName = "BUY";
                }
                else if (Req.TrnType == enTrnType.Sell_Trade)
                {
                    _TradeTransactionObj.TrnTypeName = "SELL";
                }
                Req.DebitWalletID = _WalletService.GetWalletID(Req.DebitAccountID);
                if(Req.DebitWalletID==0)
                {
                    Req.StatusMsg = EnResponseMessage.InValidDebitAccountIDMsg;
                    return MarkSystemFailTransaction(enErrorCode.InValidDebitAccountID);
                }
                HelperForLog.WriteLogIntoFile("CreateTransaction", ControllerName, "Debit WalletID" + Req.DebitWalletID + "##TrnNo:" + Req.TrnNo);
                //IF @PairID <> 0 ntrivedi 18-04-2018  check inside @TrnType (4,5) @TradeWalletMasterID will be 0 or null
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    if(Req.ordertype==enTransactionMarketType.MARKET)
                    {
                        var pairStastics = _tradePairStastics.GetSingle(pair => pair.PairId == Req.PairID);
                        Req.Price = pairStastics.LTP;
                    }
                    Req.CreditWalletID = _WalletService.GetWalletID(Req.CreditAccountID);
                    if (Req.CreditWalletID == 0)
                    {
                        Req.StatusMsg = EnResponseMessage.InValidCreditAccountIDMsg;
                        return MarkSystemFailTransaction(enErrorCode.InValidCreditAccountID);
                    }
                    HelperForLog.WriteLogIntoFile("CreateTransaction", ControllerName, "Credit WalletID" + Req.CreditWalletID + "##TrnNo:" + Req.TrnNo);
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
                    if (Amount2 < Req.Amount)
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

                if (Req.Amount <= 0) // ntrivedi 02-11-2018 if amount =0 then also invalid
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

                    //Check Whilte listed Address 
                    var WalletWhiteListResult = _WalletService.CheckWithdrawalBene(Req.DebitWalletID, Req.AddressLabel, Req.TransactionAccount, Req.WhitelistingBit);
                    if (!WalletWhiteListResult.Equals(enErrorCode.Success))
                    {
                        Req.StatusMsg = WalletWhiteListResult.ToString();
                        return MarkSystemFailTransaction(WalletWhiteListResult);
                    }

                    //Uday 02-11-2018 Check Wallet Amount Limit
                    var _ServiceLimitCheckObj = _WalletService.GetServiceLimitChargeValue(Req.TrnType, Req.SMSCode);
                    if (Req.Amount > _ServiceLimitCheckObj.MaxAmount || Req.Amount < _ServiceLimitCheckObj.MinAmount)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrn_WithdrawAmountBetweenMinAndMax.Replace("@MIN", _ServiceLimitCheckObj.MinAmount.ToString()).Replace("@MAX", _ServiceLimitCheckObj.MaxAmount.ToString());
                        return MarkSystemFailTransaction(enErrorCode.CreateTrn_NoSelfAddressWithdrawAllow);
                    }

                }
                Req.Status = enTransactionStatus.Initialize;
                Req.StatusCode = Convert.ToInt64(enErrorCode.TransactionInsertSuccess);
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
                HelperForLog.WriteErrorLog("CreateTransaction:##TrnNo " + Req.TrnNo, ControllerName, ex);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }

        }
        public BizResponse MarkSystemFailTransaction(enErrorCode ErrorCode)
        {
            try
            {
                Req.Status = enTransactionStatus.SystemFail;
                Req.StatusCode = Convert.ToInt64(ErrorCode);
                InsertTransactionInQueue();              
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    try//as some para null in starting so error occured here ,only in case of system fail
                    {
                        InsertTradeTransactionInQueue();                        
                    }
                    catch(Exception ex)
                    {
                        HelperForLog.WriteErrorLog("MarkSystemFailTransaction Trade TQ Error:##TrnNo " + Req.TrnNo, ControllerName, ex);
                    }
                    InsertTradeStopLoss();
                }
                //DI of SMS here
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.Fail, ErrorCode = ErrorCode });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("MarkSystemFailTransaction:##TrnNo " + Req.TrnNo, ControllerName, ex);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }
        }
        public BizResponse InsertTransactionInQueue()//ref long TrnNo
        {
            //_Resp = new BizResponse();
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
                    StatusCode = Req.StatusCode,
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
                HelperForLog.WriteErrorLog("InsertTransactionInQueue:##TrnNo " + Req.TrnNo, ControllerName, ex);
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
                NewTradetransaction=_TradeTransactionRepository.Add(NewTradetransaction);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("InsertTradeTransactionInQueue:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public BizResponse InsertTradeStopLoss()
        {
            try
            {
                TradeStopLossObj = new TradeStopLoss()
                {
                    TrnNo = Req.TrnNo,
                    ordertype = Convert.ToInt16(Req.ordertype),
                    StopPrice = Req.StopPrice,
                    Status = Convert.ToInt16(enTransactionStatus.Success)
                };
                TradeStopLossObj =_TradeStopLoss.Add(TradeStopLossObj);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("InsertTradeStopLoss:##TrnNo " + Req.TrnNo, ControllerName, ex);
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
                        _TransactionObj.Order_ServiceID = _BaseCurrService.Id;

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
                        _TransactionObj.Order_ServiceID = _SecondCurrService.Id;

                    }
                    if (_TransactionObj.BidPrice_TQ == 0 || _TransactionObj.BidPrice_BuyReq == 0)
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_InvalidBidPriceValueMsg;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_InvalidBidPriceValue;
                        _Resp.ReturnCode = enResponseCodeService.Fail;
                        return Task.FromResult(false);
                    }
                    //_TransactionObj.Delivery_ServiceID = _ServiceConfi.GetSingle(item => item.SMSCode == _TradeTransactionObj.Delivery_Currency).Id;                   
                    CreatePoolOrder();
                    if (PoolOrderObj == null)
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_PoolOrderCreateFailMsg;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_PoolOrderCreateFail;
                        _Resp.ReturnCode = enResponseCodeService.Fail;
                        return Task.FromResult(false);
                    }
                }
                TxnProviderList = _WebApiRepository.GetProviderDataList(new TransactionApiConfigurationRequest { amount = Req.Amount, SMSCode = Req.SMSCode, APIType = enWebAPIRouteType.TransactionAPI, trnType = Req.TrnType ==enTrnType.Sell_Trade?Convert.ToInt32(enTrnType.Buy_Trade):Convert.ToInt32(Req.TrnType)});
                if (TxnProviderList.Count == 0) //Uday 05-11-2018 check condition for no record
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
                HelperForLog.WriteErrorLog("ValidateTransaction:##TrnNo " + Req.TrnNo, ControllerName, ex);
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

                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    //var TradeTxn = _TradeTransactionRepository.GetById(Req.TrnNo);
                    NewTradetransaction.MakeTransactionSystemFail();
                    NewTradetransaction.SetTransactionStatusMsg(StatusMsg);
                    NewTradetransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                    _TradeTransactionRepository.Update(NewTradetransaction);
                }              
                try
                {
                    _ISignalRService.SendActivityNotification("Transaction Validation Fail TrnNo:" + Req.TrnNo, Req.accessToken);
                }
                catch (Exception ex)
                {
                    HelperForLog.WriteLogIntoFile("ISignalRService Notification Error-System fail", ControllerName, ex.Message + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                }
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog("MarkTransactionSystemFail:##TrnNo " + Req.TrnNo, ControllerName, ex);
               // throw ex;
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
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    NewTradetransaction.MakeTransactionHold();
                    NewTradetransaction.SetTransactionStatusMsg(StatusMsg);
                    NewTradetransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                    _TradeTransactionRepository.Update(NewTradetransaction);
                }
                
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("MarkTransactionHold:##TrnNo " + Req.TrnNo, ControllerName, ex);
                throw ex;
            }
        }
        public void MarkTransactionOperatorFail(string StatusMsg, enErrorCode ErrorCode)
        {
            //CreditWalletDrArryTrnID[] CreditWalletDrArryTrnIDObj = new CreditWalletDrArryTrnID[1];
            List<CreditWalletDrArryTrnID> CreditWalletDrArryTrnIDList = new List<CreditWalletDrArryTrnID>();
            try
            {
                //var Txn = _TransactionRepository.GetById(Req.TrnNo);
                Newtransaction.MakeTransactionOperatorFail();
                Newtransaction.SetTransactionStatusMsg(StatusMsg);
                Newtransaction.SetTransactionCode(Convert.ToInt64(ErrorCode));
                _TransactionRepository.Update(Newtransaction);

                //Cr Amount to member back
                //CreditWalletDrArryTrnIDObj[0].DrTrnRefNo = Req.TrnNo;
                //CreditWalletDrArryTrnIDObj[0].Amount = Req.Amount;
                CreditWalletDrArryTrnIDList.Add(new CreditWalletDrArryTrnID { DrTrnRefNo = Req.TrnNo, Amount = Req.Amount });


                _WalletService.GetWalletCreditNew(Req.SMSCode, Helpers.GetTimeStamp(), enWalletTrnType.Cr_Refund, Req.Amount, Req.MemberID,
                Req.DebitAccountID, CreditWalletDrArryTrnIDList.ToArray(), Req.TrnNo,1, enWalletTranxOrderType.Credit, Req.ServiceType, (enTrnType)Newtransaction.TrnType);

                try
                {
                    _ISignalRService.SendActivityNotification("Transaction Failed TrnNo:" + Req.TrnNo, Req.accessToken);
                }
                catch (Exception ex)
                {
                    HelperForLog.WriteLogIntoFile("ISignalRService Notification Error--Operator fail", ControllerName, ex.Message + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("MarkTransactionOperatorFail:##TrnNo " + Req.TrnNo, ControllerName, ex);
                throw ex;
            }
        }
        public void InsertIntoWithdrawHistory(long SerProID,string RouteTag,string TrnID)
        {
            try
            {
                WithdrawHistory WH = new WithdrawHistory();
                WH.SMSCode = Req.SMSCode;
                WH.TrnID = TrnID;
                WH.Wallet = Req.DebitWalletID;
                WH.Address = Req.TransactionAccount;
                WH.ToAddress = "";
                WH.Confirmations = 0;
                WH.Value = 0;
                WH.Amount = Req.Amount;
                WH.Charge = 0;
                WH.Status = 6; // ntrivedi status=6 to pick from deposition tps
                WH.confirmedTime = "";
                WH.unconfirmedTime = "";
                WH.CreatedDate = Helpers.UTC_To_IST();
                WH.State = 0;
                WH.IsProcessing = 0;
                WH.TrnNo = Req.TrnNo;
                WH.RouteTag = RouteTag;
                WH.UserId = Req.MemberID;
                WH.SerProID = SerProID;
                WH.TrnDate = Helpers.UTC_To_IST();
                WH.APITopUpRefNo = "";
                WH.createdTime = Helpers.UTC_To_IST().ToString();
                WH.SystemRemarks = "Initial entry";
                _WalletService.InsertIntoWithdrawHistory(WH);
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog("InsertIntoWithdrawHistory Error-Fail:##TrnNo " + Req.TrnNo, ControllerName, ex);
            }
        }
        public Task<BizResponse> CallWebAPI(BizResponse _Resp)
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

                    Newtransaction.SetServiceProviderData(Provider.ServiceID, Provider.ServiceProID, Provider.ProductID, Provider.RouteID);
                    _TransactionRepository.Update(Newtransaction);

                    WebApiConfigurationResponseObj =_IWebApiData.GetAPIConfiguration(Provider.ThirPartyAPIID);
                    if(WebApiConfigurationResponseObj==null)
                    {
                        _Resp.ReturnMsg = EnResponseMessage.ProcessTrn_ThirdPartyDataNotFoundMsg;
                        _Resp.ReturnCode = enResponseCodeService.Fail;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_ThirdPartyDataNotFound;
                        MarkTransactionOperatorFail(_Resp.ReturnMsg, _Resp.ErrorCode);
                        continue;
                    }
                    ThirdPartyAPIRequestOnj =_IGetWebRequest.MakeWebRequest(Provider.RouteID,Provider.ThirPartyAPIID,Provider.SerProDetailID, Newtransaction);
                    
                    //Insert API request Data
                    _TransactionObj.TransactionRequestID = InsertTransactionRequest(Provider, ThirdPartyAPIRequestOnj.RequestURL + "::" + ThirdPartyAPIRequestOnj.RequestBody);                   

                    switch (Provider.AppTypeID)
                    {
                        case (long)enAppType.WebSocket:

                        case (long)enAppType.JsonRPC:
                            _TransactionObj.APIResponse = _IWebApiSendRequest.SendJsonRpcAPIRequestAsync(ThirdPartyAPIRequestOnj.RequestURL, ThirdPartyAPIRequestOnj.RequestBody, ThirdPartyAPIRequestOnj.keyValuePairsHeader);
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
                    //_TransactionObj.APIResponse = "{\"txid\":\"dc0f4454e77cb3bdaa0c707b37eca250dc6ac84a40994cc99c9c063583f50bbc\",\"tx\":\"01000000000101bbdb21bfc18efc9357c2abcbed6e6a6bc47bab2dd78952aa3b1b0aaa48af972c0100000023220020a3dd471b5765c0ac7e7173c00cb8f6d8ede04809442441a78ae71e555f0e3e25ffffffff02ce2a81000000000017a914ed1f54cf86f6b69e860bd118d29b63f6dbb2e17c87988d07000000000017a9141f6b16323e351422509d8f70f2ebe8f77aabc62d87040047304402205871d8dea5a91b9d40b6b8489dfd94184b2cb6338745b95cf71f45c6e23bc12a02202c4d99a057b85a6c0e8fed053278c9e4a1d0faf95931dc7925a52723e57e0f3d01483045022100f52edab9924eb8cc9785f1f27ebf8d53a86c81232ef8bafe7c66615e593283ac02206052b2e7c76f1390a57def1dbca28480ba70b506d20b4245204c8803b2bb85260169522103ca80fd2ca4c7097f386853f52f9224128b15409340524d04bddafc5afdb6b47321026563949e58c2b622d9ad84c53dbd5d1bdc09858ec37d96fbd95af4b49f06a8342102b5208a7ce843e28e2460eff28cd6d770c948a516074f1f0627f3cfb020512c0153aebc310800\",\"status\":\"signed\"}";
                    WebAPIParseResponseClsObj = _WebApiParseResponseObj.TransactionParseResponse(_TransactionObj.APIResponse, Provider.ThirPartyAPIID);
                    NewtransactionReq.SetTrnID(WebAPIParseResponseClsObj.TrnRefNo);
                    NewtransactionReq.SetOprTrnID(WebAPIParseResponseClsObj.OperatorRefNo);
                    _TransactionRequest.Update(NewtransactionReq);

                    if ((WebAPIParseResponseClsObj.Status == enTransactionStatus.Success) || (WebAPIParseResponseClsObj.Status == enTransactionStatus.Hold))
                        if (Req.TrnType == enTrnType.Withdraw)
                            InsertIntoWithdrawHistory(Provider.ServiceProID, Provider.RouteName, WebAPIParseResponseClsObj.TrnRefNo);

                    if (WebAPIParseResponseClsObj.Status == enTransactionStatus.Success)
                    {
                        Newtransaction.MakeTransactionSuccess();
                        Newtransaction.SetTransactionStatusMsg("Success" + WebAPIParseResponseClsObj.StatusMsg);                        
                        _TransactionRepository.Update(Newtransaction);
                        IsTxnProceed = 1;//no further call next API
                        try
                        {
                            _ISignalRService.SendActivityNotification("Transaction Success TrnNo:"+ Req.TrnNo, Req.accessToken);
                            EmailSendAsync(TradeBuyRequestObj.UserID.ToString(), Convert.ToInt16(enTransactionStatus.Success), Req.SMSCode,Req.SMSCode, Newtransaction.TrnDate.ToString(),0, Req.Amount, 0);
                        }
                        catch (Exception ex)
                        {
                            HelperForLog.WriteLogIntoFile("ISignalRService Notification Error-Success", ControllerName, ex.Message + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                        }
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

                        _Resp.ReturnMsg = "Hold";
                        _Resp.ReturnCode = enResponseCodeService.Success;
                        _Resp.ErrorCode = enErrorCode.ProcessTrn_Hold;
                        IsTxnProceed = 1;//no further call next API
                        break;
                    }
                }
                if(IsTxnProceed==0)
                {
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_OprFail;
                    MarkTransactionOperatorFail("Fail" +WebAPIParseResponseClsObj.StatusMsg, _Resp.ErrorCode);
                }
              

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("CallWebAPI:##TrnNo " + Req.TrnNo, ControllerName, ex);
                if (IsTxnProceed == 0)
                {
                    _Resp.ReturnMsg = "Error occured";
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_APICallInternalError;
                }
                else
                {
                    _Resp.ReturnMsg = "Hold";
                    _Resp.ReturnCode = enResponseCodeService.Success;
                    _Resp.ErrorCode = enErrorCode.ProcessTrn_Hold;
                }
              
            }
            return Task.FromResult(_Resp);
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
                NewtransactionReq =_TransactionRequest.Add(NewtransactionReq);
                return NewtransactionReq.Id;

            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog("InsertTransactionRequest:##TrnNo " + Req.TrnNo, ControllerName, ex);
                return 0;
            }
        }
        #endregion

        #region Settlement Insert Data
        public void CreatePoolOrder()
        {
            try
            {
                PoolOrderObj = new PoolOrder()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    CreatedBy = Req.MemberID,                   
                    UserID = Req.MemberID,
                    DMemberID = Req.MemberID, //Member/User gives Amount to Pool
                    TrnNo = Req.TrnNo,
                    TrnMode = Req.TrnMode,
                    PayMode = Convert.ToInt16(enWebAPIRouteType.TradeServiceLocal),
                    ORemarks = "Order Created",
                    OrderAmt = Req.Amount,
                    DiscPer = 0,
                    DiscRs = 0,                   
                    Status = Convert.ToInt16(enTransactionStatus.Initialize),//txn type status
                    UserWalletID = Req.DebitWalletID,
                    UserWalletAccID = Req.DebitAccountID,                  
                };
                PoolOrderObj = _PoolOrder.Add(PoolOrderObj);              

                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("CreatePoolOrder:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public void Tradepoolmaster()
        {
            try
            {
                TradePoolMasterObj = new TradePoolMaster()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    CreatedBy = Req.MemberID,
                    PairName = _TradeTransactionObj.PairName,
                    ProductID = Convert.ToInt16(enWebAPIRouteType.TradeServiceLocal),
                    SellServiceID = _TransactionObj.Order_ServiceID,
                    BuyServiceID = _TransactionObj.Delivery_ServiceID,
                    BidPrice = _TransactionObj.BidPrice_TQ,
                    CountPerPrice = 0,
                    Status = Convert.ToInt16(ServiceStatus.Active),//Record Type Status
                    TotalQty = Req.Amount,
                    OnProcessing = 0,                   
                    TPSPickupStatus = 0,
                    IsSleepMode = 0,
                    GUID = Guid.NewGuid(),
                    PairId = Req.PairID,
                };
                TradePoolMasterObj = _TradePoolMaster.Add(TradePoolMasterObj);                
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("Tradepoolmaster:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public void InsertSellerList()
        {
            try
            {
                TradeSellerListObj = new TradeSellerList()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    CreatedBy = Req.MemberID,
                    TrnNo = Req.TrnNo,
                    PoolID = TradePoolMasterObj.Id,
                    SellServiceID = TradePoolMasterObj.SellServiceID,
                    BuyServiceID = TradePoolMasterObj.BuyServiceID,
                    Price = TradePoolMasterObj.BidPrice,
                    Qty = Req.Amount,//take trnno amount as pool as total amount
                    RemainQty = Req.Amount,
                    Status = Convert.ToInt16(enTransactionStatus.Initialize),//txn type status
                };
                TradeSellerListObj = _TradeSellerList.Add(TradeSellerListObj);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("Tradepoolmaster:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
       
        public void TradeBuyRequest()
        {
            try
            {
                TradeBuyRequestObj = new TradeBuyRequest()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    PickupDate = Helpers.UTC_To_IST(),
                    CreatedBy = Req.MemberID,
                    UserID = Req.MemberID,
                    TrnNo = Req.TrnNo,
                    PairID = Req.PairID,                   
                    ServiceID = _TransactionObj.Delivery_ServiceID,
                    PaidServiceID = _TransactionObj.Order_ServiceID,
                    BidPrice = _TransactionObj.BidPrice_BuyReq,
                    Qty = _TradeTransactionObj.DeliveryTotalQty,                   
                    PaidQty = _TradeTransactionObj.OrderTotalQty,
                    PendingQty = _TradeTransactionObj.DeliveryTotalQty,
                    DeliveredQty = 0,
                    IsCancel = 0,
                    IsPartialProceed = 0,
                    IsProcessing=0,
                    SellStockID = TradePoolMasterObj.Id,
                    BuyStockID = 0,//No use as process with multiple stock
                    Status = Convert.ToInt16(enTransactionStatus.Initialize),
                };
                TradeBuyRequestObj = _TradeBuyRequest.Add(TradeBuyRequestObj);

                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("Tradepoolmaster:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public void InsertBuyerList()
        {
            try
            {
               var TradeBuyerListObj = new TradeBuyerList()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    CreatedBy = Req.MemberID,
                    TrnNo = Req.TrnNo,
                    BuyReqID = TradeBuyRequestObj.Id,
                    ServiceID = TradeBuyRequestObj.ServiceID,
                    PaidServiceID = TradeBuyRequestObj.PaidServiceID,
                    Price = TradeBuyRequestObj.BidPrice,
                    Qty = TradeBuyRequestObj.Qty, //same as request as one entry per one request
                    IsProcessing = 0,
                    Status = Convert.ToInt16(enTransactionStatus.Initialize),//txn type status
                };
                TradeBuyerListObj = _TradeBuyerList.Add(TradeBuyerListObj);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("Tradepoolmaster:##TrnNo " + Req.TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        //public void InsertTradePoolQueue(long MakerTrnNo,long PoolID,decimal MakerQty, decimal MakerPrice, long TakerTrnNo, decimal TakerQty, decimal TakerPrice, decimal TakerDisc,decimal TakerLoss)
        //{
        //    try
        //    {
        //        TradePoolQueueObj = new TradePoolQueue()
        //        {
        //            CreatedDate = Helpers.UTC_To_IST(),
        //            CreatedBy = Req.MemberID,
        //            MakerTrnNo = MakerTrnNo,
        //            PoolID = PoolID,
        //            MakerQty = MakerQty,
        //            MakerPrice = MakerPrice,
        //            TakerTrnNo = TakerTrnNo,
        //            TakerQty = TakerQty,
        //            TakerPrice = TakerPrice,
        //            TakerDisc = TakerDisc,
        //            TakerLoss= TakerLoss,
        //            Status = Convert.ToInt16(enTransactionStatus.Success),//always etry after settlement done
        //        };
        //        TradePoolQueueObj = _TradePoolQueue.Add(TradePoolQueueObj);
        //        //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog("InsertTradePoolQueue:##TrnNo " + Req.TrnNo, ControllerName, ex);
        //        //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
        //        throw ex;
        //    }

        //}
        //public void CreatePoolOrderForSettlement(long OMemberID,long DMemberID,long UserID,long PoolID,long TrnNo,decimal Amount)
        //{
        //    try
        //    {
        //        PoolOrderObj = new PoolOrder()
        //        {
        //            CreatedDate = Helpers.UTC_To_IST(),
        //            CreatedBy = UserID,
        //            UserID = UserID,
        //            DMemberID = DMemberID, //Pool gives Amount to Member/User
        //            OMemberID = OMemberID, //Member/User Take Amount from Pool
        //            TrnNo = TrnNo,
        //            TrnMode = Req.TrnMode,
        //            PayMode = Convert.ToInt16(enWebAPIRouteType.TradeServiceLocal),
        //            ORemarks = "Order Created",
        //            OrderAmt = Amount,
        //            DiscPer = 0,
        //            DiscRs = 0,
        //            Status = Convert.ToInt16(enTransactionStatus.Initialize),//txn type status
        //            UserWalletID = Req.CreditWalletID,
        //            UserWalletAccID = Req.CreditAccountID,
        //        };
        //        PoolOrderObj = _PoolOrder.Add(PoolOrderObj);

        //        //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog("CreatePoolOrder:##TrnNo " + Req.TrnNo, ControllerName, ex);
        //        //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
        //        throw ex;
        //    }

        //}
        public Task<BizResponse> TradingDataInsert(BizResponse _Resp)
        {
            try
            {
                _Resp.ReturnCode = enResponseCodeService.Fail;//temp init
                foreach (TransactionProviderResponse Provider in TxnProviderList)//Make txn on every API
                {
                    //Update Service Provider Data
                    Newtransaction.SetServiceProviderData(Provider.ServiceID, Provider.ServiceProID, Provider.ProductID, Provider.RouteID);

                    //===========POOL==================
                    TradePoolMasterObj = _TradePoolMaster.GetSingle(item => item.BidPrice == _TransactionObj.BidPrice_TQ && 
                                                        item.SellServiceID == _TransactionObj.Order_ServiceID && 
                                                        item.BuyServiceID == _TransactionObj.Delivery_ServiceID &&
                                                        item.Status == Convert.ToInt16(ServiceStatus.Active));
                    if (TradePoolMasterObj != null)//Update
                    {
                        TradePoolMasterObj.TotalQty += Req.Amount;
                        TradePoolMasterObj.UpdatedBy = Req.MemberID;
                        TradePoolMasterObj.UpdatedDate = Helpers.UTC_To_IST();
                        _TradePoolMaster.Update(TradePoolMasterObj);
                        _Resp.ReturnMsg = "PoolMaster Record updated";
                    }
                    else//Make new Pool Entry
                    {
                        Tradepoolmaster();
                        _Resp.ReturnMsg = "PoolMaster Record Inserted";
                    }
                    //Insert into seller list
                    InsertSellerList();
                    _Resp.ReturnMsg = "Seller Entry Inserted";

                    PoolOrderObj.PoolID = TradePoolMasterObj.Id;
                    PoolOrderObj.OMemberID = TradePoolMasterObj.Id;//Pool takes amount from member
                    PoolOrderObj.DeliveryAmt = Req.Amount;
                    PoolOrderObj.DRemarks = "Delivery Success with " + _TransactionObj.BidPrice_TQ;
                    PoolOrderObj.Status = Convert.ToInt16(enTransactionStatus.Success);                    
                    _PoolOrder.Update(PoolOrderObj);
                    _Resp.ReturnMsg = "Pool Order Updated Inserted";

                    
                    //=======================Buy Request
                    TradeBuyRequest();
                    InsertBuyerList();

                    _Resp.ReturnCode = enResponseCodeService.Success;
                    _Resp.ReturnMsg = "Pool Order Updated Inserted";
                    try
                    {
                        var CopyNewtransaction = new TransactionQueue();
                        CopyNewtransaction = (TransactionQueue)Newtransaction.Clone();

                        var CopyNewTradetransaction = new TradeTransactionQueue();                      
                        CopyNewTradetransaction = (TradeTransactionQueue)NewTradetransaction.Clone();

                        _ISignalRService.OnStatusHold(Convert.ToInt16(enTransactionStatus.Success), CopyNewtransaction, CopyNewTradetransaction, Req.accessToken, TradeStopLossObj.ordertype);                        
                    }
                    catch (Exception ex)
                    {
                        HelperForLog.WriteLogIntoFile("ISignalRService", ControllerName, "Trading Hold Error " + ex.Message + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                    }
                    break;
                }              
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("TradingDataInsert:##TrnNo " + Req.TrnNo, ControllerName, ex);
                _Resp.ReturnCode = enResponseCodeService.Fail;
                _Resp.ReturnMsg = ex.Message;
                //throw ex;
            }
            return Task.FromResult(_Resp);
        }
        #endregion

        public async Task EmailSendAsync(string UserID, int Status, string PairName, string BaseMarket
           , string TrnDate, decimal ReqAmount = 0, decimal Amount = 0, decimal Fees = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserID) && !string.IsNullOrEmpty(PairName) && !string.IsNullOrEmpty(BaseMarket) &&
                    ReqAmount != 0 && !string.IsNullOrEmpty(TrnDate) && Amount != 0 && Status == 1)
                {
                    SendEmailRequest Request = new SendEmailRequest();
                    ApplicationUser User = new ApplicationUser();
                    User = await _userManager.FindByIdAsync(UserID);
                    if (!string.IsNullOrEmpty(User.Email))
                    {
                        IQueryable Result = await _messageConfiguration.GetTemplateConfigurationAsync(Convert.ToInt16(enCommunicationServiceType.Email), Convert.ToInt16(EnTemplateType.TransactionSuccess), 0);
                        foreach (TemplateMasterData Provider in Result)
                        {
                            Provider.Content = Provider.Content.Replace("###USERNAME###", User.Name);
                            Provider.Content = Provider.Content.Replace("###TYPE###", PairName);
                            Provider.Content = Provider.Content.Replace("###REQAMOUNT###", ReqAmount.ToString());
                            Provider.Content = Provider.Content.Replace("###STATUS###", "Success");
                            Provider.Content = Provider.Content.Replace("###USER###", User.Name);
                            Provider.Content = Provider.Content.Replace("###CURRENCY###", BaseMarket);
                            Provider.Content = Provider.Content.Replace("###DATETIME###", TrnDate);
                            Provider.Content = Provider.Content.Replace("###AMOUNT###", Amount.ToString());
                            Provider.Content = Provider.Content.Replace("###FEES###", Fees.ToString());
                            Provider.Content = Provider.Content.Replace("###FINAL###", (Amount + Fees).ToString());
                            Request.Body = Provider.Content;
                            Request.Subject = Provider.AdditionalInfo;
                        }
                        Request.Recepient = User.Email;
                        Request.EmailType = 0;
                        await _mediator.Send(Request);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("Settlement - EmailSendAsync Error ", ControllerName, ex);
            }
        }


        //#region ==============================PROCESS SETLLEMENT========================
        //public Task<BizResponse> PROCESSSETLLEMENT(BizResponse _Resp)
        //{
        //    try
        //    {
        //        TradeBuyRequestObj.Status = Convert.ToInt16(enTransactionStatus.Hold);
        //        TradeBuyRequestObj.UpdatedDate = Helpers.UTC_To_IST();
        //        TradeBuyRequestObj.IsProcessing = 1;
        //        _TradeBuyRequest.Update(TradeBuyRequestObj);

        //        TradeBuyerListObj.Status = Convert.ToInt16(enTransactionStatus.Hold);
        //        TradeBuyerListObj.UpdatedDate = Helpers.UTC_To_IST();
        //        TradeBuyerListObj.IsProcessing = 1;
        //        _TradeBuyerList.Update(TradeBuyerListObj);

        //        //SortedList<TradeSellerList, TradeSellerList>
        //        var MatchSellerListBase = _TradeSellerList.FindBy(item => item.Price <= TradeBuyRequestObj.BidPrice && item.IsProcessing == 0 
        //                                                && item.BuyServiceID== TradeBuyRequestObj.PaidServiceID &&
        //                                                item.SellServiceID== TradeBuyRequestObj.ServiceID 
        //                                                && (item.Status== Convert.ToInt16(enTransactionStatus.Initialize) || item.Status == Convert.ToInt16(enTransactionStatus.Pending))
        //                                                && item.RemainQty>0);//Pending after partial Qty remain

        //        var MatchSellerList = MatchSellerListBase.OrderBy(x => x.TrnNo).OrderBy(x => x.Price);

        //        foreach (TradeSellerList SellerList in MatchSellerList)
        //        {
        //            if (SellerList.IsProcessing == 1)
        //                continue;

        //            SellerList.IsProcessing = 1;
        //            _TradeSellerList.Update(SellerList);
        //            var PoolMst = _TradePoolMaster.GetById(SellerList.PoolID);

        //            if (SellerList.RemainQty <= TradeBuyRequestObj.PendingQty)
        //            {
        //                CreatePoolOrderForSettlement(TradeBuyRequestObj.UserID, SellerList.PoolID, TradeBuyRequestObj.UserID, SellerList.PoolID, TradeBuyRequestObj.TrnNo, SellerList.RemainQty);
        //                TradeBuyRequestObj.PendingQty = TradeBuyRequestObj.PendingQty - SellerList.RemainQty;
        //                TradeBuyRequestObj.DeliveredQty = TradeBuyRequestObj.DeliveredQty + SellerList.RemainQty;

        //                //Here Bid Price of pool always low then user given in Order , base on above Query
        //                decimal TakeDisc = 0;
        //                if (SellerList.Price < TradeBuyRequestObj.BidPrice)
        //                {
        //                    TakeDisc = (TradeBuyRequestObj.BidPrice - SellerList.Price) * SellerList.RemainQty;
        //                }
        //                InsertTradePoolQueue(SellerList.TrnNo, SellerList.PoolID, SellerList.RemainQty, SellerList.Price, TradeBuyRequestObj.TrnNo, SellerList.RemainQty, TradeBuyRequestObj.BidPrice, TakeDisc, 0);

        //                SellerList.RemainQty = SellerList.RemainQty - SellerList.RemainQty;//take all
        //                SellerList.Status = Convert.ToInt16(enTransactionStatus.Success);
        //                SellerList.IsProcessing = 0;//release as fully Empty
        //                PoolMst.TotalQty = PoolMst.TotalQty - SellerList.RemainQty;

        //                _TradeBuyRequest.Update(TradeBuyRequestObj);
        //                _TradeSellerList.Update(SellerList);
        //                _TradePoolMaster.Update(PoolMst);
        //                //Continuew as record Partially settled
        //            }
        //            else if (SellerList.RemainQty > TradeBuyRequestObj.PendingQty)//FULL SETTLEMENT TO MEMBER
        //            {
        //                SellerList.RemainQty = SellerList.RemainQty - TradeBuyRequestObj.PendingQty;//Update first as updated value in below line
        //                SellerList.Status = Convert.ToInt16(enTransactionStatus.Hold);
        //                SellerList.IsProcessing = 0;
        //                PoolMst.TotalQty = PoolMst.TotalQty - TradeBuyRequestObj.PendingQty;

        //                //Here Bid Price of pool always low then user given in Order , base on above Query
        //                decimal TakeDisc = 0;
        //                if (SellerList.Price < TradeBuyRequestObj.BidPrice)
        //                {
        //                    TakeDisc = (TradeBuyRequestObj.BidPrice - SellerList.Price) * TradeBuyRequestObj.PendingQty;
        //                }
        //                InsertTradePoolQueue(SellerList.TrnNo, SellerList.PoolID, SellerList.RemainQty, SellerList.Price, TradeBuyRequestObj.TrnNo, TradeBuyRequestObj.PendingQty, TradeBuyRequestObj.BidPrice, TakeDisc, 0);

        //                TradeBuyRequestObj.DeliveredQty = TradeBuyRequestObj.DeliveredQty + TradeBuyRequestObj.PendingQty;
        //                TradeBuyRequestObj.PendingQty = TradeBuyRequestObj.PendingQty - TradeBuyRequestObj.PendingQty;//take all
        //                TradeBuyRequestObj.IsProcessing = 0;//release as fully settled
        //                _TradeBuyRequest.Update(TradeBuyRequestObj);
        //                _TradeSellerList.Update(SellerList);
        //                _TradePoolMaster.Update(PoolMst);
        //                break;//record settled
        //            }

        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog("PROCESSSETLLEMENT:##TrnNo " + Req.TrnNo, ControllerName, ex);
        //        _Resp.ReturnCode = enResponseCodeService.Fail;
        //        _Resp.ReturnMsg = ex.Message;
        //    }
        //    return Task.FromResult(_Resp);
        //}
        //#endregion

    }
}
