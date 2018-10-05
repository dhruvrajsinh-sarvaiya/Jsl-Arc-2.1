using CleanArchitecture.Core.ApiModels;
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
            //string PairName = "";
            decimal BuyQty = 0;
            decimal BidPrice = 0;
            decimal SellQty = 0;
            decimal AskPrice = 0;
            //long TradeWalletMasterID = 0;
            //long TradeServiceID = 0;
            //long WalletServiceID = 0;
            string INRSMSCode = "";
            string Amount2 = "";
            string MemberMobile1 = "";
            TradePairMaster _TradePairObj;
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
                    _TradePairObj= _TradePairMaster.GetById(Req.PairID);
                    //Select @WalletServiceID=WalletServiceID,@TradeWalletMasterID=WalletMasterID,@TradeServiceID=ServiceID,
                    //@PairName =PairName From TradePairMaster Where PairID=@PairID AND Status=1--Incase of PairID=0
                    
                    //if (TradeWalletMasterID == 0 || TradeWalletMasterID == null)
                    //{

                    //}


                }


                return (new BizResponse { ReturnMsg = "", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);                
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError });
            }            

        }
        public BizResponse InsertTransactionInQueue(NewTransactionRequestCls NewtransactionReq)
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
                    Status = 0,
                    StatusCode = 0,
                    StatusMsg = "Initialise",
                    TrnRefNo = NewtransactionReq.TrnRefNo,
                    AdditionalInfo = NewtransactionReq.AdditionalInfo
                };
                _TransactionRepository.Add(Newtransaction);
                return (new BizResponse { ReturnMsg = "Success", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError });
            }

        }

    }
}
