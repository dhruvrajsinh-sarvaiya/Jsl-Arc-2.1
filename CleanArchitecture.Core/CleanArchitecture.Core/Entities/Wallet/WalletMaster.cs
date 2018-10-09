using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class WalletMaster : BizBase
    {      

        [Required]
        [StringLength(50)]
        public string Walletname { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Balance { get; set; }

        [Required]
        public long WalletTypeID { get; set; }

        [Required]
        public bool IsValid { get; set; }

        [Required]
        public string CoinName { get; set; }

        [Required]
        public long UserID { get; set; }

       // --------------------------------------------------
       //vsolanki 8-10-2018 add property for wallet
        [Required]
        public string UserKey { get; set; }

        [Required]
        public string BackupKey { get; set; }

        [Required]
        public string PublicKey { get; set; }

        public string passcodeEncryptionCode { get; set; }

        public bool disableTransactionNotifications { get; set; }

     ///   -------------------------------------------------
        public void CreditBalance(decimal amount)
        {
            Balance = Balance + amount;
            Events.Add(new WalletCrEvent<WalletMaster>(this));

        }
        public void DebitBalance(decimal amount)
        {
            Balance = Balance - amount;
            Events.Add(new WalletDrEvent<WalletMaster>(this));

        }

    }

}
