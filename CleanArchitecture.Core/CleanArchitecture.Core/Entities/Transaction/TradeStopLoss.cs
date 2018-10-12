using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradeStopLoss : BizBase
    {      
        public long TrnNo { get; set; }

        public short ordertype { get; set; } //type of enTransactionMarketType
    }
}
