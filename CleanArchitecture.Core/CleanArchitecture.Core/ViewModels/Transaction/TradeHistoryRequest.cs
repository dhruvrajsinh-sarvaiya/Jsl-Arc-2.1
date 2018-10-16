using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class TradeHistoryRequest
    {
        
        public string Pair { get; set; }
        
        public string Trade { get; set; }
        
        public string FromDate { get; set; }
        
        public string ToDate { get; set; }
        
        public int Status { get; set; }
       
        public int Page { get; set; }
        
        public string MarketType { get; set; }
    }
}
