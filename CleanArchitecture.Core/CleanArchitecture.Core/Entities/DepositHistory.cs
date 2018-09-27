using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class DepositHistory : BaseEntity
    {     
        [Key]
        [StringLength(100)]
        public string TrnID { get; set; }

        [Required]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        public long Confirmations { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(100)]

        public string StatusMsg { get; set; }

        [Required]
        public string confirmedTime { get; set; }

        [Required]
        public string UnconfirmedTime { get; set; }

        [Required]
        public string CreatedTime { get; set; }
               
        public long OrderID { get; set; }

        public byte IsProcessing { get; set; }

        [Required]
        [StringLength(50)]
        public string FromAddress { get; set; }

        public string APITopUpRefNo { get; set; }

        public string SystemRemarks { get; set; }

        public string RouteTag { get; set; }

        public long? SerProID { get; set; }

    }

}
