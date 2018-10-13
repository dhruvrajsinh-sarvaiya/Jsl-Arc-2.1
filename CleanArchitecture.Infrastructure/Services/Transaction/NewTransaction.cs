using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    //[Guid("9245fe4a-d402-451c-b9ed-9c1a04247482")]
    public class NewTransaction : ITransactionProcess
    {
        private readonly ICommonRepository<TradePairMaster> _TradePairMaster;
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly EFCommonRepository<TradeStopLoss> _TradeStopLoss;
        private readonly ICommonRepository<ServiceMaster> _ServiceConfi;
        private readonly ICommonRepository<AddressMaster> _AddressMasterRepository;
       public BizResponse _Resp;
        private readonly ILogger<NewTransaction> _log;
       public BizResponse _CreateTransactionResp;
       public TradePairMaster _TradePairObj;


        public NewTransaction(ILogger<NewTransaction> log, ICommonRepository<TradePairMaster> TradePairMaster,
            EFCommonRepository<TransactionQueue> TransactionRepository,
            EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository,
            ICommonRepository<ServiceMaster> ServiceConfi, ICommonRepository<AddressMaster> AddressMasterRepository,
            EFCommonRepository<TradeStopLoss> tradeStopLoss)
        {
            _log = log;
            _TradePairMaster = TradePairMaster;
            _TransactionRepository = TransactionRepository;
            _TradeTransactionRepository = TradeTransactionRepository;
            _ServiceConfi = ServiceConfi;
            _AddressMasterRepository = AddressMasterRepository;
            _TradeStopLoss = tradeStopLoss;
        }
        public async Task<BizResponse> ProcessNewTransactionAsync(NewTransactionRequestCls Req)
        {
            _Resp = new BizResponse();
           //Task<BizResponse> MethodRespTsk = CombineAllInitTransactionAsync(Req);

            _Resp = CreateTransaction(Req);
            if (_Resp.ReturnCode != enResponseCodeService.Success)
            {
                return _Resp;
            }
            var myResp = new Task(async () => CombineAllInitTransactionAsync(Req));

            return await Task.FromResult(new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess });
            //_Resp = await MethodRespTsk;
            // return await Task.FromResult(new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess });
            //return Task.FromResult(new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success, ErrorCode = enErrorCode.TransactionProcessSuccess });
            //return _Resp;
        }

        public async Task<BizResponse> CombineAllInitTransactionAsync(NewTransactionRequestCls Req)
        {
           // Thread.Sleep(10000);
            _Resp = new BizResponse();            
            //if (Req.ordertype== enTransactionMarketType.LIMIT)
            //    return null;
            //=========================INSERT
            //Take memberMobile for sms
            //_Resp = CreateTransaction(Req);
            //if (_Resp.ReturnCode != enResponseCodeService.Success)
            //{
            //    return _Resp;
            //}
            //=========================PROCESS
            //Check balance here
            var Validation=ValidateTransaction(Req);
          
            if(await Validation) //validation and balance check success
            {
    
            }
            //=========================UPDATE
            return null;
        }

        #region RegionInitTransaction    
        public BizResponse CreateTransaction(NewTransactionRequestCls Req)
        {
            //long DeliveryWalletID = 0;           
            //string INRSMSCode = "INR";
            decimal Amount2 = 0;
            //string MemberMobile1 = "";            
            NewTradeTransactionRequestCls _TradeTransactionObj = new NewTradeTransactionRequestCls();
            long TrnNo = 0;
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
                //IF @PairID <> 0 ntrivedi 18-04-2018  check inside @TrnType (4,5) @TradeWalletMasterID will be 0 or null
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {

                    _TradePairObj = new TradePairMaster();
                    _TradePairObj = _TradePairMaster.GetSingle(item => item.Id == Req.PairID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                    _TradeTransactionObj.PairName = _TradePairObj.PairName;
                    if (_TradePairObj.WalletMasterID == 0)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnNoPairSelectedMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrn_NoPairSelected);
                    }
                    if (Req.Qty <= 0 || Req.Price <= 0)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidQtyPriceMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrnInvalidQtyPrice);
                    }
                    if (Req.TrnType == enTrnType.Buy_Trade)
                    {
                        _TradeTransactionObj.BuyQty = Req.Qty;
                        _TradeTransactionObj.BidPrice = Req.Price;
                        _TradeTransactionObj.DeliveryTotalQty = Req.Qty;
                        _TradeTransactionObj.OrderTotalQty = Helpers.DoRoundForTrading(Req.Qty * Req.Price, 8);//235.415001286,8 =  235.41500129                         
                        _TradeTransactionObj.Order_Currency = _ServiceConfi.GetSingle(item => item.Id == _TradePairObj.BaseCurrencyId).SMSCode;
                        _TradeTransactionObj.Delivery_Currency = _ServiceConfi.GetSingle(item => item.Id == _TradePairObj.SecondaryCurrencyId).SMSCode;
                    }
                    if (Req.TrnType == enTrnType.Sell_Trade)
                    {
                        _TradeTransactionObj.SellQty = Req.Qty;
                        _TradeTransactionObj.AskPrice = Req.Price;
                        _TradeTransactionObj.OrderTotalQty = Req.Qty;
                        _TradeTransactionObj.DeliveryTotalQty = Helpers.DoRoundForTrading(Req.Qty * Req.Price, 8);//235.415001286,8 =  235.41500129                        
                        _TradeTransactionObj.Order_Currency = _ServiceConfi.GetSingle(item => item.Id == _TradePairObj.SecondaryCurrencyId).SMSCode;
                        _TradeTransactionObj.Delivery_Currency = _ServiceConfi.GetSingle(item => item.Id == _TradePairObj.BaseCurrencyId).SMSCode;
                    }
                    if (_TradeTransactionObj.OrderTotalQty < (decimal)(0.00000001) || _TradeTransactionObj.DeliveryTotalQty < (decimal)(0.00000001))
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidQtyNAmountMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrnInvalidQtyNAmount);
                    }
                    //Req.SMSCode = _TradeTransactionObj.Order_Currency;
                    //Balace check Here , take DeliveryWalletID output
                    _TradeTransactionObj.DeliveryWalletID = Req.CreditWalletID;

                    Req.Amount = _TradeTransactionObj.OrderTotalQty;
                    if (_TradeTransactionObj.DeliveryWalletID == 0)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrn_NoCreditAccountFoundMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrn_NoCreditAccountFound);
                    }
                }
                if (Req.TrnType == enTrnType.Shoping_Cart)//IF TRNTYPE = 7 THEN FETCH BALANCE OF INR WALLET 
                {
                    Amount2 = Convert.ToDecimal(Req.AdditionalInfo);
                    if(Amount2 < Req.Amount)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidAmountMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrnInvalidAmount);
                    }
                    //Take balance base on @INRSMSCode and take OrderWalletID,Balance
                }
                else
                {
                    //Take balance base on @SMSCode and take OrderWalletID,Balance
                }
                //if(_TradeTransactionObj.OrderWalletID==0)
                //{
                //    Req.StatusMsg = EnResponseMessage.CreateTrn_NoDebitAccountFoundMsg;
                //    return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrn_NoDebitAccountFound);
                //}
                var DuplicateTxn=_TransactionRepository.GetSingle(item => item.MemberMobile == Req.MemberMobile && item.TransactionAccount == Req.TransactionAccount && item.Amount==Req.Amount && item.TrnDate.AddMinutes(10) > Helpers.UTC_To_IST());
                if(DuplicateTxn != null)
                {
                    Req.StatusMsg = EnResponseMessage.CreateTrnDuplicateTrnMsg;
                    return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrnDuplicateTrn);
                }
                if(Req.Amount<0)
                {
                    Req.StatusMsg = EnResponseMessage.CreateTrnInvalidAmountMsg;
                    return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrnInvalidAmount);
                }
                if (Req.TrnRefNo != "0" || !string.IsNullOrEmpty(Req.TrnRefNo))
                {
                    var DuplicateTxnWithRefNo = _TransactionRepository.GetSingle(item => item.MemberID == Req.MemberID && item.TrnRefNo == Req.TrnRefNo && item.TrnType == Convert.ToInt16(enTrnType.Transaction));
                    if (DuplicateTxnWithRefNo != null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrnDuplicateTrnMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrnDuplicateTrn);
                    }
                }
                if (Req.TrnType == enTrnType.Withdraw)
                {
                    var _AddressMasterObj = _AddressMasterRepository.GetSingle(item => item.WalletId == Req.DebitWalletID && item.Address == Req.TransactionAccount && item.Status == Convert.ToInt16(ServiceStatus.Active));//in withdraw , TransactionAccount has address
                    if (_AddressMasterObj != null)
                    {
                        Req.StatusMsg = EnResponseMessage.CreateTrn_NoSelfAddressWithdrawAllowMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj, enErrorCode.CreateTrn_NoSelfAddressWithdrawAllow);
                    }
                }
                Req.Status = enTransactionStatus.Initialize;
                InsertTransactionInQueue(Req, ref TrnNo);
                _TradeTransactionObj.TrnNo = TrnNo;
                Req.TrnNo = TrnNo;
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    InsertTradeTransactionInQueue(Req, _TradeTransactionObj);
                    InsertTradeStopLoss(Req);
                }
                   
                return (new BizResponse { ReturnMsg = "", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }

        }
        public BizResponse MarkSystemFailTransaction(NewTransactionRequestCls Req, NewTradeTransactionRequestCls _TradeTransactionObj, enErrorCode ErrorCode)
        {
            long TrnNo = 0;
            try
            {
                Req.Status = enTransactionStatus.SystemFail;
                InsertTransactionInQueue(Req, ref TrnNo);
                _TradeTransactionObj.TrnNo = TrnNo;
                Req.TrnNo = TrnNo;
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {
                    InsertTradeTransactionInQueue(Req, _TradeTransactionObj);
                    InsertTradeStopLoss(Req);
                }
                //DI of SMS here
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.Fail, ErrorCode = ErrorCode });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }
        }
        public BizResponse InsertTransactionInQueue(NewTransactionRequestCls NewtransactionReq, ref long TrnNo)
        {
            _Resp = new BizResponse();
            try
            {
                var Newtransaction = new TransactionQueue()
                {
                    GUID= Guid.NewGuid(),
                    TrnMode = NewtransactionReq.TrnMode,
                    TrnType = Convert.ToInt16(NewtransactionReq.TrnType),
                    MemberID = NewtransactionReq.MemberID,
                    MemberMobile = NewtransactionReq.MemberMobile,
                    TransactionAccount = NewtransactionReq.TransactionAccount,
                    SMSCode = NewtransactionReq.SMSCode,
                    Amount = NewtransactionReq.Amount,
                    Status = Convert.ToInt16(NewtransactionReq.Status),
                    StatusCode = 0,
                    StatusMsg = NewtransactionReq.StatusMsg,
                    TrnRefNo = NewtransactionReq.TrnRefNo,
                    AdditionalInfo = NewtransactionReq.AdditionalInfo
                };
                _TransactionRepository.Add(Newtransaction);
                NewtransactionReq.TrnNo = Newtransaction.Id;
                TrnNo = Newtransaction.Id;

                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public BizResponse InsertTradeTransactionInQueue(NewTransactionRequestCls NewtransactionReq, NewTradeTransactionRequestCls NewTradetransactionReq)
        {
            _Resp = new BizResponse();
            try
            {
                var Newtransaction = new TradeTransactionQueue()
                {
                    TrnNo = NewTradetransactionReq.TrnNo,
                    TrnType = Convert.ToInt16(NewtransactionReq.TrnType),
                    TrnTypeName = NewTradetransactionReq.TrnTypeName,
                    MemberID = NewtransactionReq.MemberID,
                    PairID = NewtransactionReq.PairID,
                    PairName = NewTradetransactionReq.PairName,
                    OrderWalletID = NewTradetransactionReq.OrderWalletID,
                    DeliveryWalletID = NewTradetransactionReq.DeliveryWalletID,
                    BuyQty = NewTradetransactionReq.BuyQty,
                    BidPrice = NewTradetransactionReq.BidPrice,
                    SellQty = NewTradetransactionReq.SellQty,
                    AskPrice = NewTradetransactionReq.AskPrice,
                    Order_Currency = NewTradetransactionReq.Order_Currency,
                    OrderTotalQty = NewTradetransactionReq.OrderTotalQty,
                    Delivery_Currency = NewTradetransactionReq.Delivery_Currency,
                    DeliveryTotalQty = NewTradetransactionReq.DeliveryTotalQty,
                    SettledBuyQty = NewTradetransactionReq.SettledBuyQty,
                    SettledSellQty = NewTradetransactionReq.SettledSellQty,
                    Status = Convert.ToInt16(NewtransactionReq.Status),
                    StatusCode = NewtransactionReq.StatusCode,
                    StatusMsg = NewtransactionReq.StatusMsg
                };
                _TradeTransactionRepository.Add(Newtransaction);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public BizResponse InsertTradeStopLoss(NewTransactionRequestCls NewtransactionReq)
        {
            _Resp = new BizResponse();
            try
            {
                var Newtransaction = new TradeStopLoss()
                {
                    TrnNo = NewtransactionReq.TrnNo,
                    ordertype = Convert.ToInt16(NewtransactionReq.ordertype)                   
                };
                _TradeStopLoss.Add(Newtransaction);
                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        #endregion        

        #region RegionProcessTransaction
        public Task<Boolean> ValidateTransaction(NewTransactionRequestCls Req)
        {
            return Task.FromResult(true);
        }
        #endregion


    }
}
