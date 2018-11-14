using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IFrontTrnService
    {
        List<ActiveOrderInfo> GetActiveOrder(long MemberID, string sCondition, string FromDate, string TodDate, long PairId,int Page);
        List<BasePairResponse> GetTradePairAsset();
        List<VolumeDataRespose> GetVolumeData(long BasePairId);
        List<GetTradeHistoryInfo> GetTradeHistory(long MemberID, string sCondition, string FromDate, string TodDate, int page, int IsAll);
        long GetPairIdByName(string pair);
        bool IsValidPairName(string Pair);
        List<RecentOrderInfo> GetRecentOrder(long PairId,long MemberID);
        List<GetBuySellBook> GetBuyerBook(long id);
        List<GetBuySellBook> GetSellerBook(long id);
        bool IsValidDateFormate(string date);
        Int16  IsValidTradeType(string Type);
        Int16 IsValidMarketType(string type);
        Int16 IsValidStatus(string status);
        long GetBasePairIdByName(string BasePair);
        GetTradePairByName GetTradePairByName(long id);
        List<GetGraphDetailInfo> GetGraphDetail(long PairId,int IntervalTime,string IntervalData);
        MarketCapData GetMarketCap(long PairId);
        VolumeDataRespose GetVolumeDataByPair(long PairId);
        bool addSetteledTradeTransaction(SettledTradeTransactionQueue queueData);
        PairRatesResponse GetPairRates(long PairId);
        int AddToFavouritePair(long PairId, long UserId);
        int RemoveFromFavouritePair(long PairId, long UserId);
        List<FavouritePairInfo> GetFavouritePair(long UserId);
        void GetPairAdditionalVal(long PairId, decimal CurrentRate,long TrnNo, decimal Quantity, DateTime TranDate);
        void GetIntervalTimeValue(string Interval, ref int IntervalTime, ref string IntervalData);
    }
}
