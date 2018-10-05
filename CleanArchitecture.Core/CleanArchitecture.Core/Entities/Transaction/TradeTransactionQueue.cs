using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class TradeTransactionQueue
    {
        [Key]
        public long TrnNo { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        public short TrnType { get; set; }

        public string TrnTypeName { get; set; }

        //[Required]
        //public short TrnMode { get; set; }

        [Required]
        public long PairID { get; set; }

        [Required]
        public string PairName { get; set; }

        public long OrderWalletID { get; set; }

        public long DeliveryWalletID { get; set; }

        public decimal BuyQty { get; set; }

        public decimal BidPrice { get; set; }

        public decimal SellQty { get; set; }

        public decimal AskPrice { get; set; }

        public string Order_Currency { get; set; }

        public decimal OrderTotalQty { get; set; }

        public string Delivery_Currency { get; set; }

        public decimal DeliveryTotalQty { get; set; }

        public short Status { get; set; }

        public int StatusCode { get; set; }

        public string StatusMsg { get; set; }

        public long ServiceID { get; set; }

        public long ProductID { get; set; }

        public long SerProID { get; set; }

        public int RoutID { get; set; }//change column as new structure

        public long? TrnRefNo { get; set; }

        public short IsCancelled { get; set; }

        public decimal SettledBuyQty { get; set; }

        public decimal SettledSellQty { get; set; }

        public DateTime? SettledDate { get; set; }

        public decimal TakerPer { get; set; }
    }

}
