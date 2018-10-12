using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface IFrontTrnRepository
    {
        List<ActiveOrderDataResponse> GetActiveOrder(long MemberID,long PairId);
        List<TradeHistoryResponce> GetTradeHistory(long MemberID,long PairId,int IsAll);
        long GetPairIdByName(string pair);
        List<RecentOrderRespose> GetRecentOrder(long PairId);
    }
}
