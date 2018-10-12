using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class CreateWalletRequest
    {
        //vsolanki 10-10-2018 
        [Required(ErrorMessage = "1,Please Enter WalletName,4029")]
        [StringLength(250, ErrorMessage = "1,Please enter a valid WalletName,4030")]
        public string WalletName { get; set; }

        [Required(ErrorMessage = "1,Please Enter OTP,4025")]
        //[StringLength(6, MinimumLength = 6,ErrorMessage = "1,Please Enter Valid OTP,4026")]
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
