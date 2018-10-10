using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTrnType : BizBase
    {
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }
    }
}
