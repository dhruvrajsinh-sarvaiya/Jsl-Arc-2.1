using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class GetWalletTransferReq
    {
        [Required]
        public string walletId { get; set; }
        [Required]
        public string id { get; set; }
    }
}
