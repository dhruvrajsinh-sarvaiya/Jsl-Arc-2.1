using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public List<ActiveOrderDataResponse> GetActiveOrder(long MemberID,long PairId)
        {
            try
            {
                IQueryable<ActiveOrderDataResponse> Result = _dbContext.ActiveOrderDataResponse.FromSql(
                   @"Select TQ.Id,TQ.TrnDate,TTQ.TrnType As Type,TTQ.Order_Currency,TTQ.Delivery_Currency,
                   CASE WHEN TTQ.BuyQty=0 THEN TTQ.SellQty WHEN TTQ.SellQty=0 THEN TTQ.BuyQty END as Amount,
                   CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price,
                   TTQ.IsCancelled from TransactionQueue TQ INNER JOIN TradeTransactionQueue TTQ ON TQ.Id=TTQ.TrnNo 
                   Where TQ.Status=4 AND TQ.TrnType IN({2}) And TQ.MemberID={0} AND TTQ.PairID={1}", MemberID, PairId, Convert.ToInt16(enTransactionStatus.Hold));

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
                var model = _dbContext.TradePairMaster.Where(p => p.PairName == pair).FirstOrDefault();
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
                        WHERE TTQ.PairID ={0} AND TTQ.Status in (1, 4)  and  (TTQ.SettledSellQty>0 or TTQ.SettledBuyQty>0) and 
                        TQ.TrnDate > DATEADD(HOUR , -24,getdate()) Order By TTQ.SettledDate DESC", PairId, Convert.ToInt16(enTransactionStatus.Hold));

                return Result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public List<TradeHistoryResponce> GetTradeHistory(long MemberID, long PairId, int IsAll)
        {
            IQueryable<TradeHistoryResponce> Result;
            try
            {
                if(IsAll==0)
                {

                    Result = _dbContext.TradeHistoryInfo.FromSql(
                             @"Select top 100 TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,
                                CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,
                                CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount,
                                 TTQ.SettledDate as DateTime, TTQ.Status, TTQ.StatusMsg as StatusText, TTQ.PairID, TQ.ChargeRs from
                                 TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id  = TTQ.TrnNo
                                 WHERE TTQ.Status in ({0},{1}) and (TTQ.SettledSellQty>0 or TTQ.SettledBuyQty>0) 
                                 Order By TTQ.SettledDate DESC", Convert.ToInt16(enTransactionStatus.Success), Convert.ToInt16(enTransactionStatus.Hold));
                }
                else
                {
                    Result = _dbContext.TradeHistoryInfo.FromSql(
                             @"Select top 100 TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,
                                CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,
                                CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount,
                                 TTQ.SettledDate as DateTime, TTQ.Status, TTQ.StatusMsg as StatusText, TTQ.PairID, TQ.ChargeRs from
                                 TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id  = TTQ.TrnNo
                                 WHERE TTQ.PairID = {0} AND TTQ.Status in ({1},{2}) and  (TTQ.SettledSellQty>0 or TTQ.SettledBuyQty>0) And TQ.MemberID={3}
                                 Order By TTQ.SettledDate DESC", PairId, Convert.ToInt16(enTransactionStatus.Success), Convert.ToInt16(enTransactionStatus.Hold), MemberID);
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
                            @"Select Top 100 TTQ.BidPrice As Price, Sum(TTQ.DeliveryTotalQty) - Sum(TTQ.SettledBuyQty) As Amount, TTQ.BidPrice * (Sum(TTQ.DeliveryTotalQty) - sum(TTQ.SettledBuyQty)) as Count
                            From TradeTransactionQueue TTQ  where TTQ.Status = 4 and TTQ.TrnType = 4 AND TTQ.pairID = {0} AND TTQ.IsCancelled = 0 Group By TTQ.BidPrice Order By TTQ.BidPrice", id);

            return Result.ToList();
        }

        public List<GetBuySellBook> GetSellerBook(long id)
        {
            IQueryable<GetBuySellBook> Result = _dbContext.BuyerSellerInfo.FromSql(
                            @"Select Top 100 TTQ.AskPrice As Price,sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty) as Amount,TTQ.AskPrice* (sum(TTQ.OrderTotalQty) - Sum(TTQ.SettledSellQty)) as Count from
                              TradeTransactionQueue TTQ Where TTQ.Status = 4 and TTQ.TrnType = 5 AND 
                              TTQ.pairID = {0} AND TTQ.IsCancelled = 0 Group by TTQ.AskPrice order by TTQ.AskPrice ", id);

            return Result.ToList();
        }
    }
}
