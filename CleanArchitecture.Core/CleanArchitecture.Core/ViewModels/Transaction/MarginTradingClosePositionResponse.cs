using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MarginTradingClosePositionResponse : BizResponseClass
    {
       public MarginTradingClosePositionInfo response { get; set; }
    }
    public class MarginTradingOrder
    {

    }

    public class MarginTradingPosition
    {

    }

    public class MarginTradingClosePositionInfo
    {
        public string message { get; set; }
        public MarginTradingOrder order { get; set; }
        public MarginTradingPosition position { get; set; }
    }
}
