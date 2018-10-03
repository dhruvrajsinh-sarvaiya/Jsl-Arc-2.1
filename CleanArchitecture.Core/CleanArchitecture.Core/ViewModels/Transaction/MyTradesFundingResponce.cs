using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MyTradesFundingResponce
    {
        public decimal  rate { get; set; }
        public long  period { get; set; }
        public decimal  amount { get; set; }
        public string   timestamp { get; set; }
        public string type { get; set; }
        public long  tid { get; set; }
        public long  offer_id { get; set; }
    }
}
