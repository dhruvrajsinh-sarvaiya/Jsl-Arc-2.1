using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class WalletOrder : BizBase // Similler to MemberOrder table
    {      

        public DateTime OrderDate { get; set; }

        public byte TrnMode { get; set; }

        public long OWalletMasterID { get; set; } 

        public long DWalletMasterID { get; set; }

        public decimal OrderAmt { get; set; }

        public double DiscPer { get; set; }

        public decimal DiscRs { get; set; }

        //public long? OBankID { get; set; }

        //public string OBranchName { get; set; }

        //public string OAccountNo { get; set; }

        //public string OChequeNo { get; set; }

        //public DateTime? OChequeDate { get; set; }

        //public long DMemberID { get; set; }

        //public long DBankID { get; set; }

        //public string DAccountNo { get; set; }

        //public byte Status { get; set; }

        public string ORemarks { get; set; }

        public decimal DeliveryAmt { get; set; }

        public string DRemarks { get; set; }

        public long? DeliveryGivenBy { get; set; }

        public DateTime? DeliveryGivenDate { get; set; }

        //public byte AlertRec { get; set; }

        //public double CashChargePer { get; set; }

        //public decimal CashChargeRs { get; set; }

        //public decimal WalletAmt { get; set; }

        //public int? PGId { get; set; }

        //public long? CouponNo { get; set; }

        //public bool? IsChargeAccepted { get; set; }

        //public bool? IsDebited { get; set; }

       

    }

}
