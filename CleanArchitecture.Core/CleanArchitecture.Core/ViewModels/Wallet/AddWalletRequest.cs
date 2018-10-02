using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class AddWalletRequest
    {
        [Required]
        public string label { get; set; }

        [Required]
        public int m { get; set; }

        [Required]
        public int n { get; set; }

        [Required]
        public string[] keys { get; set; }

        public string enterprise { get; set; }
        public bool isCold { get; set; }
        public bool disableTransactionNotifications { get; set; }

    }
}
