using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CancelOfferReasponce : BizResponseClass
    {
        public CancelOfferInfo response { get; set; }
    }

    public class CancelOfferInfo
    {
        public long id { get; set; }
        public string symbol { get; set; }
        public object exchange { get; set; }
        public decimal price { get; set; }
        public decimal avg_execution_price { get; set; }
        public string side { get; set; }
        public string type { get; set; }
        public string timestamp { get; set; }
        public bool is_live { get; set; }
        public bool is_cancelled { get; set; }
        public bool is_hidden { get; set; }
        public bool was_forced { get; set; }
        public decimal original_amount { get; set; }
        public decimal remaining_amount { get; set; }
        public decimal executed_amount { get; set; }
    }
}
