using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletConfiguration
{
    public class WalletTypeMasterRequest 
    {
        [Required(ErrorMessage = "1,Please Enter parameters,4029")]
        [StringLength(50, ErrorMessage = "1,Please enter a valid parameters,4030")]
        public string WalletTypeName { get; set; }

        [Required(ErrorMessage = "1,Please Enter parameters,4029")]
        [StringLength(100, ErrorMessage = "1,Please enter a valid parameters,4030")]
        public string Discription { get; set; }

        [Required(ErrorMessage = "1,Please Enter parameters,4029")]
        public short IsDepositionAllow { get; set; }

        [Required(ErrorMessage = "1,Please Enter parameters,4029")]
        public short IsWithdrawalAllow { get; set; }

        [Required(ErrorMessage = "1,Please Enter parameters,4029")]
        public short IsTransactionWallet { get; set; }

        public short Status { get; set; }
    }
}
