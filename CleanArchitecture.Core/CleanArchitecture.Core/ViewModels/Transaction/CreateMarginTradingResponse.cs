using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateMarginTradingResponse
    {
        public long offer_id { get; set; }
        public string currency { get; set; }
        public decimal rate { get; set; }
        public int period { get; set; }
        public string direction { get; set; }
        public bool is_live { get; set; }
        public bool is_cancelled { get; set; }
        public long  timestamp { get; set; }
        public decimal original_amount { get; set; }
        public decimal remaining_amount { get; set; }
        public decimal executed_amount { get; set; }
    }
}
