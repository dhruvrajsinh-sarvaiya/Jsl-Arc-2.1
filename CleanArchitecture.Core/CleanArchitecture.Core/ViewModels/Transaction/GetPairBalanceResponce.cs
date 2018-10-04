using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetPairBalanceResponce : BizResponseClass
    {
       public GetPairBalanceInfo response { get; set; }
    }
    public class GetPairBalanceInfo
    {
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal Available { get; set; }
        public long Pending { get; set; }
        public string CryptoAddress { get; set; }
        public bool Requested { get; set; }
        public object Uuid { get; set; }
    }
}
