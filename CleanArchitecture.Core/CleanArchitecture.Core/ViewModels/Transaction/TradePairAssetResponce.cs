using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class TradePairAssetResponce : BizResponseClass
    {
        public List<BasePairResponse> response { get; set; } 
    }
    public class BasePairResponse
    {
        public long BaseCurrencyId { get; set; }
        public string BaseCurrencyName { get; set; }
        public string Abbrevation { get; set; }
        public List<TradePairRespose> PairList { get; set; }
    }
    public class TradePairRespose
    {
        public long PairId { get; set; }
        [JsonProperty(PropertyName = "PairName")]
        public string Pairname { get; set; }
        [JsonProperty(PropertyName = "CurrentRate")]
        public decimal Currentrate { get; set; }
        public decimal Volume { get; set; }
        public decimal Fee { get; set; }
        public string ChildCurrency { get; set; }
        public string Abbrevation { get; set; }
        public decimal ChangePer { get; set; }
    }
}
