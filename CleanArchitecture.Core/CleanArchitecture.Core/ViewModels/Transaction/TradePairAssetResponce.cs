using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class TradePairAssetResponce
    {
        public string timezone { get; set; }
        public long serverTime { get; set; }
        public List<RateLimit> rateLimits { get; set; }
        public List<object> exchangeFilters { get; set; }
        public List<Symbol> symbols { get; set; }
        //public string altname { get; set; }
        //public string aclass_base { get; set; }
        //public string @base { get; set; }
        //public string aclass_quote { get; set; }
        //public string quote { get; set; }
        //public string lot { get; set; }
        //public int pair_decimals { get; set; }
        //public int lot_decimals { get; set; }
        //public int lot_multiplier { get; set; }
        //public List<object> leverage_buy { get; set; }
        //public List<object> leverage_sell { get; set; }
        //public List<List<double>> fees { get; set; }
        //public List<List<double>> fees_maker { get; set; }
        //public string fee_volume_currency { get; set; }
        //public int margin_call { get; set; }
        //public int margin_stop { get; set; }
    }
    

        public class RateLimit
        {
            public string rateLimitType { get; set; }
            public string interval { get; set; }
            public int limit { get; set; }
        }

        public class Filter
        {
            public string filterType { get; set; }
            public string minPrice { get; set; }
            public string maxPrice { get; set; }
            public string tickSize { get; set; }
            public string minQty { get; set; }
            public string maxQty { get; set; }
            public string stepSize { get; set; }
            public string minNotional { get; set; }
        }

        public class Symbol
        {
            public string symbol { get; set; }
            public string status { get; set; }
            public string baseAsset { get; set; }
            public int baseAssetPrecision { get; set; }
            public string quoteAsset { get; set; }
            public int quotePrecision { get; set; }
            public List<string> orderTypes { get; set; }
            public bool icebergAllowed { get; set; }
            public List<Filter> filters { get; set; }
        }

        public class RootObject
        {
           
        }
}
