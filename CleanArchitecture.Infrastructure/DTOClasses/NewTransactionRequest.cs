using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Infrastructure.DTOClasses
{
    public class NewTransactionRequestCls
    {
        [Required]
        public short TrnMode { get; set; }
        [Required]
        public enTrnType TrnType { get; set; }
        [Required]
        public long MemberID { get; set; }
        [Required]
        public string MemberMobile { get; set; }

        [Required]
        [StringLength(10)]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(200)]
        public string TransactionAccount { get; set; }//Mob for txn , address for crypto

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        public long PairID { get; set; } = 0;

        [Range(0, 9999999999.99999999)]
        public decimal Price { get; set; } = 0;

        [Range(0, 9999999999.99999999)]
        public decimal Qty { get; set; } = 0;

        public string TrnRefNo { get; set; }
        public string AdditionalInfo { get; set; }
        public enTransactionStatus Status { get; set; }
        public int StatusCode { get; set; }
        public string StatusMsg { get; set; }
        public long WalletID { get; set; }

    }

    public class NewTradeTransactionRequestCls
    {
        public long TrnNo { get; set; }
        public string TrnTypeName { get; set; }
        public string PairName { get; set; }
        public long OrderWalletID { get; set; }
        public long DeliveryWalletID { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal BuyQty { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal BidPrice { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal SellQty { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal AskPrice { get; set; }
        public string Order_Currency { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal OrderTotalQty { get; set; }
        public string Delivery_Currency { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal DeliveryTotalQty { get; set; }
        //public long? TrnRefNo { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal SettledBuyQty { get; set; }
        [Range(0, 9999999999.99999999)]
        public decimal SettledSellQty { get; set; }
        //public decimal TakerPer { get; set; }
    }
}
