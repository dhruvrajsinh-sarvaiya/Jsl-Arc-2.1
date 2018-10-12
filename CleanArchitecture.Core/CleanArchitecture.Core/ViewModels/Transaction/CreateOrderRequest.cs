﻿using CleanArchitecture.Core.Enums;
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
        public long DebitWalletID { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public long CreditWalletID { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 100)]
        public decimal FeePer { get; set; }
        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public short TrnMode { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal price { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [Range(0, 9999999999.99999999)]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        [EnumDataType(typeof(enTransactionMarketType))]
        public enTransactionMarketType ordertype { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")] 
        [EnumDataType(typeof(enTrnType),ErrorMessage = "1,Required Parameter Enum,4003")]
        public enTrnType orderSide { get; set; }

        [Required(ErrorMessage = "1,Required Parameter,4003")]
        public long Nonce { get; set; }//Timestamp
    }
}
