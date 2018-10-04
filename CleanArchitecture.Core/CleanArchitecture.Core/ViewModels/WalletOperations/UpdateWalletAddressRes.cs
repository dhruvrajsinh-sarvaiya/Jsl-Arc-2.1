using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class UpdateWalletAddressRes : BizResponseClass
    {
        public string address { get; set; }
        public int chain { get; set; }
        public int index { get; set; }
        public string coin { get; set; }
        public string label { get; set; }
        public string wallet { get; set; }
        public CoinSpecific coinSpecific { get; set; }
    }
    //public class UpdateWalletAddressRootObject 
    //{

    //}
    //public class CoinSpecific
    //{
    //    public string redeemScript { get; set; }
    //}
}
