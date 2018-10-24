using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ListBalanceResponse 
    {
        public List<BalanceResponse> Response { get; set; }
        public decimal? TotalBalance { get; set; }
        public BizResponseClass BizResponseObj { get; set; }
    }
}
