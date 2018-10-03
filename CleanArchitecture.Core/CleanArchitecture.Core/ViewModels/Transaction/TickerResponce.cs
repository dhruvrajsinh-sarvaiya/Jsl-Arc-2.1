using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class TickerResponce
    {
        //Binance
        public string symbol { get; set; }
        public decimal bidPrice { get; set; }
        public decimal  bidQty { get; set; }
        public decimal askPrice { get; set; }
        public decimal  askQty { get; set; }
    }

    
}
