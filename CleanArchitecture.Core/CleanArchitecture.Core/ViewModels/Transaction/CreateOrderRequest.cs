using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateTransactionRequest
    {
        [Required]
        public string pair_name { get; set; }

        [Required]
        public long CurrencyPairID { get; set; }

        [Required]
        public long DebitWalletID { get; set; }

        [Required]
        public long CreditWalletID { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal FeePer { get; set; }
        [Required]
        public decimal Fee { get; set; }

        [Required]
        public short TrnMode { get; set; }

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal price { get; set; }

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }       

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal Total { get; set; }

        [Required]
        public short ordertype { get; set; }

        [Required]
        public long Nonce { get; set; }//Timestamp
    }
}
