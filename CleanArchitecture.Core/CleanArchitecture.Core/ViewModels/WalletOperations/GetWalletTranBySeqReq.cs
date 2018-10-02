using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class GetWalletTranBySeqReq
    {
        [Required]
        public string walletId { get; set; }
        [Required]
        public string sequenceId { get; set; }
    }
}
