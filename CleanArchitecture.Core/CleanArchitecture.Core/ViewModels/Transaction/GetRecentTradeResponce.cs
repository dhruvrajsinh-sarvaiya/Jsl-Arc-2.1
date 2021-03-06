﻿using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetRecentTradeResponce : BizResponseClass
    {
        public List<RecentOrderInfo> response { get; set; }
    }
    public class RecentOrderInfo
    {
        public long TrnNo { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Qty { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public string PairName { get; set; }
        public long PairId { get; set; }
        public string OrderType { get; set; }
    }
}
