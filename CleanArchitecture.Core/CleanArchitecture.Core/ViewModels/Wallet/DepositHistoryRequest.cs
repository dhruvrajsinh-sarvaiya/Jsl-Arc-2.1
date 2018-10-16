using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class DepositHistoryRequest
    {
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        public string Coin { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Amount { get; set; }
        public byte? Status { get; set; }
    }
}
