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
        public decimal SellFees { get; set; }
        public decimal BuyFees { get; set; }
        public string ChildCurrency { get; set; }
        public string Abbrevation { get; set; }
        public decimal ChangePer { get; set; }
        public decimal High24Hr { get; set; }
        public decimal Low24Hr { get; set; }
        public decimal HighWeek { get; set; }
        public decimal LowWeek { get; set; }
        public decimal High52Week { get; set; }
        public decimal Low52Week { get; set; }
        public short UpDownBit { get; set; }

    }
}
