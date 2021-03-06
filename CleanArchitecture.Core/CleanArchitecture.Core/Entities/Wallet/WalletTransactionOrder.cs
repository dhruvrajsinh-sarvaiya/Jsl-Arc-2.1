﻿using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTransactionOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long OrderID { get; set; }

        //[Required]
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public long OWalletID { get; set; }

        [Required]
        public long DWalletID { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(5)]
        public string WalletType { get; set; }

        [Required]
        public long OTrnNo { get; set; }

        [Required]
        public long DTrnNo { get; set; }

        public enTransactionStatus Status { get; set; }


        [Required]
        [StringLength(50)]
        public string StatusMsg { get; set; }
    }
}
