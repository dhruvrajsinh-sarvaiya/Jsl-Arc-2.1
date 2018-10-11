using CleanArchitecture.Core.ApiModels;
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
        public string Pairname { get; set; }
        public decimal Currentrate { get; set; }
        public decimal Volume { get; set; }
        public decimal Fee { get; set; }
    }
}
