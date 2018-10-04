using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MarginFundingResponce : BizResponseClass
    {
        public List<MarginFunding> MarginFunding { get; set; }
    }
    public class MarginFunding
    {
        public long id { get; set; }
        public decimal position_id { get; set; }
        public string currency { get; set; }
        public decimal rate { get; set; }
        public int period { get; set; }
        public decimal amount { get; set; }
        public string timestamp { get; set; }
        public bool auto_close { get; set; }
    }
}
