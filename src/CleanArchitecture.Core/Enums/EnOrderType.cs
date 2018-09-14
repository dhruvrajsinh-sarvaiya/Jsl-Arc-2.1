using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Enums
{
    public enum EnOrderType
    {
         BuyOrder = 1,
         SellOrder = 2,
         CancelOrder = 3,
         DepositOrder = 4,
         WithdrawalOrder = 5,
         CashOnBank = 6,
         Cheque =7,
         Transfer = 8            
    }
}
