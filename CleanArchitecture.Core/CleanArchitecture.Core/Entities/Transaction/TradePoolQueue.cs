using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradePoolQueue : BizBase
    {
        [Required]
        public long PoolID { get; set; }
        public long SellerListID { get; set; }

        public long MakerTrnNo { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MakerQty { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal MakerPrice { get; set; }

        public long TakerTrnNo { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal TakerQty { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal TakerPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal TakerDisc { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal TakerLoss { get; set; }
    }
}
