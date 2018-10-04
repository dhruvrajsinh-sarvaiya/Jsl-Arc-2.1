using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CloseMarginFundingResponce: BizResponseClass
    {
        public MarginFunding MarginFunding { get; set; }
    }
}
