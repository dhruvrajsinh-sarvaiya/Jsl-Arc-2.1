using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction.BackOffice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Transaction
{
    public class BackOfficeTrnRepository : IBackOfficeTrnRepository
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly ILogger<BackOfficeTrnRepository> _logger;

        public BackOfficeTrnRepository(CleanArchitectureContext dbContext, ILogger<BackOfficeTrnRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<GetTradingSummary> GetTradingSummary(long MemberID, string FromDate, string ToDate, long TrnNo, short status, string SMSCode, long PairID, short trade)
        {
            try
            {
                IQueryable<GetTradingSummary> Result = _dbContext.GetTradingSummary.FromSql(
                    @"Select TTQ.TrnNo,MM.Id AS MemberID,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type, 
                                    CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price, 
                                    CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount, 
                                    TTQ.SettledDate as DateTime, TTQ.StatusMsg as StatusText, TTQ.PairID,TTQ.PairName, TQ.ChargeRs,
                                    ISNULL(PreBal, 0) as PreBal,ISNULL(PostBal, 0) as PostBal from
                                    TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id = TTQ.TrnNo
                                    inner Join BizUser MM On MM.Id = TTQ.MemberID INNER JOIN WalletLedgers WL ON WL.Id = TTQ.MemberID
                                    WHERE(TTQ.SettledSellQty > 0 or TTQ.SettledBuyQty > 0) Order By TTQ.TrnNo Desc");
                return Result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
