using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Transaction
{
    public class FrontTrnRepository : IFrontTrnRepository
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly ILogger<FrontTrnRepository> _logger;

        public FrontTrnRepository(CleanArchitectureContext dbContext, ILogger<FrontTrnRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<ActiveOrderDataResponse> GetActiveOrder(long MemberID, string sCondition, string FromDate, string ToDate, long PairId)
        {
            string Qry = "";
            IQueryable<ActiveOrderDataResponse> Result;
            DateTime fDate, tDate;
            try
            {
                Qry = @"Select TQ.Id,TSL.ordertype,TTQ.PairName,TTQ.PairId,TQ.TrnDate,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,TTQ.Order_Currency,TTQ.Delivery_Currency, " +
                   "ISNULL((CASE WHEN TTQ.BuyQty = 0 THEN TTQ.SellQty WHEN TTQ.SellQty = 0 THEN TTQ.BuyQty END),0) as Amount, "+
                   "ISNULL((CASE WHEN TTQ.BidPrice = 0 THEN ISNULL(TTQ.AskPrice,0) WHEN TTQ.AskPrice = 0 THEN ISNULL(TTQ.BidPrice,0) END),0) as Price, " +
                   "TTQ.IsCancelled from TransactionQueue TQ INNER JOIN TradeTransactionQueue TTQ ON TQ.Id = TTQ.TrnNo INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                   "Where "+ sCondition  +" AND TQ.Status ={1} And TQ.MemberID ={0} ";
                
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    fDate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    tDate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    Result = _dbContext.ActiveOrderDataResponse.FromSql(Qry
                   , MemberID, Convert.ToInt16(enTransactionStatus.Hold),fDate ,tDate);
                }
                else
                    Result = _dbContext.ActiveOrderDataResponse.FromSql(Qry,MemberID, Convert.ToInt16(enTransactionStatus.Hold));

                return Result.ToList();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
            
        }

        public long GetPairIdByName(string pair)
        {

            try
            {
                var model = _dbContext.TradePairMaster.Where(p => p.PairName == pair && p.Status ==1).FirstOrDefault();
                if (model == null)
                    return 0;

                return model.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
            
        }

        public List<RecentOrderRespose> GetRecentOrder(long PairId, long MemberID)
        {
            string sCondition = "";
            IQueryable<RecentOrderRespose> Result;
            try
            {
                if (PairId != 999)
                    sCondition = " AND TTQ.PairID ={4} ";

                string Qry = "Select TTQ.TrnNo,TSL.ordertype,TTQ.PairName,TTQ.PairId,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type, " +
                        "ISNULL((CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END),0) as Price, "+
                        "ISNULL((CASE WHEN TTQ.Status =1 AND TTQ.TrnType=4 THEN TTQ.SettledBuyQty WHEN TTQ.Status=1 AND TTQ.TrnType=5 THEN TTQ.SettledSellQty WHEN TTQ.Status =4 AND TTQ.TrnType=4 THEN TTQ.BuyQty WHEN TTQ.Status=4 AND TTQ.TrnType=5 THEN TTQ.SellQty END),0) as Qty, " +
                        "TTQ.TrnDate as DateTime,TTQ.Status from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id = TTQ.TrnNo INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                        "WHERE TTQ.MemberID ={0} AND TTQ.Status in ({1},{2}) AND TQ.TrnDate > DATEADD(HOUR, -24, getdate()) "+ sCondition +
                        "UNION ALL Select TTQ.TrnNo,TSL.ordertype,TTQ.PairName,TTQ.PairId,CASE WHEN TTQ.TrnType = 4 THEN 'BUY' WHEN TTQ.TrnType = 5 THEN 'SELL' END as Type, " +
                        "ISNULL((CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END),0) as Price, "+
                        "ISNULL((CASE WHEN TTQ.TrnType = 4 THEN TCQ.PendingBuyQty else TCQ.DeliverQty END),0) as Qty, "+
                        "TTQ.TrnDate as DateTime,TTQ.Status from TradeCancelQueue TCQ INNER JOIN TradeTransactionQueue TTQ ON TTQ.TrnNo = TCQ.TrnNo  " +
                        "INNER JOIN TransactionQueue TQ ON TQ.Id = TTQ.TrnNo INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id WHERE  TTQ.MemberID ={0} " +
                        "AND TCQ.Status in ({2}) AND TQ.TrnDate > DATEADD(HOUR, -24, getdate()) "+ sCondition +
                        "UNION ALL Select TTQ.TrnNo,TSL.ordertype,TTQ.PairName,TTQ.PairId,CASE WHEN TTQ.TrnType = 4 THEN 'BUY' WHEN TTQ.TrnType = 5 THEN 'SELL' END as Type, " +
                        "ISNULL((CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END),0) as Price,  "+
                        "ISNULL((CASE WHEN TTQ.TrnType = 4  THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END),0) as Qty, "+
                        "TTQ.TrnDate as DateTime,TTQ.Status from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id = TTQ.TrnNo INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                        "WHERE TTQ.MemberID ={0} AND TTQ.Status in ({3}) AND TQ.TrnDate > DATEADD(HOUR, -24, getdate()) "+ sCondition + " Order By TTQ.TrnDate Desc";

                if(PairId == 999)
                    Result = _dbContext.RecentOrderRespose.FromSql(Qry, MemberID, Convert.ToInt16(enTransactionStatus.Success), Convert.ToInt16(enTransactionStatus.Hold), Convert.ToInt16(enTransactionStatus.SystemFail));
                else
                    Result = _dbContext.RecentOrderRespose.FromSql(Qry, MemberID, Convert.ToInt16(enTransactionStatus.Success), Convert.ToInt16(enTransactionStatus.Hold), Convert.ToInt16(enTransactionStatus.SystemFail),PairId);

                return Result.ToList();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<TradeHistoryResponce> GetTradeHistory(long MemberID, string sCondition, string FromDate, string ToDate, int page, int IsAll,long TrnNo=0)
        {
            IQueryable<TradeHistoryResponce> Result = null;
            string qry = "";
            DateTime fDate, tDate;

            try
            {
                if (IsAll == 0)
                {
                    var sCon = "";
                   
                    long PairId = MemberID;
                    if (PairId != 999)
                        sCon = "and TTQ.PairID ="+ PairId;
                    if (TrnNo != 0)
                        sCon = "and TQ.TrnNo =" + TrnNo;

                    qry = "Select top 100 TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type, " +
                                "ISNULL((CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END),0) as Price, "+
                                "ISNULL((CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END),0) as Amount, "+
                                 "TTQ.SettledDate as DateTime, TTQ.Status, TTQ.StatusMsg as StatusText, TP.PairName,ISNULL(TQ.ChargeRs, 0) as ChargeRs,TTQ.IsCancelled from "+
                                 "TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id = TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id = TTQ.PairID INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                                 "WHERE TTQ.Status in ({0}) and (TTQ.SettledSellQty > 0 or TTQ.SettledBuyQty > 0) "+ sCon + " Order By TTQ.SettledDate DESC";
                    Result = _dbContext.TradeHistoryInfo.FromSql(qry, Convert.ToInt16(enTransactionStatus.Success));
                    //Result = _dbContext.TradeHistoryInfo.FromSql(
                    //         @"Select top 100 TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,
                    //            CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,
                    //            CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount,
                    //             TTQ.SettledDate as DateTime, TTQ.Status, TTQ.StatusMsg as StatusText, TP.PairName, ISNULL(TQ.ChargeRs,0)as ChargeRs,TTQ.IsCancelled from
                    //             SettledTradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id  = TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID
                    //             WHERE TTQ.Status in ({0}) and (TTQ.SettledSellQty>0 or TTQ.SettledBuyQty>0) 
                    //             Order By TTQ.SettledDate DESC", Convert.ToInt16(enTransactionStatus.Success), PairId);
                }
                else
                {
                    if (IsAll == 1)
                    {
                        qry = @"Select  TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type, " +
                                   "ISNULL((CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END),0) as Price, " +
                                   "ISNULL((CASE WHEN TTQ.TrnType = 4  THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END),0) as Amount," +
                                   "TTQ.TrnDate as DateTime,TTQ.Status,TTQ.StatusMsg as StatusText,TP.PairName,ISNULL(TQ.ChargeRs,0)as ChargeRs,TTQ.IsCancelled " +
                                   "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id = TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                                   "WHERE " + sCondition + " AND TTQ.IsCancelled=0 AND TTQ.MemberID=" + MemberID + " AND TTQ.Status=" + Convert.ToInt16(enTransactionStatus.Success) + " Order By TrnNo desc";

                    }
                    else if (IsAll == 2)
                    {
                        qry = @"Select  TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type," +
                               " ISNULL((CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END),0) as Price," +
                                "ISNULL((CASE WHEN TTQ.TrnType = 4  THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END),0) as Amount,TTQ.TrnDate as DateTime," +
                                "TTQ.Status,TTQ.StatusMsg as StatusText,TP.PairName,ISNULL(TQ.ChargeRs,0)as ChargeRs,TTQ.IsCancelled " +
                                "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id=TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                                "WHERE " + sCondition + "AND TTQ.Status=" + Convert.ToInt16(enTransactionStatus.SystemFail) + " AND TTQ.MemberID=" + MemberID + " Order By TrnNo desc";

                    }
                    else if (IsAll == 9)
                    {
                        qry = @"Select TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type," +
                               " ISNULL((CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END),0) as Price," +
                                "ISNULL((CASE WHEN TTQ.TrnType = 4 THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END),0) as Amount,TTQ.TrnDate as DateTime," +
                                "TTQ.Status,TTQ.StatusMsg as StatusText,TP.PairName,ISNULL(TQ.ChargeRs,0)as ChargeRs,TTQ.IsCancelled " +
                                "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id=TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID  INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                                "WHERE " + sCondition + "AND TTQ.Status in(" + Convert.ToInt16(enTransactionStatus.Success) + "," + Convert.ToInt16(enTransactionStatus.OperatorFail) +
                                ") AND TTQ.MemberID=" + MemberID + " AND TTQ.IsCancelled=1  Order By TrnNo desc";

                    }
                    else
                    {
                        qry = @"Select TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,ISNULL((CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice " +
                                " WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END),0) as Price,ISNULL((CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END),0) as Amount,TTQ.TrnDate as DateTime,TTQ.Status,TTQ.StatusMsg as StatusText," +
                                " TP.PairName,ISNULL(TQ.ChargeRs,0)as ChargeRs,CAST(0 as smallint) As IsCancelled from  TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id  = TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id " +
                                " WHERE " + sCondition + " AND TTQ.MemberID=" + MemberID + "  and TTQ.Status =" + Convert.ToInt16(enTransactionStatus.Success) +
                                " UNION  ALL Select TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,ISNULL((CASE WHEN TTQ.BidPrice=0 THEN " +
                                " TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END),0) as Price,ISNULL((CASE WHEN TTQ.TrnType = 4 THEN TCQ.PendingBuyQty else TCQ.DeliverQty END),0) as Amount,TTQ.TrnDate as DateTime,TTQ.Status, " +
                                " TCQ.StatusMsg as StatusText,TP.PairName,ISNULL(TQ.ChargeRs,0)as ChargeRs,CAST(1 as smallint) as 'IsCancelled' from TradeCancelQueue TCQ INNER JOIN TradeTransactionQueue TTQ ON TTQ.TrnNo = TCQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID" +
                                " INNER JOIN TransactionQueue TQ ON TQ.Id  = TTQ.TrnNo INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id  WHERE " + sCondition + " AND TTQ.MemberID=" + MemberID + " and TCQ.Status=" + Convert.ToInt16(enTransactionStatus.Success) +
                                " UNION ALL Select TTQ.TrnNo,TSL.ordertype,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,ISNULL((CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END),0) as Price, " +
                                " ISNULL((CASE WHEN TTQ.TrnType = 4  THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END),0) as Amount,TTQ.TrnDate as DateTime,TTQ.Status,TTQ.StatusMsg as StatusText,TP.PairName,ISNULL(TQ.ChargeRs,0)as ChargeRs,TTQ.IsCancelled " +
                                " from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id=TTQ.TrnNo INNER JOIN TradePairMaster TP ON TP.Id =TTQ.PairID INNER JOIN TradeStopLoss TSL ON TSL.TrnNo =TQ.Id  WHERE " + sCondition + " AND TTQ.MemberID=" + MemberID + " AND TTQ.Status=" + Convert.ToInt16(enTransactionStatus.SystemFail) +
                                " Order By TTQ.TrnNo Desc";

                    }
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        fDate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        tDate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        Result = _dbContext.TradeHistoryInfo.FromSql(qry, FromDate, ToDate);
                    }

                    else
                        Result = _dbContext.TradeHistoryInfo.FromSql(qry);

                }
                return Result.ToList();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        public List<GetBuySellBook> GetBuyerBook(long id,decimal Price=-0)
        {
            try
            {
                IQueryable<GetBuySellBook> Result;

                //Uday  05-11-2018 As Per Instruction by ritamam not get the OrderId From TradePoolMaster
                
                //if (Price != -0)
                //{
                //    Result = _dbContext.BuyerSellerInfo.FromSql(
                //                  @"Select Top 100 TTQ.BidPrice As Price, Sum(TTQ.DeliveryTotalQty) - Sum(TTQ.SettledBuyQty) As Amount,
                //                Count(TTQ.BidPrice) As RecordCount,(Select Top 1 GUID From TradePoolMaster TPM Where TPM.BidPrice = TTQ.BidPrice And TPM.PairId = TTQ.PairID) As OrderId
                //                From TradeTransactionQueue TTQ  Where TTQ.Status = 4 and TTQ.TrnType = 4 AND TTQ.PairID = {0}
                //                AND TTQ.IsCancelled = 0 AND TTQ.BidPrice={1} Group By TTQ.BidPrice,PairID Order By TTQ.BidPrice desc", id, Price);
                //}
                //else
                //{
                //    Result = _dbContext.BuyerSellerInfo.FromSql(
                //                  @"Select Top 100 TTQ.BidPrice As Price, Sum(TTQ.DeliveryTotalQty) - Sum(TTQ.SettledBuyQty) As Amount,
                //                Count(TTQ.BidPrice) As RecordCount,(Select Top 1 GUID From TradePoolMaster TPM Where TPM.BidPrice = TTQ.BidPrice And TPM.PairId = TTQ.PairID) As OrderId
                //                From TradeTransactionQueue TTQ  Where TTQ.Status = 4 and TTQ.TrnType = 4 AND TTQ.PairID = {0}
                //                AND TTQ.IsCancelled = 0 Group By TTQ.BidPrice,PairID Order By TTQ.BidPrice desc", id);
                //}

                if (Price != -0)
                {
                    Result = _dbContext.BuyerSellerInfo.FromSql(
                                  @"Select Top 100 TTQ.BidPrice As Price, Sum(TTQ.DeliveryTotalQty) - Sum(TTQ.SettledBuyQty) As Amount,
                                Count(TTQ.BidPrice) As RecordCount,NEWID() As OrderId
                                From TradeTransactionQueue TTQ  Where TTQ.Status = 4 and TTQ.TrnType = 4 AND TTQ.PairID = {0}
                                AND TTQ.IsCancelled = 0 AND TTQ.BidPrice={1} Group By TTQ.BidPrice,PairID Order By TTQ.BidPrice desc", id, Price);
                }
                else
                {
                    Result = _dbContext.BuyerSellerInfo.FromSql(
                                  @"Select Top 100 TTQ.BidPrice As Price, Sum(TTQ.DeliveryTotalQty) - Sum(TTQ.SettledBuyQty) As Amount,
                                Count(TTQ.BidPrice) As RecordCount,NEWID() As OrderId
                                From TradeTransactionQueue TTQ  Where TTQ.Status = 4 and TTQ.TrnType = 4 AND TTQ.PairID = {0}
                                AND TTQ.IsCancelled = 0 Group By TTQ.BidPrice,PairID Order By TTQ.BidPrice desc", id);
                }


                return Result.ToList();
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name + " #PairId# : " + id + " #Price# : " + Price, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<GetBuySellBook> GetSellerBook(long id, decimal Price = -0)
        {
            try
            {
                IQueryable<GetBuySellBook> Result;
                //Uday  05-11-2018 As Per Instruction by ritamam not get the OrderId From TradePoolMaster
                
                //if (Price != -0)
                //{
                //    Result = _dbContext.BuyerSellerInfo.FromSql(
                //                @"Select Top 100 TTQ.AskPrice As Price,sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty) as Amount,
                //              Count(TTQ.AskPrice) As RecordCount,(Select Top 1 GUID From TradePoolMaster TPM Where TPM.BidPrice = TTQ.AskPrice And TPM.PairId = TTQ.PairID) As OrderId
                //              from TradeTransactionQueue TTQ Where TTQ.Status = 4 and TTQ.TrnType = 5 AND 
                //              TTQ.pairID = {0} AND TTQ.IsCancelled = 0 AND TTQ.AskPrice={1} Group by TTQ.AskPrice,PairID order by TTQ.AskPrice", id, Price);
                //}
                //else
                //{
                //    Result = _dbContext.BuyerSellerInfo.FromSql(
                //                @"Select Top 100 TTQ.AskPrice As Price,sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty) as Amount,
                //              Count(TTQ.AskPrice) As RecordCount,(Select Top 1 GUID From TradePoolMaster TPM Where TPM.BidPrice = TTQ.AskPrice And TPM.PairId = TTQ.PairID) As OrderId
                //              from TradeTransactionQueue TTQ Where TTQ.Status = 4 and TTQ.TrnType = 5 AND 
                //              TTQ.pairID = {0} AND TTQ.IsCancelled = 0 Group by TTQ.AskPrice,PairID order by TTQ.AskPrice", id);
                //}

                if (Price != -0)
                {
                    Result = _dbContext.BuyerSellerInfo.FromSql(
                                @"Select Top 100 TTQ.AskPrice As Price,sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty) as Amount,
                              Count(TTQ.AskPrice) As RecordCount,NEWID() As OrderId
                              from TradeTransactionQueue TTQ Where TTQ.Status = 4 and TTQ.TrnType = 5 AND 
                              TTQ.pairID = {0} AND TTQ.IsCancelled = 0 AND TTQ.AskPrice={1} Group by TTQ.AskPrice,PairID order by TTQ.AskPrice", id, Price);
                }
                else
                {
                    Result = _dbContext.BuyerSellerInfo.FromSql(
                                @"Select Top 100 TTQ.AskPrice As Price,sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty) as Amount,
                              Count(TTQ.AskPrice) As RecordCount,NEWID() As OrderId
                              from TradeTransactionQueue TTQ Where TTQ.Status = 4 and TTQ.TrnType = 5 AND 
                              TTQ.pairID = {0} AND TTQ.IsCancelled = 0 Group by TTQ.AskPrice,PairID order by TTQ.AskPrice", id);
                }
                return Result.ToList();
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name + " #PairId# : " + id + " #Price# : " + Price, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<GetGraphResponse> GetGraphData(long id, int IntervalTime, string IntervalData,DateTime Minute,int socket = 0)
        {
            try
            {
                string Query = "";
                IQueryable<GetGraphResponse> Result;
                if (socket == 0)
                {
                    Query = "Select DATEADD(#IntervalData#, DATEDIFF(#IntervalData#, 0, T.DataDate) / #IntervalTime# * #IntervalTime#, 0) As DataDate," +
                                   "MAX(T.High) As High, MIN(T.Low) As Low, SUM(T.Volume) As Volume," +
                                   "(Select T1.OpenVal From TradeData T1 Where T1.TranNo = MIN(T.TranNo)) As OpenVal," +
                                   "(Select T1.CloseVal From TradeData T1 Where T1.TranNo = MAX(T.TranNo)) As CloseVal From TradeData T" +
                                   " Where PairId = {0} And DATEADD(MINUTE, DATEDIFF(MINUTE, 0, T.DataDate) / 1 * 1, 0) > DATEADD(Day,-30,dbo.GetISTDate()) GROUP BY DATEADD(#IntervalData#, DATEDIFF(#IntervalData#, 0, T.DataDate) / #IntervalTime# * #IntervalTime#, 0)" +
                                   " Order By DATEADD(#IntervalData#, DATEDIFF(#IntervalData#, 0, T.DataDate) / #IntervalTime# * #IntervalTime#, 0) desc";

                    Query = Query.Replace("#IntervalData#", IntervalData).Replace("#IntervalTime#", IntervalTime.ToString());
                    Result = _dbContext.GetGraphResponse.FromSql(Query, id);
                }
                else
                {
                    Query = "Select DATEADD(#IntervalData#, DATEDIFF(#IntervalData#, 0, T.DataDate) / #IntervalTime# * #IntervalTime#, 0) As DataDate," +
                                   "MAX(T.High) As High, MIN(T.Low) As Low, SUM(T.Volume) As Volume," +
                                   "(Select T1.OpenVal From TradeData T1 Where T1.TranNo = MIN(T.TranNo)) As OpenVal," +
                                   "(Select T1.CloseVal From TradeData T1 Where T1.TranNo = MAX(T.TranNo)) As CloseVal From TradeData T" +
                                   " Where PairId = {0} And DataDate = {1} GROUP BY DATEADD(#IntervalData#, DATEDIFF(#IntervalData#, 0, T.DataDate) / #IntervalTime# * #IntervalTime#, 0)" +
                                   " Order By DATEADD(#IntervalData#, DATEDIFF(#IntervalData#, 0, T.DataDate) / #IntervalTime# * #IntervalTime#, 0) desc";

                    Query = Query.Replace("#IntervalData#", IntervalData).Replace("#IntervalTime#", IntervalTime.ToString());
                    string MinuteData =  Minute.ToString("yyyy-MM-dd HH:mm:00:000");

                    Result = _dbContext.GetGraphResponse.FromSql(Query,id,MinuteData);
                }
                return Result.ToList();
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public decimal LastPriceByPair(long PairId,ref short UpDownBit)
        {
            try
            {
                Decimal lastPrice=0;
                var pairStastics = _dbContext.TradePairStastics.Where(x => x.PairId == PairId).SingleOrDefault();
                UpDownBit = pairStastics.UpDownBit;
                lastPrice = pairStastics.LTP;
                return lastPrice;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public PairRatesResponse GetPairRates(long PairId)
        {
            try
            {
                decimal BuyPrice = 0;
                decimal SellPrice = 0;
                PairRatesResponse response = new PairRatesResponse();

                var BidPriceRes = _dbContext.TradeTransactionQueue.Where(e => e.TrnType == Convert.ToInt16(enTrnType.Buy_Trade) && e.Status == Convert.ToInt16(enTransactionStatus.Hold) && e.PairID == PairId).OrderByDescending(e => e.TrnNo).FirstOrDefault();
                if(BidPriceRes != null)
                {
                    BuyPrice = BidPriceRes.BidPrice;
                }
                else
                {
                    BuyPrice = 0;
                }

                var AskPriceRes = _dbContext.TradeTransactionQueue.Where(e => e.TrnType == Convert.ToInt16(enTrnType.Sell_Trade) && e.Status == Convert.ToInt16(enTransactionStatus.Hold) && e.PairID == PairId).OrderByDescending(e => e.TrnNo).FirstOrDefault();
                if (AskPriceRes != null)
                {
                    SellPrice = AskPriceRes.AskPrice;
                }
                else
                {
                    SellPrice = 0;
                }

                var PairResponse = _dbContext.TradePairDetail.Where(e => e.PairId == PairId).FirstOrDefault();
                
                if(PairResponse != null)
                {
                    if(BuyPrice == 0)
                    {
                        response.BuyPrice = PairResponse.BuyPrice;
                    }
                    else
                    {
                        response.BuyPrice = BuyPrice;
                    }
                    if (SellPrice == 0)
                    {
                        response.SellPrice = PairResponse.SellPrice;
                    }
                    else
                    {
                        response.SellPrice = SellPrice;
                    }
                    response.BuyMaxPrice = PairResponse.BuyMaxPrice;
                    response.BuyMinPrice = PairResponse.BuyMinPrice;
                    response.SellMaxPrice = PairResponse.SellMaxPrice;
                    response.SellMinPrice = PairResponse.SellMinPrice;
                }

                return response;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<TradePairTableResponse> GetTradePairAsset()
        {
            try
            {
                IQueryable<TradePairTableResponse> Result;
               
                Result = _dbContext.TradePairTableResponse.FromSql(
                            @"Select SM1.Id As BaseId,SM1.Name As BaseName,SM1.SMSCode As BaseCode,TPM.ID As PairId,TPM.PairName As Pairname,TPS.CurrentRate As Currentrate,TPD.BuyFees As BuyFees,TPD.SellFees As SellFees,
                                SM2.Name As ChildCurrency,SM2.SMSCode As Abbrevation,TPS.ChangePer24 As ChangePer,TPS.ChangeVol24 As Volume,TPS.High24Hr AS High24Hr,TPS.Low24Hr As Low24Hr,
                                TPS.HighWeek As HighWeek,TPS.LowWeek As LowWeek,TPS.High52Week AS High52Week,TPS.Low52Week As Low52Week,TPS.UpDownBit As UpDownBit from Market M 
                                Inner Join TradePairMaster TPM ON TPM.BaseCurrencyId = M.ServiceID
                                Inner Join TradePairDetail TPD ON TPD.PairId = TPM.Id
                                Inner Join TradePairStastics TPS ON TPS.PairId = TPM.Id
                                Inner Join ServiceMaster SM1 ON SM1.Id = TPM.BaseCurrencyId
                                Inner Join ServiceMaster SM2 ON SM2.Id = TPM.SecondaryCurrencyId Where TPM.Status = 1  Order By M.ID");
                
                return Result.ToList();
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public List<ServiceMasterResponse> GetAllServiceConfiguration()
        {

            try
            {
                IQueryable<ServiceMasterResponse> Result;

                Result = _dbContext.ServiceMasterResponse.FromSql(
                            @"Select SM.Id As ServiceId,SM.Name As ServiceName,SM.SMSCode,SM.ServiceType,SD.ServiceDetailJson,
                            SS.CirculatingSupply,SS.IssueDate,SS.IssuePrice,
                            ISNULL((Select STM.Status From ServiceTypeMapping STM Where STM.ServiceId = SM.Id and TrnType = 1),0) TransactionBit,
                            ISNULL((Select STM.Status From ServiceTypeMapping STM Where STM.ServiceId = SM.Id and TrnType = 6),0) WithdrawBit,
                            ISNULL((Select STM.Status From ServiceTypeMapping STM Where STM.ServiceId = SM.Id and TrnType = 8),0) DepositBit
                            From ServiceMaster SM
                            Inner Join ServiceDetail SD On SD.ServiceId = SM.Id
                            Inner Join ServiceStastics SS On SS.ServiceId = SM.Id Where SM.Status = 1");

                return Result.ToList();
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
    }
}
