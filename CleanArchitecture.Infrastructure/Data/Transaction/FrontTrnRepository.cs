using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Transaction
{
    public class FrontTrnRepository : IFrontTrnRepository
    {
        private readonly CleanArchitectureContext _dbContext;

        public FrontTrnRepository(CleanArchitectureContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ActiveOrderDataResponse> GetActiveOrder(long MemberID)
        {
            IQueryable<ActiveOrderDataResponse> Result = _dbContext.ActiveOrderDataResponse.FromSql(
                   @"Select TQ.Id,TQ.TrnDate,TTQ.TrnType As Type,TTQ.Order_Currency,TTQ.Delivery_Currency,
                   CASE WHEN TTQ.BuyQty=0 THEN TTQ.SellQty WHEN TTQ.SellQty=0 THEN TTQ.BuyQty END as Amount,
                   CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price,
                   TTQ.IsCancelled from TransactionQueue TQ INNER JOIN TradeTransactionQueue TTQ ON TQ.Id=TTQ.TrnNo 
                   Where TQ.Status=4 AND TQ.TrnType IN(4,5) And TQ.MemberID={0}", MemberID);

            return Result.ToList();
        }

        public long GetPairIdByName(string pair)
        {
            
            var model = _dbContext.TradePairMaster.Where(p => p.PairName == pair).FirstOrDefault();
            if (model == null)
                return 0;

            return model.Id;
        }

        public List<TradeHistoryResponce> GetTradeHistory(long id)
        {
            IQueryable<TradeHistoryResponce> Result = _dbContext.TradeHistoryInfo.FromSql(
                             @"Select top 100 TTQ.TrnNo,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type,
                                CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price,
                                CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount,
                                 TTQ.SettledDate as DateTime, TTQ.Status, TTQ.StatusMsg as StatusText, TTQ.PairID, TQ.ChargeRs from
                                 TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id  = TTQ.TrnNo
                                 WHERE TTQ.PairID = {0} AND TTQ.Status in ({1}, {2})   and  (TTQ.SettledSellQty>0 or TTQ.SettledBuyQty>0) 
                                 Order By TTQ.SettledDate DESC", id, Convert.ToInt16(enTransactionStatus.Success), Convert.ToInt16(enTransactionStatus.Hold));

            return Result.ToList();
        }
    }
}
