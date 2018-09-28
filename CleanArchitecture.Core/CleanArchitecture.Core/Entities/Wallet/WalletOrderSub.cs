using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;

namespace CleanArchitecture.Core.Entities
{
    class WalletOrderSub : BizBase
    {
        [Required]
        public long WalletOrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string OBranchName { get; set; }

        [Required]
        [StringLength(20)]
        public string OAccountNo { get; set; }

        [Required]
        [StringLength(20)]
        public string OChequeNo { get; set; }

        [Required]
        public DateTime? OChequeDate { get; set; }
                
        public long? RefNo { get; set; }

        //public byte AlertRec { get; set; }

        //public double CashChargePer { get; set; }

        //public decimal CashChargeRs { get; set; }

        //public decimal WalletAmt { get; set; }

        //public int? PGId { get; set; }

        //public long? CouponNo { get; set; }

        //public bool? IsChargeAccepted { get; set; }

        //public bool? IsDebited { get; set; }

        //public double DiscPer { get; set; }

        //public decimal DiscRs { get; set; }

        //public long? OBankID { get; set; }

        //public string OBranchName { get; set; }

        //public string OAccountNo { get; set; }

        //public string OChequeNo { get; set; }

        //public DateTime? OChequeDate { get; set; }

        //public long DMemberID { get; set; }

        //public long DBankID { get; set; }

        //public string DAccountNo { get; set; }

        //public byte Status { get; set; }

        public void SetAsSuccess()
        {
            Status = Convert.ToInt16(enOrderStatus.Success);
            Events.Add(new ServiceStatusEvent<WalletOrderSub>(this));
        }
        public void SetAsRejected()
        {
            Status = Convert.ToInt16(enOrderStatus.Rejected);
            Events.Add(new ServiceStatusEvent<WalletOrderSub>(this));
        }
    }
}
