using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetActiveOrderResponse
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
