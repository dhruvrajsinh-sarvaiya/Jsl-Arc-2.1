using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class TransactionAccount : BizBase
    {       
        public long BatchNo { get; set; }

        public long RefNo { get; set; }

        public DateTime TrnDate { get; set; }

        public long WalletID { get; set; } // accountid fk of walletmaster

        public decimal CrAmt { get; set; }

        public decimal DrAmt { get; set; }       

        public string Remarks { get; set; }

        public short IsSettled { get; set; }

        public void SetAsSettled()
        {
            IsSettled = 1;
            Events.Add(new TransactionAccountSetAsSettled(this));
        }
    }

}
