using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations 
{
    public class ListWalletTransfersResponse : BizResponseClass
    {
        [JsonProperty(PropertyName = "Coin")]
        public string coin { get; set; }
        [JsonProperty(PropertyName = "Transfers")]
        public List<Transfer> transfers { get; set; }
        [JsonProperty(PropertyName = "Count")]
        public int count { get; set; }
    }
    //public class ListWalletTransfersRootObject
    //{

    //}
    //public class History
    //{
    //    public DateTime date { get; set; }
    //    public string action { get; set; }
    //}

    //public class Entry
    //{
    //    public string address { get; set; }
    //    public int value { get; set; }
    //    public string wallet { get; set; }
    //}

    //public class Output
    //{
    //    public string id { get; set; }
    //    public string address { get; set; }
    //    public int value { get; set; }
    //    public string valueString { get; set; }
    //    public string wallet { get; set; }
    //    public int chain { get; set; }
    //    public int index { get; set; }
    //}

    //public class Input
    //{
    //    public string id { get; set; }
    //    public string address { get; set; }
    //    public int value { get; set; }
    //    public string valueString { get; set; }
    //    public string wallet { get; set; }
    //    public int chain { get; set; }
    //    public int index { get; set; }
    //}

    public class Transfer
    {
        [JsonProperty(PropertyName = "Id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "Coin")]
        public string coin { get; set; }
        [JsonProperty(PropertyName = "Wallet")]
        public string wallet { get; set; }
        [JsonProperty(PropertyName = "TxId")]
        public string txid { get; set; }
        [JsonProperty(PropertyName = "NormalizedTxHash")]
        public string normalizedTxHash { get; set; }
        [JsonProperty(PropertyName = "Height")]
        public int height { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public DateTime date { get; set; }
        [JsonProperty(PropertyName = "Confirmations")]
        public int confirmations { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string type { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public int value { get; set; }
        [JsonProperty(PropertyName = "BitgoFee")]
        public int bitgoFee { get; set; }
        [JsonProperty(PropertyName = "USD")]
        public double usd { get; set; }
        [JsonProperty(PropertyName = "USDRate")]
        public double usdRate { get; set; }
        [JsonProperty(PropertyName = "State")]
        public string state { get; set; }
        [JsonProperty(PropertyName = "VSize")]
        public int vSize { get; set; }
        [JsonProperty(PropertyName = "NSegwitInputs")]
        public int nSegwitInputs { get; set; }
        [JsonProperty(PropertyName = "tags")]
        public List<string> tags { get; set; }
        [JsonProperty(PropertyName = "SequenceId")]
        public string sequenceId { get; set; }
        [JsonProperty(PropertyName = "History")]
        public List<History> history { get; set; }
        [JsonProperty(PropertyName = "Entries")]
        public List<Entry> entries { get; set; }
        [JsonProperty(PropertyName = "Outputs")]
        public List<Output> outputs { get; set; }
        [JsonProperty(PropertyName = "Inputs")]
        public List<Input> inputs { get; set; }
        [JsonProperty(PropertyName = "ConfirmedTime")]
        public DateTime confirmedTime { get; set; }
        [JsonProperty(PropertyName = "CreatedTime")]
        public DateTime createdTime { get; set; }
    }
}
