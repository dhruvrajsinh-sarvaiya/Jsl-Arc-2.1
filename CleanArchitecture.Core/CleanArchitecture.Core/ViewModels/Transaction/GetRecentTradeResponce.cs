using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetRecentTradeResponce : BizResponseClass
    {
        public List<RecentTrade> RecentTrades { get; set; }
    }
    public class RecentTrade 
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public long time { get; set; }
        public bool isBuyerMaker { get; set; }
        public bool isBestMatch { get; set; }
        
    }
}
