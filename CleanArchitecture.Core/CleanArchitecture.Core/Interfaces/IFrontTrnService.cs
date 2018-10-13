using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IFrontTrnService
    {
        List<ActiveOrderInfo> GetActiveOrder(long MemberID, long PairId);
        List<BasePairResponse> GetTradePairAsset();
        List<VolumeDataRespose> GetVolumeData();
        List<GetTradeHistoryInfo> GetTradeHistory(long MemberID, long PairId,short TrnType,short Status,int PageSize,int MarketType,DateTime  fromDate, DateTime Todate, int IsAll);
        long GetPairIdByName(string pair);
        bool IsValidPairName(string Pair);
        List<RecentOrderInfo> GetRecentOrder(long PairId);
        List<GetBuySellBook> GetBuyerBook(long id);
        List<GetBuySellBook> GetSellerBook(long id);
        bool IsValidDateFormate(string date);
        Int16  IsValidTradeType(string Type);
        Int16 IsValidMarketType(string type);
        Int16 IsValidStatus(string status);

    }
}
