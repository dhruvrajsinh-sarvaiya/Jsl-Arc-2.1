﻿using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTransactionQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long TrnNo { get; set; }

        [Required]
        [StringLength(50)]
        public Guid Guid { get; set; }

        [Required]
        public enWalletTranxOrderType TrnType { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Amount { get; set; }

        [Required]
        public long TrnRefNo { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        //[Required]
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public long WalletID { get; set; }

        [Required]
        [StringLength(5)]
        public string WalletType { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeStamp { get; set; }

        public enTransactionStatus Status { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusMsg { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        [DefaultValue(0)]
        public decimal SettedAmt { get; set; }
        
        public enWalletTrnType WalletTrnType { get; set; }
    }
}
