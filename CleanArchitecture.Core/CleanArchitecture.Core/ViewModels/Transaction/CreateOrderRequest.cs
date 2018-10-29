using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateTransactionRequest
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4601")]
        public long CurrencyPairID { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4602")]
        public string DebitWalletID { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4603")]
        public string CreditWalletID { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4604")]
        [Range(0, 100)]
        public decimal FeePer { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4605")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4606")]
        public short TrnMode { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4607")]
        [Range(0, 9999999999.99999999)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4608")]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4609")]
        [Range(0, 9999999999.99999999)]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4610")]
        [EnumDataType(typeof(enTransactionMarketType))]
        public enTransactionMarketType OrderType { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4611")]
        [EnumDataType(typeof(enTrnType),ErrorMessage = "1,Invalid Parameter Value,4612")]
        public enTrnType OrderSide { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4613")]
        public long Nonce { get; set; }//Timestamp
    }
    public class WithdrawalRequest
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4614")]
        public string asset { get; set; }//Timestamp

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4615")]
        public string address { get; set; }//Timestamp

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4602")]
        public string DebitWalletID { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4606")]
        public short TrnMode { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4608")]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4616")]
        public enWhiteListingBit WhitelistingBit { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4617")]
        public string AddressLabel { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4613")]
        public long Nonce { get; set; }//Timestamp
    }
}
