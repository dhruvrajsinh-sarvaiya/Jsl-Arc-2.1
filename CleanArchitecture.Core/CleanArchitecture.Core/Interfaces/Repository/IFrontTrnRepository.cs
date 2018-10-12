using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface IFrontTrnRepository
    {
        List<ActiveOrderDataResponse> GetActiveOrder(long MemberID);
        List<TradeHistoryResponce> GetTradeHistory(long id);
        long GetPairIdByName(string pair);
    }
}
