using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetRecentTradeResponce
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public decimal   qty { get; set; }
        public long time { get; set; }
        public bool isBuyerMaker { get; set; }
        public bool isBestMatch { get; set; }
    }
    
}
