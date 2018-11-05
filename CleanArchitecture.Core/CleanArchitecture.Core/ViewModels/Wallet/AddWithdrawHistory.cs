using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class AddWithdrawHistory
    {
        [Required(ErrorMessage ="")]
        [StringLength(100)]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Wallet { get; set; }

        [Key]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public long Confirmations { get; set; }

        [Required]
       // [Column(TypeName = "decimal(18, 8)")]
        public decimal Value { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Amount { get; set; }

        [Required]
        public decimal Charge { get; set; }

        [Required]
        public short State { get; set; }

        [Required]
        [StringLength(50)]
        public string confirmedTime { get; set; }

        [Required]
        [StringLength(50)]
        public string unconfirmedTime { get; set; }

        [Required]
        [StringLength(50)]
        public string createdTime { get; set; }

        [Required]
        public short IsProcessing { get; set; }

        [Required]
        [StringLength(50)]
        public string ToAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string APITopUpRefNo { get; set; }

        [Required]
        [StringLength(100)]
        public string SystemRemarks { get; set; }

        [Required]
        public long TrnNo { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public string RouteTag { get; set; }

        [Required]
        public long SerProID { get; set; }

        [Required]
        public short Status { get; set; }
    }
}
