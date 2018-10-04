using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetPairBalanceResponce
    {
        public string  Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal Available { get; set; }
        public long Pending { get; set; }
        public string CryptoAddress { get; set; }
        public bool Requested { get; set; }
        public object Uuid { get; set; }
    }
}
