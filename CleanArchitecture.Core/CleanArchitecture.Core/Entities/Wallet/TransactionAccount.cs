using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CleanArchitecture.Core.Entities
{
    public class TransactionAccount : BizBase
    {
        [Required]
        public long BatchNo { get; set; }

        [Required]
        public long RefNo { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public long WalletID { get; set; } // accountid fk of walletmaster

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal CrAmt { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DrAmt { get; set; }

        [Required]
        [StringLength(150)]
        public string Remarks { get; set; }

        [Required]
        public short IsSettled { get; set; }

        public void SetAsSettled()
        {
            IsSettled = 1;
            Events.Add(new ServiceStatusEvent<TransactionAccount>(this));
        }
    }

}
