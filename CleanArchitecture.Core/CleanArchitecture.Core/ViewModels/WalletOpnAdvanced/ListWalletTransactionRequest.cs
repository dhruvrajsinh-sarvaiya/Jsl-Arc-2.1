using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ListWalletTransactionRequest
    {
        [Required]
        public string id { get; set; }

        public string prevId { get; set; }
        public bool allTokens { get; set; }
    }
}
