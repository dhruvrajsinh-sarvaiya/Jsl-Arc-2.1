using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetRecentTradesResponse
    {
        public string pair_name { get; set; }

        public List<RecentTrades> recent_trades { get; set; }
    }
    public class RecentTrades
    {
        public long timestamp { get; set; }
        public long trans_id { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; }
    }
}
