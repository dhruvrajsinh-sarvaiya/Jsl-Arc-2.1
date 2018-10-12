using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetRecentTradeResponce : BizResponseClass
    {
        public List<RecentOrderInfo> responce { get; set; }
    }
    public class RecentOrderInfo
    {
        public long TrnNo { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Qty { get; set; }
        public DateTime DateTime { get; set; }
        public short Status { get; set; }
    }
}
