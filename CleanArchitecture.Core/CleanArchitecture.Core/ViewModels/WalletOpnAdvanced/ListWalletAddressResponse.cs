using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ListWalletAddressResponse
    {
        public class CoinSpecific
        {
            public int chain { get; set; }
            public int index { get; set; }
            public string redeemScript { get; set; }
        }

        public class Address
        {
            public string address { get; set; }
            public string coin { get; set; }
            public string label { get; set; }
            public string wallet { get; set; }
            public CoinSpecific coinSpecific { get; set; }
        }

        public class ListWalletAddressRootObject
        {
            public int limit { get; set; }
            public string coin { get; set; }
            public List<Address> addresses { get; set; }
            public int count { get; set; }
            public int pendingAddressCount { get; set; }
            public int totalAddressCount { get; set; }
        }
    }
}
