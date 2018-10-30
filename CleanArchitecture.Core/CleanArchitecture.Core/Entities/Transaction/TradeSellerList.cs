using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradeSellerList : BizBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new long Id { get; set; }
        [Key]
        public long TrnNo { get; set; }
        public long BuyReqID { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Price { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Qty { get; set; }
    }
}
