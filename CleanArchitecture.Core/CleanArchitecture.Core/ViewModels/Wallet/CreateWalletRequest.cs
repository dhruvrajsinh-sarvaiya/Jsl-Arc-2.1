using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class CreateWalletRequest
    {
        [Required]
        public string label { get; set; }

        [Required]
        public string passphrase { get; set; }

        public string userKey { get; set; }
        public string backupXpub { get; set; }
        public string backupXpubProvider { get; set; }
        public string enterprise { get; set; }
        public bool disableTransactionNotifications { get; set; }
        public int gasPrice { get; set; }
        public string passcodeEncryptionCode { get; set; }
    }
}
