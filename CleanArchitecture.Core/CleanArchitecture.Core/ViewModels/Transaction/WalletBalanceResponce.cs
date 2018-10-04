using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class WalletBalanceResponce
    {
        public string type { get; set; }
        public string  currency { get; set; }
        public decimal  amount { get; set; }
        public string available { get; set; }
    }
}
