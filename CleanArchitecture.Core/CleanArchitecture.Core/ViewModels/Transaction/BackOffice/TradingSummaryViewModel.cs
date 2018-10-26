using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction.BackOffice
{
    public class TradingSummaryViewModel
    {
        public long TrnNo { get; set; }
        public long MemberID { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Total { get; set; }
        public DateTime DateTime { get; set; }
        public string StatusText { get; set; }
        public long PairID { get; set; }
        public String PairName { get; set; }
        public Decimal ChargeRs { get; set; }
        public Decimal PreBal { get; set; }
        public Decimal PostBal { get; set; }
    }
    
}
