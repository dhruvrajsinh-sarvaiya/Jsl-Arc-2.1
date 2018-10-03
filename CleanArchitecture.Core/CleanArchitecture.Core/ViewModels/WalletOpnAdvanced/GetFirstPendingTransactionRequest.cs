using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class GetFirstPendingTransactionRequest
    {
        public string walletId { get; set; }
        public string enterpriseId { get; set; }
    }
}
