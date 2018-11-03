using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface IFrontTrnRepository
    {
        List<ActiveOrderDataResponse> GetActiveOrder(long MemberID, string sCondition, string FromDate, string TodDate, long PairId);
        List<TradeHistoryResponce> GetTradeHistory(long MemberID, string sCondition, string FromDate, string TodDate, int page, int IsAll, long TrnNo = 0);
        long GetPairIdByName(string pair);
        List<RecentOrderRespose> GetRecentOrder(long PairId, long MemberID);
        List<GetBuySellBook> GetBuyerBook(long id,decimal Price= -0);
        List<GetBuySellBook> GetSellerBook(long id, decimal Price = -0);
        List<GetGraphResponse> GetGraphData(long id, int IntervalTime, string IntervalData);
        decimal LastPriceByPair(long PairId, ref short UpDownBit);
        PairRatesResponse GetPairRates(long PairId);
        List<TradePairTableResponse> GetTradePairAsset();
    }
}
