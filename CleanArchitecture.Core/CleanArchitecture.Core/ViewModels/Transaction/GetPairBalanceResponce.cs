using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetPairBalanceResponce
    {
        public string Currency { get; set; }
        public double Balance { get; set; }
        public double Available { get; set; }
        public int Pending { get; set; }
        public string CryptoAddress { get; set; }
        public bool Requested { get; set; }
        public object Uuid { get; set; }
    }
}
