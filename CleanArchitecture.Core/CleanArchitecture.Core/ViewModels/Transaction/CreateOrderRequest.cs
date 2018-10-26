using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateTransactionRequest
    {        
        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public long CurrencyPairID { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public string DebitWalletID { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public string CreditWalletID { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 100)]
        public decimal FeePer { get; set; }
        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public short TrnMode { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [EnumDataType(typeof(enTransactionMarketType))]
        public enTransactionMarketType OrderType { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")] 
        [EnumDataType(typeof(enTrnType),ErrorMessage = "1,Invalid Parameter Value,4003")]
        public enTrnType OrderSide { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public long Nonce { get; set; }//Timestamp
    }
    public class WithdrawalRequest
    {
        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public string asset { get; set; }//Timestamp

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public string address { get; set; }//Timestamp

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public long DebitWalletID { get; set; }        

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public short TrnMode { get; set; }      

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public long Nonce { get; set; }//Timestamp
    }
}
