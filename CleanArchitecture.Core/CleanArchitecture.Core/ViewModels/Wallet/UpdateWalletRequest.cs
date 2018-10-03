using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class UpdateWalletRequest
    {
        public string label { get; set; }
        public bool disableTransactionNotifications { get; set; }
        public int approvalsRequired { get; set; }
        public object tokenFlushThresholds { get; set; }
    }
}
