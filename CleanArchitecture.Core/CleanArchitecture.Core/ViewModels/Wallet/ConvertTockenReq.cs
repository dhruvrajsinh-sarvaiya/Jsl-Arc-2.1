using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ConvertTockenReq
    {
        public string SourceCoin { get; set; }
        public string DestinationCoin { get; set; }
        public decimal SourcePrice { get; set; }
        public string DestinationPrice { get; set; }
    }
}
