using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetTradeHistoryResponse : BizResponseClass
    {
        public List<GetTradeHistoryInfo> response { get; set; }
    }
    public class GetTradeHistoryInfo
    {
        public long TrnNo { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Total { get; set; }
        public DateTime DateTime { get; set; }
        public short Status { get; set; }
        public string StatusText { get; set; }
        public long PairID { get; set; }
        public Decimal ChargeRs { get; set; }
        
        //public short  IsCancel { get; set; }
    }
}
