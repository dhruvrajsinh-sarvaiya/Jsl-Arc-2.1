using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetMarginFundingTotalTakenFundsResponse : BizResponseClass
    {
        public List<GetMarginFundingTotalTakenFundsInfo> resposne { get; set; }
    }

    public class GetMarginFundingTotalTakenFundsInfo
    {
        public string position_pair { get; set; }
        public decimal total_swaps { get; set; }
    }
}
