using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetMarketSummaryResponse
    {
        public string pair_name { get; set; }

        public PairInformation pair_detail { get; set; }
    }
    public class PairInformation
    {
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal volume { get; set; }
        public decimal last_price { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }
        public int open_buy_order { get; set; }
        public int open_sell_order { get; set; }
        public decimal prev_day { get; set; }
    }
}
