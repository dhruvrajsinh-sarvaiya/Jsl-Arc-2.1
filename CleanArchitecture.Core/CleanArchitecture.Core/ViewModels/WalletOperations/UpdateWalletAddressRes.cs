using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class UpdateWalletAddressRes : BizResponseClass
    {
        [JsonProperty(PropertyName = "Address")]
        public string address { get; set; }
        [JsonProperty(PropertyName = "Chain")]
        public int chain { get; set; }
        [JsonProperty(PropertyName = "Index")]
        public int index { get; set; }
        [JsonProperty(PropertyName = "Coin")]
        public string coin { get; set; }
        [JsonProperty(PropertyName = "Label")]
        public string label { get; set; }
        [JsonProperty(PropertyName = "Wallet")]
        public string wallet { get; set; }
        [JsonProperty(PropertyName = "CoinSpecific")]
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
