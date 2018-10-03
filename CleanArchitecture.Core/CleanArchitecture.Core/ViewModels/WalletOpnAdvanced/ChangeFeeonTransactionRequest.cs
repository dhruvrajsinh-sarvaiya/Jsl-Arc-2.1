using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ChangeFeeonTransactionRequest
    {
        //[Required]
        //public string walletId { get; set; }

        [Required]
        public string txid { get; set; }

        [Required]
        public int fee { get; set; }
    }
}
