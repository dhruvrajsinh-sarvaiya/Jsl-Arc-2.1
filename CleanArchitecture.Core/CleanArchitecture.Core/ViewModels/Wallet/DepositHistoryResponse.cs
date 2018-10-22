using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class DepositHistoryResponse : BizResponseClass
    {
       public List<HistoryObject> Histories { get; set; }
    }
}
