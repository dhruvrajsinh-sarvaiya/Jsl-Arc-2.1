using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Transaction.BackOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface IBackOfficeTrnRepository
    {
        List<GetTradingSummary> GetTradingSummary(long MemberID, string FromDate, string ToDate, long TrnNo, short status, string SMSCode, long PairID, short trade);
    }
}
