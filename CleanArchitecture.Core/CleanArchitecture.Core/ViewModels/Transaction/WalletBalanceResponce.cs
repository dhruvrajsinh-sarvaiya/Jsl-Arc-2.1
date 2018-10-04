using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class WalletBalanceResponce : BizResponseClass
    {
       public List<WalletBalanceInfo> response { get; set; }
    }
    public class WalletBalanceInfo
    {
        public string type { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string available { get; set; }
    }
}
