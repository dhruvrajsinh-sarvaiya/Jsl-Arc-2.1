using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class GetWalletRequest
    {
        [Required]
        public string id { get; set; }

        public bool allTokens { get; set; }
    }
}
