using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ConvertTockenReq
    {
        [Required]
        public long SourceWalletId { get; set; }

        [Required]
        public long DestinationWalletId { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal SourcePrice { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal DestinationPrice { get; set; }
        // public decimal DestinationPrice { get; set; }
    }
}
