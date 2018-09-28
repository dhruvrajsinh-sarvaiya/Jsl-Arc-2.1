using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class WalletLedger : BizBase
    {
        [Required]
        public long WalletMasterId { get; set;} // fk of walletmaster table

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public enServiceType ServiceTypeID { get; set; } // fk of ServiceTypeMaster table

        [Required]
        public enTrnType TrnType { get; set; } // type of txn

        [Required]
        public long TrnNo { get; set; }

        [Required]        
        [Column(TypeName = "decimal(18, 8)")]
        public decimal CrAmt { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DrAmt { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal PreBal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal PostBal { get; set; }

        [Required]
        [StringLength(100)]
        public string Remarks { get; set; }
       
    }
}
