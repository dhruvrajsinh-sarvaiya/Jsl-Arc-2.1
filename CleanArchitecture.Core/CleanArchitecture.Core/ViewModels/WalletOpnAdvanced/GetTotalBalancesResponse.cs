using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class GetTotalBalancesResponse : BizResponseClass
    {
        //public class GetTotalBalancesRootObject : BizResponseClass
        //{
            public int balance { get; set; }
            public string balanceString { get; set; }
            public int confirmedBalance { get; set; }
            public string confirmedBalanceString { get; set; }
            public int spendableBalance { get; set; }
            public string spendableBalanceString { get; set; }
      //  }
    }
}
