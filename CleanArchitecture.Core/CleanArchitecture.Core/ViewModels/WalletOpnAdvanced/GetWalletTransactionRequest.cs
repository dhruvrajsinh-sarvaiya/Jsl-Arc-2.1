using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class GetWalletTransactionRequest
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string txid { get; set; }
    }
}
