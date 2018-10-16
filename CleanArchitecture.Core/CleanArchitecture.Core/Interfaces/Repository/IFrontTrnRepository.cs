using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface IFrontTrnRepository
    {
        List<ActiveOrderDataResponse> GetActiveOrder(long MemberID, string sCondition, string FromDate, string TodDate, long PairId);
        List<TradeHistoryResponce> GetTradeHistory(long MemberID, string sCondition, string FromDate, string TodDate, int page, int IsAll);
        long GetPairIdByName(string pair);
        List<RecentOrderRespose> GetRecentOrder(long PairId);
        List<GetBuySellBook> GetBuyerBook(long id);
        List<GetBuySellBook> GetSellerBook(long id);
    }
}
