using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class WalletTypeMaster : BizBase
    {
        [Required]
        [StringLength(50)]
        public string WalletTypeName { get; set; }

        [Required]
        [StringLength(100)]
        public string Discription { get; set; }
    }
}
