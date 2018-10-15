using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTransactionOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long OrderID { get; set; }

        //[Required]
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public long OWalletID { get; set; }

        [Required]
        public long DWalletID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(5)]
        public string WalletType { get; set; }

        [Required]
        public long OTrnNo { get; set; }

        [Required]
        public long DTrnNo { get; set; }

        public byte Status { get; set; }

        [Required]
        public string StatusMsg { get; set; }
    }
}
