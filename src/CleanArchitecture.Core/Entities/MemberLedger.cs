using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class MemberLedger : BizBase
    {

        public long WalletMasterId { get; set;} // fk of walletmaster table

        public DateTime TrnDate { get; set; }

        public byte ServiceTypeID { get; set; } // fk of ServiceTypeMaster table

        public long TrnNo { get; set; }

        public decimal CrAmt { get; set; }

        public decimal DrAmt { get; set; }
        
        public decimal PreBal { get; set; }

        public decimal PostBal { get; set; }

        public string Remarks { get; set; }
       
    }
}
