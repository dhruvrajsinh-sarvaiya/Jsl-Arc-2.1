using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class GetFirstPendingTransactionResponse
    {
        public class GetFirstPendingTransactionRootObject
        {
            public string walletId { get; set; }
            public string txid { get; set; }
            public string tx { get; set; }
            public string coin { get; set; }
            public long gasPrice { get; set; }
        }
    }
}
