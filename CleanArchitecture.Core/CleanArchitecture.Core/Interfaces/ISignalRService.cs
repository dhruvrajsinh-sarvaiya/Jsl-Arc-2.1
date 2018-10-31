using CleanArchitecture.Core.ApiModels;
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
        Task BuyerBook(GetBuySellBook Data,string Pair);
        Task SellerBook(GetBuySellBook Data, string Pair);
        Task TradingHistoryByPair(GetTradeHistoryInfo Data, string Pair);
        Task ChartData(List<GetGraphResponse> Data, string Pair);
        Task MarketData(MarketCapData Data, string Pair);
        Task LastPrice(LastPriceViewModel Data,string Pair);

        //user wise
        Task OpenOrder(ActiveOrderInfo Data, string Token);
        Task OrderHistory(GetTradeHistoryInfo Data, string Token);
        Task TradeHistoryByUser(GetTradeHistoryInfo Data, string Token);
        Task BuyerSideWalletBal(WalletMasterResponse Data,string Wallet, string Token);
        Task SellerSideWalletBal(WalletMasterResponse Data, string Wallet, string Token);
        Task ActivityNotification(string Msg,string Token);

        //Base Market
        Task PairData(VolumeDataRespose Data,string Base);
        Task MarketTicker(VolumeDataRespose Data, string Base);
    }
}
