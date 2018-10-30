using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class PoolOrder : BizBase
    {       
        public long PoolID { get; set; }
        public long TrnNo { get; set; }
        [Required]
        public long UserID { get; set; }       
        [Required]
        public short PayMode { get; set; }
        [Required]
        public short TrnMode { get; set; }
        public string ORemarks { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal OrderAmt { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DiscPer { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DiscRs { get; set; }
       
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DeliveryAmt { get; set; }
        public string DRemarks { get; set; }
        public long UserWalletID { get; set; }
        public string UserWalletAccID { get; set; }
        public long CancelID { get; set; }
        //public short OBankID { get; set; }
        //public string OBranchName { get; set; }
        //public string OAccountNo { get; set; }
        //public string OChequeNo { get; set; }
        //public DateTime OChequeDate { get; set; }       
        //[Required]
        //public short DBankID { get; set; }
        //[Required]
        //public string DAccountNo { get; set; }
        //public long DeliveryGivenBy { get; set; }        
        //public DateTime DeliveryGivenDate { get; set; }
        //[Required]
        //public byte AlertRec { get; set; }
        //[Required]
        //public byte AlertRec { get; set; }       
        //[Required]
        //public double CashChargePer { get; set; }
        //[Required]
        //[Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 8)")]
        //public decimal CashChargeRs { get; set; }
        //[Required]
        //[Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 8)")]
        //public decimal WalletAmt { get; set; }
        //public int PGId { get; set; }
        ////public long CouponNo { get; set; }
        //public bool IsChargeAccepted { get; set; }
        //public bool IsDebited { get; set; }

    }
}
