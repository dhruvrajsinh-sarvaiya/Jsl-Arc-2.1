using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletLimitConfigurationMaster : BizBase
    {
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new long Id { get; set; }

        [Required]
        [Key]
        public int TrnType { get; set; }

        [Required]
        public decimal LimitPerHour { get; set; }

        [Required]
        public decimal LimitPerDay { get; set; }

        [Required]
        public decimal LimitPerTransaction { get; set; }

        public decimal? LifeTime { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

    }
}
