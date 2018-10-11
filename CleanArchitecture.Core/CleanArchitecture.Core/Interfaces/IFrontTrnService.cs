using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IFrontTrnService
    {
        List<GetActiveOrderInfo> GetActiveOrder(long MemberID);
        List<BasePairResponse> GetTradePairAsset();
    }
}
