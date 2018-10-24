using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    //vsolnkki 24-10-2018
    public class MemberShadowLimit : BizBase
    {
        [Required]
        public long MemberTypeId { get; set; }//organization Id

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal ShadowLimitAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }//wallettype
    }
}
