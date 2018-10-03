using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetTradeHistoryResponse
    {
        public long order_id { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public long timestamp { get; set; }
        public string type { get; set; }
        public long tid { get; set; }
        public string fee_currency { get; set; }
        public decimal fee_amount { get; set; }
    }
}
