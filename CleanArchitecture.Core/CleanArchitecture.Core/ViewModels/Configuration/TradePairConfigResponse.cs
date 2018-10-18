using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TradePairConfigResponse : BizResponseClass
    {
        public TradePairConfigInfo response { get; set; }
    }
    public class TradePairConfigInfo
    {
        public long PairId { get; set; }
    }
}