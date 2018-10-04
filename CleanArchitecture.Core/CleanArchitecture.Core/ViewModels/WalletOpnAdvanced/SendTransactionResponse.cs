using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class SendTransactionResponse : BizResponseClass
    {
        public string txid { get; set; }
        public string txHex { get; set; }
        public string status { get; set; }

    }
}
