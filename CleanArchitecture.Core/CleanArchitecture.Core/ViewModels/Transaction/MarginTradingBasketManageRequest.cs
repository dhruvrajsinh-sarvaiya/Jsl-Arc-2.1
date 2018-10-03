using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MarginTradingBasketManageRequest
    {
        public decimal amount { get; set; }
        public int dir { get; set; }
        public string pair_name { get; set; }
    }
}
