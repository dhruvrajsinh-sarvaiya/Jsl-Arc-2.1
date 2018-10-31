using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
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
        public long PoolID { get; set; }
        [Key]
        public long TrnNo { get; set; }
        public long SellServiceID { get; set; }
        public long BuyServiceID { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Price { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Qty { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal RemainQty { get; set; }

        public short IsProcessing { get; set; }

        public void MakeTransactionInProcess()
        {
            Status = Convert.ToInt16(enTransactionStatus.Pending);
            AddValueChangeEvent();
        }
        public void MakeTransactionSuccess()
        {
            Status = Convert.ToInt16(enTransactionStatus.Success);
            AddValueChangeEvent();
        }
        public void MakeTransactionHold()
        {
            Status = Convert.ToInt16(enTransactionStatus.Hold);
            AddValueChangeEvent();
        }
        public void MakeTransactionOperatorFail()
        {
            Status = Convert.ToInt16(enTransactionStatus.OperatorFail);
            AddValueChangeEvent();
        }
        public void AddValueChangeEvent()
        {
            Events.Add(new ServiceStatusEvent<TradeSellerList>(this));
        }
    }
}
