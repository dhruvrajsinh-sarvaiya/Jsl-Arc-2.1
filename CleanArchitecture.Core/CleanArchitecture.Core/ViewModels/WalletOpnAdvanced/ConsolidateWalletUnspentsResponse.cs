using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ConsolidateWalletUnspentsResponse : BizResponseClass
    {
        //public class ConsolidateWalletUnspentsRootObject : BizResponseClass
        //{
            public string txid { get; set; }
            public string tx { get; set; }
            public string status { get; set; }
       // }
    }
}
