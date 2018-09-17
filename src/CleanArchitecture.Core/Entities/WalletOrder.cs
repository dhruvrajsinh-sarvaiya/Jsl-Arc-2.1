using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class WalletOrder : BizBase // Similler to MemberOrder table
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public EnOrderType OrderType { get; set; }

        [Required]
        public long OWalletMasterID { get; set; }

        [Required]
        public long DWalletMasterID { get; set; }

        [Required]       
        [Range(1, 99999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal OrderAmt { get; set; }

        [Required]
        public new EnOrderStatus Status { get; set; }

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
        [Required]
        [StringLength(100)]
        public string ORemarks { get; set; }

        [Required]
        [Range(1, 99999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
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

        public void SetAsSuccess()
        {
            Status = EnOrderStatus.Success;
            Events.Add(new CommonCompletedEvent<WalletOrder>(this));
        }
        public void SetAsRejected()
        {
            Status = EnOrderStatus.Rejected;
            Events.Add(new CommonCompletedEvent<WalletOrder>(this));
        }

    }



}
