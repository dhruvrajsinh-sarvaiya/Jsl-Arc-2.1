using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Repository;
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
                Qry = @"Select TQ.Id,TQ.TrnDate,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,TTQ.Order_Currency,TTQ.Delivery_Currency, " +
                   "CASE WHEN TTQ.BuyQty = 0 THEN TTQ.SellQty WHEN TTQ.SellQty = 0 THEN TTQ.BuyQty END as Amount, "+
                   "CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price, "+
                   "TTQ.IsCancelled from TransactionQueue TQ INNER JOIN TradeTransactionQueue TTQ ON TQ.Id = TTQ.TrnNo "+
                   "Where "+ sCondition  +" AND TQ.Status ={2} And TQ.MemberID ={0} AND TTQ.PairID ={1} ";

                


                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    fDate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    tDate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    Result = _dbContext.ActiveOrderDataResponse.FromSql(Qry
                   , MemberID, PairId, Convert.ToInt16(enTransactionStatus.Hold),fDate ,tDate);
                }
                else
                    Result = _dbContext.ActiveOrderDataResponse.FromSql(Qry,MemberID, PairId, Convert.ToInt16(enTransactionStatus.Hold));

                return Result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
            
        }

        public List<RecentOrderRespose> GetRecentOrder(long PairId)
        {
            try
            {
                IQueryable<RecentOrderRespose> Result = _dbContext.RecentOrderRespose.FromSql(
                   @"Select top 100 TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,
                        CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,
                        CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Qty,
                        TTQ.SettledDate as DateTime, TTQ.Status from TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id  = TTQ.TrnNo
                        WHERE TTQ.PairID ={0} AND 
                        TQ.TrnDate > DATEADD(HOUR , -24,getdate()) Order By TTQ.SettledDate DESC", PairId, Convert.ToInt16(enTransactionStatus.Hold));

                return Result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public List<TradeHistoryResponce> GetTradeHistory(long MemberID, string sCondition, string FromDate, string ToDate, int page, int IsAll)
        {
            IQueryable<TradeHistoryResponce> Result = null;
            string qry = "";
            DateTime fDate, tDate;
            
            try
            {
                if (IsAll == 0)
                {
                    long PairId = MemberID;
                    Result = _dbContext.TradeHistoryInfo.FromSql(
                             @"Select top 100 TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,
                                CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,
                                CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount,
                                 TTQ.SettledDate as DateTime, TTQ.Status, TTQ.StatusMsg as StatusText, TTQ.PairID, TQ.ChargeRs,TTQ.IsCancelled from
                                 TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id  = TTQ.TrnNo
                                 WHERE TTQ.Status in ({0}) and (TTQ.SettledSellQty>0 or TTQ.SettledBuyQty>0) and TTQ.PairID = {1} 
                                 Order By TTQ.SettledDate DESC", Convert.ToInt16(enTransactionStatus.Success), PairId);
                }
                else
                {
                    if (IsAll == 1)
                    {
                        qry = @"Select  TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type, " +
                                   "CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price, " +
                                   "CASE WHEN TTQ.TrnType = 4  THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount," +
                                   "TTQ.TrnDate as DateTime,TTQ.Status,TTQ.StatusMsg as StatusText,TTQ.PairID,TQ.ChargeRs,TTQ.IsCancelled " +
                                   "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id = TTQ.TrnNo " +
                                   "WHERE " + sCondition + " AND TTQ.IsCancelled=0 AND TTQ.MemberID=" + MemberID + " AND TTQ.Status=" + Convert.ToInt16(enTransactionStatus.Success) +" Order By TrnNo desc";

                    }
                    else if (IsAll == 2)
                    {
                        qry = @"Select  TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type," +
                               " CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price," +
                                "CASE WHEN TTQ.TrnType = 4  THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END as Amount,TTQ.TrnDate as DateTime," +
                                "TTQ.Status,TTQ.StatusMsg as StatusText,TTQ.PairID,TQ.ChargeRs,TTQ.IsCancelled " +
                                "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id=TTQ.TrnNo  " +
                                "WHERE " + sCondition + "AND TTQ.Status=" + Convert.ToInt16(enTransactionStatus.SystemFail) + " AND TTQ.MemberID=" + MemberID + " Order By TrnNo desc";

                    }
                    else if (IsAll == 9)
                    {
                        qry = @"Select TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type," +
                               " CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price," +
                                "CASE WHEN TTQ.TrnType = 4  THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END as Amount,TTQ.TrnDate as DateTime," +
                                "TTQ.Status,TTQ.StatusMsg as StatusText,TTQ.PairID,TQ.ChargeRs,TTQ.IsCancelled " +
                                "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id=TTQ.TrnNo  " +
                                "WHERE " + sCondition + "AND TTQ.Status in(" + Convert.ToInt16(enTransactionStatus.Success) + "," + Convert.ToInt16(enTransactionStatus.OperatorFail) +
                                ") AND TTQ.MemberID=" + MemberID + " AND TTQ.IsCancelled=1  Order By TrnNo desc";

                    }
                    else
                    {
                        qry = @"Select TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice " +
                                "WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount,TTQ.TrnDate as DateTime,TTQ.Status,TTQ.StatusMsg as StatusText," +
                                "TTQ.PairID,TQ.ChargeRs,CAST(0 as smallint) As IsCancelled from  TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id  = TTQ.TrnNo " +
                                "WHERE " + sCondition + " AND TTQ.MemberID=" + MemberID + "  and TTQ.Status =" + Convert.ToInt16(enTransactionStatus.Success) +
                                "UNION  ALL Select TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,CASE WHEN TTQ.BidPrice=0 THEN " +
                                "TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,CASE WHEN TTQ.TrnType = 4 THEN TCQ.PendingBuyQty else TCQ.DeliverQty END as Amount,TTQ.TrnDate as DateTime,TTQ.Status, " +
                                "TCQ.StatusMsg as StatusText,TTQ.PairID,TQ.ChargeRs,CAST(1 as smallint) as 'IsCancelled' from TradeCancelQueue TCQ INNER JOIN TradeTransactionQueue TTQ ON TTQ.TrnNo = TCQ.TrnNo " +
                                "INNER JOIN TransactionQueue TQ ON TQ.Id  = TTQ.TrnNo WHERE " + sCondition + " AND TTQ.MemberID=" + MemberID + " and TCQ.Status=" + Convert.ToInt16(enTransactionStatus.Success) +
                                "UNION ALL Select TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price, " +
                                "CASE WHEN TTQ.TrnType = 4  THEN TTQ.BuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SellQty END as Amount,TTQ.TrnDate as DateTime,TTQ.Status,TTQ.StatusMsg as StatusText,TTQ.PairID,TQ.ChargeRs,TTQ.IsCancelled " +
                                "from TradeTransactionQueue TTQ INNER JOIN TransactionQueue TQ ON TQ.Id=TTQ.TrnNo WHERE " + sCondition + " AND TTQ.MemberID=" + MemberID + " AND TTQ.Status=" + Convert.ToInt16(enTransactionStatus.SystemFail) +
                                "Order By TTQ.TrnNo Desc";

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
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }

        public List<GetBuySellBook> GetBuyerBook(long id)
        {
            IQueryable<GetBuySellBook> Result = _dbContext.BuyerSellerInfo.FromSql(
                            @"Select Top 100 TTQ.BidPrice As Price, Sum(TTQ.DeliveryTotalQty) - Sum(TTQ.SettledBuyQty) As Amount
                            From TradeTransactionQueue TTQ  where TTQ.Status = 4 and TTQ.TrnType = 4 AND TTQ.pairID = {0} AND TTQ.IsCancelled = 0 Group By TTQ.BidPrice Order By TTQ.BidPrice", id);

            return Result.ToList();
        }

        public List<GetBuySellBook> GetSellerBook(long id)
        {
            IQueryable<GetBuySellBook> Result = _dbContext.BuyerSellerInfo.FromSql(
                            @"Select Top 100 TTQ.AskPrice As Price,sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty) as Amount from
                              TradeTransactionQueue TTQ Where TTQ.Status = 4 and TTQ.TrnType = 5 AND 
                              TTQ.pairID = {0} AND TTQ.IsCancelled = 0 Group by TTQ.AskPrice order by TTQ.AskPrice ", id);

            return Result.ToList();
        }

        public List<GetGraphResponse> GetGraphData(long id)
        {
            IQueryable<GetGraphResponse> Result = _dbContext.GetGraphResponse.FromSql(
                            @"Select CONVERT(BIGINT,DATEDIFF(ss,'01-01-1970 00:00:00',DataDate)) As DataDate,Volume,High,Low,TodayClose,TodayOpen 
                              From TradeGraphDetail Where PairID={0}
                              AND DataDate>DATEADD(DAY, -366, dbo.GetISTDate()) AND DataDate<=DATEADD(DAY, -1, dbo.GetISTDate())", id);

            return Result.ToList();
        }
    }
}
