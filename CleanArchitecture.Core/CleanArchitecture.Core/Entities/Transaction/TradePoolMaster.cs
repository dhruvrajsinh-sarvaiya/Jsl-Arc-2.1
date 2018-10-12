﻿using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradePoolMaster : BizBase
    {
        [Required]
        [StringLength(50)]
        public String PairName { get; set; }

        public long ProductID { get; set; }

        [Key]
        public long SellServiceID { get; set; }
        [Key]
        public long BuyServiceID { get; set; }
        [Key]
        public long BidPrice { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal TotalQty { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(37, 16)")]
        public decimal Landing
        {
            get { return BidPrice * TotalQty; }
            private set { }
        }

        [Required]
        public short OnProcessing { get; set; }
        [Required]
        public short TPSPickupStatus { get; set; }
        [Required]
        public short IsSleepMode { get; set;}
    }
}