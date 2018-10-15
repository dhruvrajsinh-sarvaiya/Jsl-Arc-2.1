using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTransactionQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long TrnNo { get; set; }

        [Required]
        [StringLength(50)]
        public Guid Guid { get; set; }

        [Required]
        public byte TrnType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public long TrnRefNo { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        //[Required]
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public long WalletID { get; set; }

        [Required]
        [StringLength(5)]
        public string WalletType { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        [StringLength(5)]

        public string TimeStamp { get; set; }

        public byte Status { get; set; }

        [Required]
        [StringLength(5)]
        public string StatusMsg { get; set; }
    }
}
