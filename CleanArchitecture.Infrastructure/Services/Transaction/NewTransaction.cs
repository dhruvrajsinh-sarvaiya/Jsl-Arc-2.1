﻿using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.DTOClasses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class NewTransaction
    {
        private readonly ICommonRepository<TradePairMaster> _TradePairMaster;
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        BizResponse _Resp;
        readonly ILogger _log;
        BizResponse _CreateTransactionResp;


        public NewTransaction(ILogger log, ICommonRepository<TradePairMaster> TradePairMaster, EFCommonRepository<TransactionQueue> TransactionRepository)
        {
            _log = log;
            _TradePairMaster = TradePairMaster;
            _TransactionRepository = TransactionRepository;
        }

        public BizResponse ProcessNewTransaction(NewTransactionRequestCls Req)
        {
            _Resp = new BizResponse();
           

            //=========================INSERT

            //Take memberMobile for sms



            //=========================PROCESS



            //=========================UPDATE



            return null;
        }

        public BizResponse CreateTransaction(NewTransactionRequestCls Req)
        {
            string TrnTypeName = "";
            decimal Balance = 0;
            long TABactchNo = 0;
            string ServiceName = "";
            string Order_Currency = "";
            decimal OrderTotalQty =0;
            string Delivery_Currency = "";
            decimal DeliveryTotalQty = 0;
            long OrderWalletID = 0;
            long DeliveryWalletID = 0;          
            decimal BuyQty = 0;
            decimal BidPrice = 0;
            decimal SellQty = 0;
            decimal AskPrice = 0;          
            string INRSMSCode = "INR";
            string Amount2 = "";
            string MemberMobile1 = "";
            TradePairMaster _TradePairObj;           
            NewTradeTransactionRequestCls _TradeTransactionObj=new NewTradeTransactionRequestCls();
            //string PairName = "";
            //long TradeWalletMasterID = 0;
            //long TradeServiceID = 0;
            //long WalletServiceID = 0;
            try
            {
                if(Req.TrnType == enTrnType.Buy_Trade)
                {
                    TrnTypeName = "BUY";
                }
                else if (Req.TrnType == enTrnType.Sell_Trade)
                {
                    TrnTypeName = "SELL";
                }
                //IF @PairID <> 0 ntrivedi 18-04-2018  check inside @TrnType (4,5) @TradeWalletMasterID will be 0 or null
                if (Req.TrnType == enTrnType.Buy_Trade || Req.TrnType == enTrnType.Sell_Trade)
                {

                    _TradePairObj = new TradePairMaster();                  
                    _TradePairObj = _TradePairMaster.GetSingle(item => item.Id == Req.PairID && item.Status == Convert.ToInt16(ServiceStatus.Active)); 
                    if (_TradePairObj.WalletMasterID == 0)
                    {                       
                        Req.StatusMsg = EnResponseMessage.CreateTrnNoPairSelectedMsg;
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj);
                    }
                    if(Req.Qty<=0 || Req.Price<=0)
                    {                       
                        Req.StatusMsg = EnResponseMessage.CreateTrnInvalidQtyPriceMsg;                       
                        return MarkSystemFailTransaction(Req, _TradeTransactionObj);
                    }


                }


                return (new BizResponse { ReturnMsg = "", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);                
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError, ErrorCode = enErrorCode.TransactionInsertInternalError });
            }            

        }
        public BizResponse MarkSystemFailTransaction(NewTransactionRequestCls Req, NewTradeTransactionRequestCls _TradeTransactionObj)
        {
            long TrnNo = 0;
            try
            {
                Req.Status = enTransactionStatus.SystemFail;
                InsertTransactionInQueue(Req, ref TrnNo);
                _TradeTransactionObj.TrnNo = TrnNo;
                InsertTradeTransactionInQueue(Req, _TradeTransactionObj);
                //DI of SMS here
                return (new BizResponse { ReturnMsg = Req.StatusMsg, ReturnCode = enResponseCodeService.Fail });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError , ErrorCode =enErrorCode.TransactionInsertInternalError });
            }
        }
        public BizResponse InsertTransactionInQueue(NewTransactionRequestCls NewtransactionReq,ref long TrnNo)
        {
            _Resp = new BizResponse();
            try
            {
                var Newtransaction = new TransactionQueue()
                {
                    TrnMode = NewtransactionReq.TrnMode,
                    TrnType = Convert.ToInt16(NewtransactionReq.TrnType),
                    MemberID = NewtransactionReq.MemberID,
                    MemberMobile = NewtransactionReq.MemberMobile,
                    TransactionAccount = NewtransactionReq.TransactionAccount,
                    SMSCode = NewtransactionReq.SMSCode,
                    Amount = NewtransactionReq.Amount,
                    Status = Convert.ToInt16(NewtransactionReq.Status),
                    StatusCode = 0,
                    StatusMsg = "Initialise",
                    TrnRefNo = NewtransactionReq.TrnRefNo,
                    AdditionalInfo = NewtransactionReq.AdditionalInfo
                };
                _TransactionRepository.Add(Newtransaction);
                TrnNo = Newtransaction.Id;

                return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError });
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
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError });
            }

        }

    }
}