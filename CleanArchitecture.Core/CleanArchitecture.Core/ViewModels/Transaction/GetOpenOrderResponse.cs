using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetOpenOrderResponse : BizResponseClass
    {
        public List<GetOpenOrderInfo> response { get; set; }
    }

    public class GetOpenOrderInfo
    {
        public long order_id { get; set; }
        public string pair_name { get; set; }
        public string side { get; set; }
        public string type { get; set; }
        public decimal price { get; set; }
        public long timestamp { get; set; }
        public decimal volume { get; set; }
    }
}
