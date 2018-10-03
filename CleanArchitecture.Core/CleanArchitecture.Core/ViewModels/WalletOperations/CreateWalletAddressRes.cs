using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class CreateWalletAddressRes
    {
        public class CoinSpecific
        {
            public int chain { get; set; }
            public int index { get; set; }
            public string redeemScript { get; set; }
        }

        public class CreateWalletAddressRootObject: BizResponseClass
        {
            public string address { get; set; }
            public string coin { get; set; }
            public string label { get; set; }
            public string wallet { get; set; }
            public CoinSpecific coinSpecific { get; set; }
        }
    }
}
