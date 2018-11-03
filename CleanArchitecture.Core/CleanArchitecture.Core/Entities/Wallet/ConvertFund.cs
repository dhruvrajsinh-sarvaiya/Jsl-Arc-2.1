using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class ConvertFund : BizBase
    {
        [Required]
        public long FromWalletId { get; set; }

        [Required]
        public long ToWalletId { get; set; }

        //[Required]
        //public long  SourceWalletTypeId{get;set;}

        //[Required]
        //public long  DestinationWalletTypeId{get;set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DestinationPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SourcePrice { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }
    }
}
