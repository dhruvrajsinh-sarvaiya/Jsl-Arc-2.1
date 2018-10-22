using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    //public class CommonResponseClasses
    //{

    //}
    public class CoinSpecific
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonProperty(PropertyName = "Chain")]
        public int? chain { get; set; }
        [JsonProperty(PropertyName = "Index")]
        public int? index { get; set; }
        [JsonProperty(PropertyName = "RedeemScript")]
        public string redeemScript { get; set; }
    }
    public class History
    {
        [JsonProperty(PropertyName = "Date")]
        public DateTime date { get; set; }
        [JsonProperty(PropertyName = "Action")]
        public string action { get; set; }
    }

    public class Entry
    {
        [JsonProperty(PropertyName = "Address")]
        public string address { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public int value { get; set; }
        [JsonProperty(PropertyName = "Wallet")]
        public string wallet { get; set; }
    }

    public class Output
    {
        [JsonProperty(PropertyName = "Id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string address { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public int value { get; set; }
        [JsonProperty(PropertyName = "ValueString")]
        public string valueString { get; set; }
        [JsonProperty(PropertyName = "Wallet")]
        public string wallet { get; set; }
        [JsonProperty(PropertyName = "Chain")]
        public int chain { get; set; }
        [JsonProperty(PropertyName = "Index")]
        public int index { get; set; }
    }

    public class Input
    {
        [JsonProperty(PropertyName = "Id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string address { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public int value { get; set; }
        [JsonProperty(PropertyName = "ValueString")]
        public string valueString { get; set; }
        [JsonProperty(PropertyName = "Wallet")]
        public string wallet { get; set; }
        [JsonProperty(PropertyName = "Chain")]
        public int chain { get; set; }
        [JsonProperty(PropertyName = "Index")]
        public int index { get; set; }
    }
}
