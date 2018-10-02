using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class SendTranToManyRes
    {
        public class SendTranToManyRootObject : BizResponseClass
        {
            public string txid { get; set; }
            public string status { get; set; }
        }
    }
}
