using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TradePairConfigGetAllResponse : BizResponseClass
    {
        public List<TradePairConfigRequest> response { get; set; }
    }
}
