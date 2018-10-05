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

        public List<GetActiveOrderInfo> GetActiveOrder(long MemberID)
        {
            IQueryable<Object> Result = _dbContext.TransactionQueue.FromSql(
                   @"Select TQ.TrnNo,TQ.TrnDate,TTQ.TrnType,TTQ.Order_Currency,TTQ.Delivery_Currency,
                   CASE WHEN TTQ.BuyQty=0 THEN TTQ.SellQty WHEN TTQ.SellQty=0 THEN TTQ.BuyQty END as Amount,
                   CASE WHEN TTQ.BidPrice=0 THEN TTQ.AskPrice WHEN TTQ.AskPrice=0 THEN TTQ.BidPrice END as Price,
                   TTQ.IsCancelled from TransactionQueue TQ INNER JOIN TradeTransactionQueue TTQ ON TQ.TrnNo=TTQ.TrnNo 
                   Where TQ.Status=4 AND TQ.TrnType IN(4,5) And TQ.MemberID=@MemberID", MemberID);
            return null;
        }

    }
}
