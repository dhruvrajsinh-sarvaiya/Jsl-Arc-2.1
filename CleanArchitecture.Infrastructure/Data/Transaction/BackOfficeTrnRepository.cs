using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction.BackOffice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string Qry = "";
            string sCondition = "";
            DateTime fDate, tDate;
            try
            {
                if (PairID != 999)
                    sCondition += " AND TTQ.PairId=" + PairID;

                if(MemberID > 0)
                    sCondition += " AND MM.Id=" + MemberID;
                if(TrnNo > 0)
                    sCondition += " AND TTQ.TrnNo=" + TrnNo;

                if(!String.IsNullOrEmpty(FromDate))
                    sCondition += " AND TTQ.TrnDate Between {0} And {1} And TQ.Status>0 ";

                if(!string.IsNullOrEmpty(SMSCode))
                    sCondition += " AND TQ.SMSCode=" + SMSCode;

                if (trade!=999)
                    sCondition += " AND TTQ.TrnType=" + trade;

                if (status == 91)
                    sCondition += " And TQ.Status = 1 ";
                else if (status == 95)
                    sCondition += " And TQ.Status = 4 ";
                else if (status == 92)
                    sCondition += " And TQ.Status = 1 AND IsCancelled=1 ";
                else if (status == 93)
                    sCondition += " And TQ.Status = 1 AND IsCancelled=0 ";
                else if (status == 94)
                    sCondition += " And TQ.Status = 3 ";

                Qry = "Select TTQ.TrnNo,MM.Id AS MemberID,CASE WHEN TTQ.TrnType=4 THEN 'BUY' WHEN TTQ.TrnType=5 THEN 'SELL' END as Type, " +
                        " CASE WHEN TTQ.BidPrice = 0 THEN TTQ.AskPrice WHEN TTQ.AskPrice = 0 THEN TTQ.BidPrice END as Price, " +
                        " CASE WHEN TTQ.TrnType = 4 THEN TTQ.SettledBuyQty WHEN TTQ.TrnType = 5 THEN TTQ.SettledSellQty END as Amount, " +
                        " TTQ.SettledDate as DateTime, TTQ.StatusMsg as StatusText, TTQ.PairID,TTQ.PairName, TQ.ChargeRs, " +
                        " ISNULL(PreBal, 0) as PreBal,ISNULL(PostBal, 0) as PostBal from " +
                        " TradeTransactionQueue TTQ inner join TransactionQueue TQ on TQ.Id = TTQ.TrnNo " +
                        " Join BizUser MM On MM.Id = TTQ.MemberID INNER JOIN WalletLedgers WL ON WL.Id = TTQ.MemberID " +
                        " WHERE (TTQ.SettledSellQty > 0 or TTQ.SettledBuyQty > 0) "+ sCondition +" Order By TTQ.TrnNo Desc";

                IQueryable<GetTradingSummary> Result;
                if (!String.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    tDate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    Result = _dbContext.GetTradingSummary.FromSql(Qry,fDate ,tDate);
                }   
                else
                    Result = _dbContext.GetTradingSummary.FromSql(Qry);
                return Result.ToList();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
    }
}
