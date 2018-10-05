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

    }
}
