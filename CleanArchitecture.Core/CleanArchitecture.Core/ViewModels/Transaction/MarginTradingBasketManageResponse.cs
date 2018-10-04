using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MarginTradingBasketManageResponse : BizResponseClass
    {
       public MarginTradingBasketManageInfo response { get; set; }
    }

    public class MarginTradingBasketManageInfo
    {
        public string message { get; set; }
    }
}
