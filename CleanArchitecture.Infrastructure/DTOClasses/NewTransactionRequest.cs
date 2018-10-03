using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Infrastructure.DTOClasses
{
    public class NewTransactionRequestCls
    {
        [Required]
        public short TrnMode { get; set; }
        [Required]
        public short TrnType { get; set; }
        [Required]
        public long MemberID { get; set; }
        [Required]
        public string MemberMobile { get; set; }

        [Required]
        [StringLength(10)]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(200)]
        public string TransactionAccount { get; set; }//Mob for txn , address for crypto

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal Amount { get; set; }

        public long PairID { get; set; } = 0;

        [Range(0, 9999999999.99999999)]
        public decimal Price { get; set; } = 0;

        [Range(0, 9999999999.99999999)]
        public decimal Qty { get; set; } = 0;
    }
}
