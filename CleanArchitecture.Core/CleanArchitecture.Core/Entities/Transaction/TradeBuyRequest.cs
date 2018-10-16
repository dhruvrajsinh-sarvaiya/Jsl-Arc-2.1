    using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradeBuyRequest : BizBase
    {
        public new long Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TrnDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PickupDate { get; set; }
        [Required]
        public long MemberID { get; set; }
        [Key]
        public long TrnNo { get; set; }
        [Required]
        public long PairID { get; set; }
        public long ServiceID { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Qty { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BidPrice { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal PaidQty { get; set; }
        [Required]
        public long PaidServiceID { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DeliveredQty { get; set; }
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal PendingQty { get; set; }
        [Required]
        public short IsCancel { get; set; }
        [Required]
        public short IsPartialProceed { get; set; }
        [Required]
        public short IsProcessing { get; set; }
        public long SellStockID { get; set; }
        public long BuyStockID { get; set; }

    }
}
