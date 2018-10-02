using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class UpdateWalletAddressReq
    {
        [Required]
        public string walletId { get; set; }
        [Required]
        public string addressOrId { get;set;}
        public string label { get; set; }
    }
}
