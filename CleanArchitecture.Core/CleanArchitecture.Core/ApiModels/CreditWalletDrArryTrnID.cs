using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class CreditWalletDrArryTrnID
    {
        public long DrTrnRefNo { get; set; }
        public decimal Amount { get; set; }
        public long dWalletId { get; set; }
    }
}
