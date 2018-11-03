using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Data.Transaction
{
    public class SettlementRepository: ISettlementRepository<BizResponse>
    {
        private readonly CleanArchitectureContext _dbContext;
        //private readonly ILogger<SettlementRepository> _logger;
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly ICommonRepository<TradePoolQueue> _TradePoolQueue;
        private readonly ICommonRepository<TradeBuyRequest> _TradeBuyRequest;
        private readonly ICommonRepository<TradeBuyerList> _TradeBuyerList;
        private readonly ICommonRepository<TradeSellerList> _TradeSellerList;
        private readonly ICommonRepository<TradePoolMaster> _TradePoolMaster;
        private readonly ICommonRepository<PoolOrder> _PoolOrder;

        private readonly IWalletService _WalletService;

        string ControllerName = "SettlementRepository";
        TradePoolQueue TradePoolQueueObj;
        TradeBuyerList TradeBuyerListObj;
        PoolOrder PoolOrderObj;
        TransactionQueue TransactionQueueObj;
        TradeTransactionQueue TradeTransactionQueueObj;

        public SettlementRepository(CleanArchitectureContext dbContext, ICommonRepository<TradePoolQueue> TradePoolQueue,
            ICommonRepository<TradeBuyRequest> TradeBuyRequest, ICommonRepository<TradeBuyerList> TradeBuyerList,
            ICommonRepository<TradeSellerList> TradeSellerList, ICommonRepository<TradePoolMaster> TradePoolMaster,
            ICommonRepository<PoolOrder> PoolOrder, EFCommonRepository<TransactionQueue> TransactionRepository,
            EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository, IWalletService WalletService)
        {
            _dbContext = dbContext;
            //_logger = logger;
            _TradePoolQueue = TradePoolQueue;
            _TradeBuyRequest = TradeBuyRequest;
            _TradeBuyerList = TradeBuyerList;
            _TradeSellerList = TradeSellerList;
            _TradePoolMaster = TradePoolMaster;
            _PoolOrder = PoolOrder;
            _TransactionRepository = TransactionRepository;
            _TradeTransactionRepository = TradeTransactionRepository;
            _WalletService = WalletService;
        }

        #region ==============================PROCESS SETLLEMENT========================
        public void InsertTradePoolQueue(long MemberID,long MakerTrnNo, long PoolID, decimal MakerQty, decimal MakerPrice, long TakerTrnNo, decimal TakerQty, decimal TakerPrice, decimal TakerDisc, decimal TakerLoss,long SellerListID)
        {
            try
            {
                TradePoolQueueObj = new TradePoolQueue()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    CreatedBy = MemberID,
                    MakerTrnNo = MakerTrnNo,
                    PoolID = PoolID,
                    SellerListID = SellerListID,
                    MakerQty = MakerQty,
                    MakerPrice = MakerPrice,
                    TakerTrnNo = TakerTrnNo,
                    TakerQty = TakerQty,
                    TakerPrice = TakerPrice,
                    TakerDisc = TakerDisc,
                    TakerLoss = TakerLoss,
                    Status = Convert.ToInt16(enTransactionStatus.Success),//always etry after settlement done
                };
                TradePoolQueueObj = _TradePoolQueue.Add(TradePoolQueueObj);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("InsertTradePoolQueue:##TrnNo " + TakerTrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }
        public PoolOrder CreatePoolOrderForSettlement(long OMemberID, long DMemberID, long UserID, long PoolID, long TrnNo, decimal Amount,long CreditWalletID,string CreditAccountID)
        {
            try
            {
                PoolOrderObj = new PoolOrder()
                {
                    CreatedDate = Helpers.UTC_To_IST(),
                    CreatedBy = UserID,
                    UserID = UserID,
                    DMemberID = DMemberID, //Pool gives Amount to Member/User
                    OMemberID = OMemberID, //Member/User Take Amount from Pool
                    TrnNo = TrnNo,
                    TrnMode = 0,
                    PayMode = Convert.ToInt16(enWebAPIRouteType.TradeServiceLocal),
                    ORemarks = "Order Created",
                    OrderAmt = Amount,
                    DeliveryAmt = Amount,
                    DiscPer = 0,
                    DiscRs = 0,
                    Status = Convert.ToInt16(enTransactionStatus.Initialize),//txn type status
                    UserWalletID = CreditWalletID,
                    UserWalletAccID = CreditAccountID,
                };
                PoolOrderObj = _PoolOrder.Add(PoolOrderObj);
                return PoolOrderObj;
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("CreatePoolOrder:##TrnNo " + TrnNo, ControllerName, ex);
                //return (new BizResponse { ReturnMsg = EnResponseMessage.CommFailMsgInternal, ReturnCode = enResponseCodeService.InternalError });
                throw ex;
            }

        }

        public Task<BizResponse> PROCESSSETLLEMENT(BizResponse _Resp, TradeBuyRequest TradeBuyRequestObj, ref List<long> HoldTrnNos,string accesstocken = "")
        {
            string DebitAccountID;
            string CreditAccountID;
            long DebitWalletID;
            long CreditWalletID;
            short TrackBit=0;
            try
            {
                TransactionQueueObj = _TransactionRepository.GetById(TradeBuyRequestObj.TrnNo);
                TradeTransactionQueueObj = _TradeTransactionRepository.GetSingle(item=>item.TrnNo==TradeBuyRequestObj.TrnNo);
                TradeBuyerListObj = _TradeBuyerList.GetSingle(item => item.TrnNo == TradeBuyRequestObj.TrnNo);

                DebitWalletID = TradeTransactionQueueObj.OrderWalletID;
                DebitAccountID=_WalletService.GetAccWalletID(DebitWalletID);
                CreditWalletID = TradeTransactionQueueObj.DeliveryWalletID;
                CreditAccountID = _WalletService.GetAccWalletID(CreditWalletID);

                                
                TradeBuyRequestObj.MakeTransactionHold();
                TradeBuyRequestObj.UpdatedDate = Helpers.UTC_To_IST();
                TradeBuyRequestObj.IsProcessing = 1;
                _TradeBuyRequest.Update(TradeBuyRequestObj);
                
                TradeBuyerListObj.MakeTransactionHold();
                TradeBuyerListObj.UpdatedDate = Helpers.UTC_To_IST();
                TradeBuyerListObj.IsProcessing = 1;
                _TradeBuyerList.Update(TradeBuyerListObj);

                if(TradeTransactionQueueObj.IsCancelled==1)
                {
                    //Code for settlement
                    return Task.FromResult(_Resp);
                }
                if(TradeBuyRequestObj.PendingQty==0)
                {
                    _Resp.ErrorCode = enErrorCode.Settlement_AlreadySettled;
                    _Resp.ReturnCode = enResponseCodeService.Success;
                    _Resp.ReturnMsg = "ALready Settled";
                    return Task.FromResult(_Resp);
                }
                //SortedList<TradeSellerList, TradeSellerList>
                var MatchSellerListBase = _TradeSellerList.FindBy(item => item.Price <= TradeBuyRequestObj.BidPrice && item.IsProcessing == 0
                                                        && item.BuyServiceID == TradeBuyRequestObj.PaidServiceID &&
                                                        item.SellServiceID == TradeBuyRequestObj.ServiceID
                                                        && (item.Status == Convert.ToInt16(enTransactionStatus.Initialize) || item.Status == Convert.ToInt16(enTransactionStatus.Hold))
                                                        && item.RemainQty > 0);//Pending after partial Qty remain

                var MatchSellerList = MatchSellerListBase.OrderBy(x => x.Price).OrderBy(x => x.TrnNo);

                foreach (TradeSellerList SellerList in MatchSellerList)
                {
                    if (SellerList.IsProcessing == 1)
                        continue;

                    TrackBit = 1;
                    decimal SettlementQty = 0;
                    List<CreditWalletDrArryTrnID> CreditWalletDrArryTrnIDList = new List<CreditWalletDrArryTrnID>();
                    SellerList.IsProcessing = 1;
                    _TradeSellerList.Update(SellerList);
                    var PoolMst = _TradePoolMaster.GetById(SellerList.PoolID);

                    //====================================Partial SETTLEMENT TO MEMBER
                    if (SellerList.RemainQty < TradeBuyRequestObj.PendingQty)
                    {
                        SettlementQty = SellerList.RemainQty;//Take all Seller's Qty
                        //Topup Order create
                        PoolOrderObj = CreatePoolOrderForSettlement(TradeBuyRequestObj.UserID, SellerList.PoolID, TradeBuyRequestObj.UserID, SellerList.PoolID, TradeBuyRequestObj.TrnNo, SettlementQty, CreditWalletID,CreditAccountID);

                        TradeBuyRequestObj.PendingQty = TradeBuyRequestObj.PendingQty - SettlementQty;
                        TradeBuyRequestObj.DeliveredQty = TradeBuyRequestObj.DeliveredQty + SettlementQty;
                        TradeBuyerListObj.DeliveredQty = TradeBuyerListObj.DeliveredQty + SettlementQty;
                        //Here Bid Price of pool always low then user given in Order , base on above Query
                        decimal TakeDisc = 0;
                        if (SellerList.Price < TradeBuyRequestObj.BidPrice)
                        {
                            TakeDisc = (TradeBuyRequestObj.BidPrice - SellerList.Price) * SettlementQty;
                        }
                        InsertTradePoolQueue(TradeBuyRequestObj.UserID,SellerList.TrnNo, SellerList.PoolID, SellerList.RemainQty, SellerList.Price, TradeBuyRequestObj.TrnNo, SettlementQty, TradeBuyRequestObj.BidPrice, TakeDisc, 0, SellerList.Id);

                        SellerList.RemainQty = SellerList.RemainQty - SettlementQty;//this will give result 0
                        SellerList.MakeTransactionSuccess();
                        PoolMst.TotalQty = PoolMst.TotalQty - SettlementQty;                        
                        PoolOrderObj.MakeTransactionSuccess();
                        PoolOrderObj.DRemarks = "Delivery Success with " + SellerList.Price;

                        CreditWalletDrArryTrnIDList.Add(new CreditWalletDrArryTrnID { DrTrnRefNo = SellerList.TrnNo, Amount = SettlementQty });

                        _dbContext.Database.BeginTransaction();

                        _dbContext.Entry(PoolOrderObj).State = EntityState.Modified;
                        _dbContext.Entry(TradeBuyRequestObj).State = EntityState.Modified;
                        _dbContext.Entry(SellerList).State = EntityState.Modified;
                        _dbContext.Entry(PoolMst).State = EntityState.Modified;
                        _dbContext.Entry(TradeBuyerListObj).State = EntityState.Modified;
                       var CreditWalletResult =_WalletService.GetWalletCreditNew(TradeTransactionQueueObj.Delivery_Currency, Helpers.GetTimeStamp(), 
                                                        enWalletTrnType.Cr_Buy_Trade, SettlementQty, TradeBuyRequestObj.UserID,
                                                        CreditAccountID, CreditWalletDrArryTrnIDList.ToArray(), TradeBuyRequestObj.TrnNo, 0, 
                                                        enWalletTranxOrderType.Credit, enServiceType.Trading);
                        if(CreditWalletResult.ReturnCode==enResponseCode.Fail)
                        {
                            HelperForLog.WriteLogIntoFile("PROCESSSETLLEMENT RollbackTransaction", ControllerName, "Balance credit fail" + CreditWalletResult.ReturnMsg + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                            _dbContext.Database.RollbackTransaction();
                        }
                        else
                        {
                            _dbContext.SaveChanges();
                            _dbContext.Database.CommitTransaction();
                        }
                        HoldTrnNos.Add(SellerList.TrnNo);
                        HelperForLog.WriteLogIntoFile("PROCESSSETLLEMENT", ControllerName, "Partial Settlement Done with " + SellerList.TrnNo + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                        _Resp.ErrorCode = enErrorCode.Settlement_PartialSettlementDone;
                        _Resp.ReturnCode = enResponseCodeService.Success;
                        _Resp.ReturnMsg = "Partial Settlement Done of TrnNo "+ TradeBuyRequestObj.TrnNo + " Settled: "+ TradeBuyRequestObj.DeliveredQty + " Remain:"+ TradeBuyRequestObj.PendingQty;                        
                        //Continuew as record Partially settled
                    }
                    //====================================FULL SETTLEMENT TO MEMBER
                    else if (SellerList.RemainQty >= TradeBuyRequestObj.PendingQty && TradeBuyRequestObj.PendingQty!=0)
                    {
                        SettlementQty = TradeBuyRequestObj.PendingQty;
                        //Topup Order create
                        PoolOrderObj = CreatePoolOrderForSettlement(TradeBuyRequestObj.UserID, SellerList.PoolID, TradeBuyRequestObj.UserID, SellerList.PoolID, TradeBuyRequestObj.TrnNo, SettlementQty, CreditWalletID, CreditAccountID);

                        SellerList.RemainQty = SellerList.RemainQty - SettlementQty;//Update first as updated value in below line
                        SellerList.MakeTransactionHold();                    
                        PoolMst.TotalQty = PoolMst.TotalQty - SettlementQty;

                        //Here Bid Price of pool always low then user given in Order , base on above Query
                        decimal TakeDisc = 0;
                        if (SellerList.Price < TradeBuyRequestObj.BidPrice)
                        {
                            TakeDisc = (TradeBuyRequestObj.BidPrice - SellerList.Price) * SettlementQty;
                        }
                        InsertTradePoolQueue(TradeBuyRequestObj.UserID,SellerList.TrnNo, SellerList.PoolID, SellerList.RemainQty, SellerList.Price, TradeBuyRequestObj.TrnNo, SettlementQty, TradeBuyRequestObj.BidPrice, TakeDisc, 0, SellerList.Id);

                        TradeBuyRequestObj.DeliveredQty = TradeBuyRequestObj.DeliveredQty + SettlementQty;//Fully settled Here
                        TradeBuyRequestObj.PendingQty = TradeBuyRequestObj.PendingQty - SettlementQty;//this will 0
                        
                        TradeBuyRequestObj.MakeTransactionSuccess();
                        TradeBuyerListObj.MakeTransactionSuccess();                        
                        TransactionQueueObj.MakeTransactionSuccess();
                        TradeTransactionQueueObj.MakeTransactionSuccess();

                        TradeBuyerListObj.DeliveredQty = TradeBuyerListObj.DeliveredQty + SettlementQty;

                        PoolOrderObj.MakeTransactionSuccess();
                        PoolOrderObj.DRemarks = "Delivery Success with " + SellerList.Price;

                        CreditWalletDrArryTrnIDList.Add(new CreditWalletDrArryTrnID { DrTrnRefNo = SellerList.TrnNo, Amount = SettlementQty });

                        _dbContext.Database.BeginTransaction();

                        _dbContext.Entry(PoolOrderObj).State = EntityState.Modified;
                        _dbContext.Entry(TradeBuyRequestObj).State = EntityState.Modified;
                        _dbContext.Entry(SellerList).State = EntityState.Modified;
                        _dbContext.Entry(PoolMst).State = EntityState.Modified;
                        _dbContext.Entry(TradeBuyerListObj).State = EntityState.Modified;
                        _dbContext.Entry(TransactionQueueObj).State = EntityState.Modified;
                        _dbContext.Entry(TradeTransactionQueueObj).State = EntityState.Modified;
                        var CreditWalletResult = _WalletService.GetWalletCreditNew(TradeTransactionQueueObj.Delivery_Currency, Helpers.GetTimeStamp(),
                                                         enWalletTrnType.Cr_Buy_Trade, SettlementQty, TradeBuyRequestObj.UserID,
                                                         CreditAccountID, CreditWalletDrArryTrnIDList.ToArray(), TradeBuyRequestObj.TrnNo, 1,
                                                         enWalletTranxOrderType.Credit, enServiceType.Trading);
                        if (CreditWalletResult.ReturnCode == enResponseCode.Fail)
                        {
                            HelperForLog.WriteLogIntoFile("PROCESSSETLLEMENT RollbackTransaction", ControllerName, "Balance credit fail" + CreditWalletResult.ReturnMsg + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                            _dbContext.Database.RollbackTransaction();
                        }
                        else
                        {
                            _dbContext.SaveChanges();
                            _dbContext.Database.CommitTransaction();
                        }

                        HoldTrnNos.Add(SellerList.TrnNo);
                        HelperForLog.WriteLogIntoFile("PROCESSSETLLEMENT", ControllerName, "Full Settlement Done with " + SellerList.TrnNo + "##TrnNo:" + TradeBuyRequestObj.TrnNo);
                        _Resp.ErrorCode = enErrorCode.Settlement_FullSettlementDone;
                        _Resp.ReturnCode = enResponseCodeService.Success;
                        _Resp.ReturnMsg = "Full Settlement Done of TrnNo " + TradeBuyRequestObj.TrnNo + " Settled: " + TradeBuyRequestObj.DeliveredQty + " Remain:" + TradeBuyRequestObj.PendingQty;
                        break;//record settled
                    }
                    SellerList.IsProcessing = 0;//Release Seller List
                    _TradeSellerList.Update(SellerList);

                }
                TradeBuyRequestObj.IsProcessing = 0;//Release Buy Order
                _TradeBuyRequest.Update(TradeBuyRequestObj);
                TradeBuyerListObj.IsProcessing = 0;
                _TradeBuyerList.Update(TradeBuyerListObj);
                if(TrackBit==0)//No any record Process
                {
                    _Resp.ErrorCode = enErrorCode.Settlement_NoSettlementRecordFound;
                    _Resp.ReturnCode = enResponseCodeService.Success;
                    _Resp.ReturnMsg = "No Any Match Record Found";
                }

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("PROCESSSETLLEMENT:##TrnNo " + TradeBuyRequestObj.TrnNo, ControllerName, ex);
                _Resp.ReturnCode = enResponseCodeService.Fail;
                _Resp.ReturnMsg = ex.Message;
                _Resp.ErrorCode = enErrorCode.Settlement_SettlementInternalError;
            }
            return Task.FromResult(_Resp);
        }
        #endregion
    }
}
