using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class SignTransactionResponse
    {
        public class SignTransactionResRootObject : BizResponseClass
        {
            public string txHex { get; set; }
        }
    }
}
