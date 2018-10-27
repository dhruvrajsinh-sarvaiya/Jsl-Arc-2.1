using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetPairRatesResponse : BizResponseClass
    {
        public PairRatesResponse response { get; set; }
    }
    public class PairRatesResponse
    {
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal BuyMaxPrice { get; set; }
        public decimal BuyMinPrice { get; set; }
        public decimal SellMaxPrice { get; set; }
        public decimal SellMinPrice { get; set; }
    }
}
