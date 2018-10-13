using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTransactionOrder: SharedKernel.BizBase
    {
        [Required]
        public long OrderID { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public long OWalletID { get; set; }

        [Required]
        public long DWalletID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string WalletType { get; set; }

        [Required]
        public long OTrnNo { get; set; }

        [Required]
        public long DTrnNo { get; set; }
       // public string Status { get; set; }
        public string StatusMsg { get; set; }
    }
}
