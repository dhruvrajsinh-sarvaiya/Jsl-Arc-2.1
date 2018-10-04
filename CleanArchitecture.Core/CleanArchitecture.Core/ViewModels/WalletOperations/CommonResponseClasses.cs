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
        public int? chain { get; set; }
        public int? index { get; set; }
        public string redeemScript { get; set; }
    }
    public class History
    {
        public DateTime date { get; set; }
        public string action { get; set; }
    }

    public class Entry
    {
        public string address { get; set; }
        public int value { get; set; }
        public string wallet { get; set; }
    }

    public class Output
    {
        public string id { get; set; }
        public string address { get; set; }
        public int value { get; set; }
        public string valueString { get; set; }
        public string wallet { get; set; }
        public int chain { get; set; }
        public int index { get; set; }
    }

    public class Input
    {
        public string id { get; set; }
        public string address { get; set; }
        public int value { get; set; }
        public string valueString { get; set; }
        public string wallet { get; set; }
        public int chain { get; set; }
        public int index { get; set; }
    }
}
