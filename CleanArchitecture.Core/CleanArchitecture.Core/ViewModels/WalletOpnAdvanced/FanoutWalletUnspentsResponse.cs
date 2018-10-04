using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class FanoutWalletUnspentsResponse : BizResponseClass
    {
        //public class FanoutWalletUnspentsRootObject : BizResponseClass
        //{
            public string txid { get; set; }
            public string tx { get; set; }
            public string status { get; set; }
        //}
    }
}
