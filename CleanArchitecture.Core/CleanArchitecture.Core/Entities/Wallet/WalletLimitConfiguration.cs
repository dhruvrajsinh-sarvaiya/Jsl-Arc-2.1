using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletLimitConfiguration : BizBase
    {
        [Required]
        public long ID { get; set; }

        [Required]
        public long WalletId { get; set; }

        [Required]
        public int TrnType { get; set; }

        [Required]
        public decimal LimitPerHour { get; set; }

        [Required]
        public decimal LimitPerDay { get; set; }

        [Required]
        public decimal LimitPerTransaction { get; set; }
    }
}
