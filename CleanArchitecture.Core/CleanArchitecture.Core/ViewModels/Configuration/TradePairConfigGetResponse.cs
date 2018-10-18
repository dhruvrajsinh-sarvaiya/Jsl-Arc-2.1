using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TradePairConfigGetResponse : BizResponseClass
    {
        public TradePairConfigRequest response { get; set; }
    }
}
