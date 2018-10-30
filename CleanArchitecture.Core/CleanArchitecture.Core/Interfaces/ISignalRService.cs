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
        void BuyerBook(GetBuySellBook Data,string Pair);
        void SellerBook(GetBuySellBook Data, string Pair);
        void TradingHistoryByPair(GetTradeHistoryInfo Data, string Pair);
        void ChartData(List<GetGraphResponse> Data, string Pair);
        void MarketData(MarketCapData Data, string Pair);
        void LastPrice(Decimal Price,string Pair);

        //user wise
        void OpenOrder(ActiveOrderInfo Data, string Token);
        void OrderHistory(GetTradeHistoryInfo Data, string Token);
        void TradeHistoryByUser(GetTradeHistoryInfo Data, string Token);
        void BuyerSideWalletBal(WalletMasterResponse Data,string Wallet, string Token);
        void SellerSideWalletBal(WalletMasterResponse Data, string Wallet, string Token);

        //Base Market
        void PairData(VolumeDataRespose Data,string Base);
        void MarketTicker(VolumeDataRespose Data, string Base);
    }
}
