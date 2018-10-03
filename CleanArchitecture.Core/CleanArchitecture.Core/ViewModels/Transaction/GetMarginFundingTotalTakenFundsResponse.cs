using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetMarginFundingTotalTakenFundsResponse
    {
        public string position_pair { get; set; }
        public decimal total_swaps { get; set; }
    }
}
