using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISignalRService
    {
        //Pair wise
        void BuyerBook(GetBuySellBook Data,string Pair);
        void SellerBook(GetBuySellBook Data, string Pair);
        void OrderHistory(GetTradeHistoryInfo Data, string Pair);
        void ChartData(List<GetGraphResponse> Data, string Pair);
        void MarketData(MarketCapData Data, string Pair);
        void LastPrice(LastPriceViewModel Data,string Pair);

        //user wise
        void OpenOrder(ActiveOrderInfo Data, string Token);
        void TradeHistory(GetTradeHistoryInfo Data, string Token);
        void RecentOrder(RecentOrderInfo Data, string Token);
        void BuyerSideWalletBal(WalletMasterResponse Data,string Wallet, string Token);
        void SellerSideWalletBal(WalletMasterResponse Data, string Wallet, string Token);
        void ActivityNotification(string Msg,string Token);

        //Base Market
        void PairData(VolumeDataRespose Data,string Base);
        void MarketTicker(VolumeDataRespose Data, string Base);

        //Event Call
        void OnStatusChange(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token, short OrderType, short IsPartial = 0);
        void OnVolumeChange(VolumeDataRespose volumeData, MarketCapData capData);
        void OnWalletBalChange(WalletMasterResponse Data, string WalletTypeName, string Token, short TokenType = 1);
        //void OnWalletBalChangeByUserID(WalletMasterResponse Data, string WalletTypeName,long UserID);
        void GetAndSendOpenOrderData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short OrderType, short IsPop = 0);
        GetTradeHistoryInfo GetAndSendTradeHistoryInfoData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short OrderType, short IsPop = 0);
        void GetAndSendRecentOrderData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short OrderType, short IsPop = 0);

        string GetTokenByUserID(string ID);
    }
}
