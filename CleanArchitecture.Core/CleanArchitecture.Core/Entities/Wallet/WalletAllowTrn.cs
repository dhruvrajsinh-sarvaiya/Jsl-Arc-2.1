using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletAllowTrn : BizBase
    {
        [Required]
        public long WalletId { get; set; }

        [Required]
        public byte TrnType { get; set; } // fk of wallettrntype

    }
}
