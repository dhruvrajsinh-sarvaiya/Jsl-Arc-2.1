using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class BalanceResponseWithLimit
    {
        public List<BalanceResponse> Response { get; set; }
        public decimal? TotalBalance { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal UsedLimit { get; set; }//amount of TQ   
        public BizResponseClass BizResponseObj { get; set; }
    }
    //public class BalanceResponseWithLimit
    //{
    //    public List<BalanceResponse> BalanceResponses { get; set; }
         
    //}
}
