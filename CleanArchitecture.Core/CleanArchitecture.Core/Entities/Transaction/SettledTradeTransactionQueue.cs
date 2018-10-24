using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class SettledTradeTransactionQueue : BizBase
    {
       
        public long TrnNo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DefaultValue("dbo.GetISTdate()")]
        public DateTime TrnDate { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        public short TrnType { get; set; }

        public string TrnTypeName { get; set; }

        [Required]
        public long PairID { get; set; }

        [Required]
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

        public decimal DeliveryTotalQty { get; set; }

        public long StatusCode { get; set; }

        public string StatusMsg { get; set; }

        public long ServiceID { get; set; }

        public long ProductID { get; set; }

        public long SerProID { get; set; }

        public int RoutID { get; set; }//change column as new structure

        public long? TrnRefNo { get; set; }

        public short IsCancelled { get; set; }

        [Range(0, 9999999999.99999999)]
        public decimal SettledBuyQty { get; set; }

        [Range(0, 9999999999.99999999)]
        public decimal SettledSellQty { get; set; }

        public DateTime? SettledDate { get; set; }

        public decimal TakerPer { get; set; }
    }
}
