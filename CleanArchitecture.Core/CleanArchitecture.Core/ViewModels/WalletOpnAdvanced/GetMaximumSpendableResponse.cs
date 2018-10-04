using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public  class GetMaximumSpendableResponse : BizResponseClass
    {
        //public class GetMaximumSpendableRootObject : BizResponseClass
        //{
            public int maximumSpendable { get; set; }
            public string coin { get; set; }
       // }
    }
}
