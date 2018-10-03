using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetMarketSummaryRequest
    {
        public string pair_name { get; set; }
        public int interval { get; set; }
    }
}
