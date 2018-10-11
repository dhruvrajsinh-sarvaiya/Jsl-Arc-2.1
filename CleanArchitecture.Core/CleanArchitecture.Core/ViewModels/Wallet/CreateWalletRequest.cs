using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class CreateWalletRequest
    {
        //vsolanki 10-10-2018 
        [Required(ErrorMessage = "Please Enter WalletName")]
        public string WalletName { get; set; }

        [Required(ErrorMessage = "Please Enter OTP")]
        public long OTP { get; set; }

        public byte IsDefaultWallet { get; set; }

        public int[] AllowTrnType { get; set; }

        //[Required]
        //public string label { get; set; }

        //[Required]
        //public string passphrase { get; set; }

        //public string userKey { get; set; }
        //public string backupXpub { get; set; }
        //public string backupXpubProvider { get; set; }
        //public string enterprise { get; set; }
        //public bool disableTransactionNotifications { get; set; }
        //public int gasPrice { get; set; }
        //public string passcodeEncryptionCode { get; set; }
    }
}
