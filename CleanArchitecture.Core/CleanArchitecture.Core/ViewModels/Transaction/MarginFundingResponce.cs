using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MarginFundingResponce
    {
        public string rate { get; set; }
        public int period { get; set; }
        public string amount { get; set; }
        public string timestamp { get; set; }
        public string type { get; set; }
        public int tid { get; set; }
        public int offer_id { get; set; }
    }
}
