using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradePairMaster : BizBase
    {
        [Required]
        public string Pairname { get; set; }
        [Required]
        public long Serviceid { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Currentrate { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMinQty { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public short BuyMaxQty { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMinQty { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMaxQty { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DailyHigh { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DailyLow { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal CurrencyPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Volume { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMinPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMaxPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMinPrice { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMaxPrice { get; set; }

        [Required]
        public long WalletMasterID { get; set; }

        [Required]
        public long WalletServiceID { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Fee { get; set; }
        public short FeeType { get; set; }
        public long LastTrnNo { get; set; }

        public void MakePairActive()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            AddValueChangeEvent();
        }
        public void MakePairInActive()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            AddValueChangeEvent();
        }
        public void AddValueChangeEvent()
        {
            Events.Add(new ServiceStatusEvent<TradePairMaster>(this));
        }
    }
}
