using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
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
        void TradingHistoryByPair(GetTradeHistoryInfo Data, string Pair);
        void ChartData(List<GetGraphResponse> Data, string Pair);
        void MarketData(MarketCapData Data, string Pair);
        void LastPrice(LastPriceViewModel Data,string Pair);

        //user wise
        void OpenOrder(ActiveOrderInfo Data, string Token);
        void OrderHistory(GetTradeHistoryInfo Data, string Token);
        void TradeHistoryByUser(GetTradeHistoryInfo Data, string Token);
        void BuyerSideWalletBal(WalletMasterResponse Data,string Wallet, string Token);
        void SellerSideWalletBal(WalletMasterResponse Data, string Wallet, string Token);
        void ActivityNotification(string Msg,string Token);

        //Base Market
        void PairData(VolumeDataRespose Data,string Base);
        void MarketTicker(VolumeDataRespose Data, string Base);



        //Event Call
        void OnStatusChange(short Status, TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, string Token);
        void OnVolumeChange(VolumeDataRespose volumeData, MarketCapData capData);
        void OnWalletBalChange(WalletMasterResponse Data, string WalletTypeName, string Token);
        void GetAndSendOpenOrderData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short IsPop = 0);
        GetTradeHistoryInfo GetAndSendGetTradeHistoryInfoData(TransactionQueue Newtransaction, TradeTransactionQueue NewTradeTransaction, short IsPop = 0);
    }
}
