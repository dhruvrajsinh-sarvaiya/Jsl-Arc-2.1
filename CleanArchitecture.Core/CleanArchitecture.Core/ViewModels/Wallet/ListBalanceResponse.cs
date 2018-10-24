using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ListBalanceResponse : BizResponseClass
    {
        public List<BalanceResponse> Response { get; set; }
    }
}
