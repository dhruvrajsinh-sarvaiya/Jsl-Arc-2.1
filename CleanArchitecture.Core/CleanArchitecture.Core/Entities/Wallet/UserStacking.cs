using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    //vsolnkki 24-10-2018
    public class UserStacking : BizBase
    {
        [Required]
        public long SchemeId { get; set; }//fk

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal StackingAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string WalletType { get; set; }//wallettype

        public string Remarks { get; set; }
    }
}
