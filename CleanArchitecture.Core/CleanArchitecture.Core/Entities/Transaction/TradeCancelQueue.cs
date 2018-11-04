using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradeCancelQueue:BizBase
    {
        [Required]
        public long TrnNo { get; set; }
        [Required]
        public long DeliverServiceID { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DefaultValue("dbo.GetISTdate()")]
        public DateTime TrnDate { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal PendingBuyQty { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DeliverQty { get; set; }

        public short? OrderType { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? DeliverBidPrice { get; set; }

        //public short Status { get; set; }

        [Required]
        public string StatusMsg { get; set; }
        [Required]
        public long OrderID { get; set; }
        public DateTime? SettledDate { get; set; }
    }
}
