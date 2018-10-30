using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction.BackOffice;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class BackOfficeTrnService : IBackOfficeTrnService
    {
        private readonly ILogger<FrontTrnService> _logger;
        private readonly IBackOfficeTrnRepository _backOfficeTrnRepository;
        private readonly ICommonRepository<TransactionQueue> _transactionQueueRepository;
        private readonly ICommonRepository<TradeTransactionQueue> _tradeTransactionRepository;
        private readonly ICommonRepository<TradeBuyRequest> _tradeBuyRepository;
        private readonly ICommonRepository<TradeCancelQueue> _tradeCancelQueueRepository;
        private readonly IBasePage _basePage;
        private readonly ICommonRepository<PoolOrder> _poolOrderRepository;
        private readonly ICommonRepository<TradePoolMaster> _tradePoolMasterRepository;

        public BackOfficeTrnService(ILogger<FrontTrnService> logger,
            IBackOfficeTrnRepository backOfficeTrnRepository,
            ICommonRepository<TransactionQueue> transactionQueueRepository,
            ICommonRepository<TradeTransactionQueue> tradeTransactionRepository,
            ICommonRepository<TradeBuyRequest> tradeBuyRepository,
            ICommonRepository<TradeCancelQueue> tradeCancelQueueRepository,
            IBasePage basePage,
            ICommonRepository<PoolOrder> poolOrderRepository,
            ICommonRepository<TradePoolMaster> tradePoolMasterRepository
            )
        {
            _logger = logger;
            _backOfficeTrnRepository = backOfficeTrnRepository;
            _transactionQueueRepository = transactionQueueRepository;
            _tradeTransactionRepository = tradeTransactionRepository;
            _tradeBuyRepository = tradeBuyRepository;
            _tradeCancelQueueRepository = tradeCancelQueueRepository;
            _basePage = basePage;
            _poolOrderRepository = poolOrderRepository;
            _tradePoolMasterRepository = tradePoolMasterRepository;
        }

        public List<TradingSummaryViewModel> GetTradingSummary(long MemberID, string FromDate, string ToDate, long TrnNo, short status, string SMSCode, long PairID, short trade)
        {
            try
            {
                List<TradingSummaryViewModel> list = new List<TradingSummaryViewModel>();
                var Modellist = _backOfficeTrnRepository.GetTradingSummary(MemberID, FromDate, ToDate, TrnNo, status, SMSCode, PairID, trade);
                if (Modellist == null)
                    return null;

                foreach (var model in Modellist)
                {
                    list.Add(new TradingSummaryViewModel()
                    {
                        Amount = model.Amount,
                        ChargeRs = model.ChargeRs,
                        DateTime = model.DateTime.Date,
                        MemberID =Convert.ToInt64 (model .MemberID),
                        PairID =model .PairID,
                        PairName =model .PairName,
                        PostBal =model .PostBal,
                        PreBal =model .PreBal,
                        Price =model .Price,
                        StatusText =model .StatusText,
                        Total = model.Type == "BUY" ? ((model.Price * model.Amount) - model.ChargeRs) : ((model.Price * model.Amount)),
                        TrnNo =model .TrnNo,
                        Type =model .Type 
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public BizResponseClass TradeRecon(long TranNo, string ActionMessage,long UserId)
        {
            BizResponseClass Response = new BizResponseClass();
            try
            {
                var transactionQueue = _transactionQueueRepository.GetById(TranNo);
                var tradeTranQueue = _tradeTransactionRepository.GetSingle(x => x.TrnNo == TranNo);
                var tradeBuyRequest = _tradeBuyRepository.GetSingle(x => x.TrnNo == TranNo);

                if (transactionQueue != null && transactionQueue != null && tradeBuyRequest != null)
                {
                    var datediff = _basePage.UTC_To_IST() - transactionQueue.TrnDate;
                    if(UserId != 1 && datediff.Days > 7)
                    {
                        //After 7 days of transaction you can not take action, Please contact admin
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.TradeRecon_After7DaysTranDontTakeAction;
                        Response.ErrorCode = enErrorCode.TradeRecon_After7DaysTranDontTakeAction;
                       
                    }
                    else if(transactionQueue.Status != 4)
                    {
                        //Invalid Transaction Status For Trade Recon
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.TradeRecon_InvalidTransactionStatus;
                        Response.ErrorCode = enErrorCode.TradeRecon_InvalidTransactionStatus;

                    }
                    else if(tradeTranQueue.IsCancelled == 1)
                    {
                        //Transaction Cancellation request is already in processing.
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.TradeRecon_CancelRequestAlreayInProcess;
                        Response.ErrorCode = enErrorCode.TradeRecon_CancelRequestAlreayInProcess;
                    }
                    else if(tradeBuyRequest.IsProcessing == 1)
                    {
                        //Transaction Already in Process, Please try After Sometime
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.TradeRecon_TransactionAlreadyInProcess;
                        Response.ErrorCode = enErrorCode.TradeRecon_TransactionAlreadyInProcess;
                    }
                    else if(tradeBuyRequest.PendingQty == 0)
                    {
                        //Can not initiate Cancellation Request.Your order is fully executed
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.TradeRecon_OrderIsFullyExecuted;
                        Response.ErrorCode = enErrorCode.TradeRecon_OrderIsFullyExecuted;
                    }
                    else
                    {
                        var DeliveryQty = Math.Round((transactionQueue.Amount * tradeBuyRequest.PendingQty) / tradeBuyRequest.Qty,8);

                        if(DeliveryQty == 0 || DeliveryQty < 0)
                        {
                            //Invalid Delivery Amount
                            Response.ReturnCode = enResponseCode.Fail;
                            Response.ReturnMsg = EnResponseMessage.TradeRecon_InvalidDeliveryAmount;
                            Response.ErrorCode = enErrorCode.TradeRecon_InvalidDeliveryAmount;
                        }
                        if(DeliveryQty > transactionQueue.Amount)
                        {
                            //Invalid Delivery Amount
                            Response.ReturnCode = enResponseCode.Fail;
                            Response.ReturnMsg = EnResponseMessage.TradeRecon_InvalidDeliveryAmount;
                            Response.ErrorCode = enErrorCode.TradeRecon_InvalidDeliveryAmount;
                        }
                        else
                        {
                            //Add record in Transaction Cancel Queue
                            var tradeCancelQueue = new TradeCancelQueue()
                            {
                                TrnNo = TranNo,
                                DeliverServiceID = transactionQueue.ServiceID,
                                TrnDate = _basePage.UTC_To_IST(),
                                PendingBuyQty = tradeBuyRequest.PendingQty,
                                DeliverQty = DeliveryQty,
                                Status = 0,
                                StatusMsg = "Cancel Order",
                                CreatedBy = UserId,
                                CreatedDate = _basePage.UTC_To_IST()
                            };

                            tradeCancelQueue = _tradeCancelQueueRepository.Add(tradeCancelQueue);

                            //Add record in PoolOrder
                            var poolOrder = new PoolOrder()
                            {

                                CreatedDate = _basePage.UTC_To_IST(),
                                CreatedBy = transactionQueue.MemberID,
                                TrnMode = Convert.ToByte(transactionQueue.TrnMode),
                                PayMode = Convert.ToInt16(enWebAPIRouteType.TradeServiceLocal),
                                ORemarks = "Cancellation Initiated",
                                OrderAmt = DeliveryQty,
                                DiscPer = 0,
                                DiscRs = 0,
                                Status = Convert.ToInt16(enTransactionStatus.Initialize),//txn type status
                                UserWalletID = tradeTranQueue.OrderWalletID,
                                //UserWalletAccID = tradeTranQueue.OrderWalletID,
                                TrnNo = TranNo,
                                CancelID = tradeCancelQueue.Id,
                                DeliveryAmt = DeliveryQty,
                                DRemarks = "Cancel Order",

                                //OrderDate = _basePage.UTC_To_IST(),
                                //TrnMode = Convert.ToByte(transactionQueue.TrnMode),
                                //OMemberID = transactionQueue.MemberID,
                                //PayMode = 2,
                                //OrderAmt = DeliveryQty,
                                //DiscPer = 0,
                                //DiscRs = 0,
                                //OBankID = 0,
                                //OBranchName = "",
                                //OAccountNo = "",
                                //OChequeNo = "",
                                //DMemberID = tradeBuyRequest.SellStockID,
                                //DBankID = 0,
                                //DAccountNo = "",
                                //Status = 0,
                                //ORemarks = "",                             
                                //AlertRec = 0,
                                //CashChargePer = 0,
                                //CashChargeRs = 0,
                                //WalletAmt = 0,
                                //PGId = 0,
                                //CouponNo = 0,
                                //IsChargeAccepted = false,
                                //WalletID = tradeTranQueue.OrderWalletID,                              
                                //CreatedBy = UserId,
                                //CreatedDate = _basePage.UTC_To_IST()
                            };

                            poolOrder = _poolOrderRepository.Add(poolOrder);

                            //Update TradeBuyRequest
                            tradeBuyRequest.UpdatedDate = _basePage.UTC_To_IST();
                            tradeBuyRequest.UpdatedBy = UserId;
                            tradeBuyRequest.IsCancel = 1;
                            _tradeBuyRepository.Update(tradeBuyRequest);

                            //Update TradeTransaction Queue
                            tradeTranQueue.UpdatedDate = _basePage.UTC_To_IST();
                            tradeTranQueue.UpdatedBy = UserId;
                            tradeTranQueue.IsCancelled = 1;
                            tradeTranQueue.StatusMsg = "Cancellation Initiated";
                            _tradeTransactionRepository.Update(tradeTranQueue);

                            //Update OrderID in TransactionCancel Queue
                            tradeCancelQueue.OrderID = poolOrder.Id;
                            _tradeCancelQueueRepository.Update(tradeCancelQueue);

                            var tradePoolMaster = _tradePoolMasterRepository.GetSingle(x => x.Id == tradeBuyRequest.SellStockID && x.IsSleepMode == 1);
                            if(tradePoolMaster != null)
                            {
                                tradePoolMaster.IsSleepMode = 0;
                                _tradePoolMasterRepository.Update(tradePoolMaster);
                            }

                            Response.ReturnCode = enResponseCode.Success;
                            Response.ReturnMsg = EnResponseMessage.TradeRecon_CencelRequestSuccess;
                            Response.ErrorCode = enErrorCode.TradeRecon_CencelRequestSuccess;
                        }
                    }
                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.TradeRecon_InvalidTransactionNo;
                    Response.ErrorCode = enErrorCode.TradeRecon_InvalidTransactionNo;
                }

                return Response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
