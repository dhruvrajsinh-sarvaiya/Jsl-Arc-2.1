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
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Walletname { get; set; }

        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Balance { get; set; }

        [Required]
        public long WalletTypeID { get; set; }

        [Required]
        public bool IsValid { get; set; }

        [Required]
        [StringLength(16)] 
        [Key]
        public string AccWalletID { get; set; } // dynamically generated accountid 

        [Required]
        public long UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string PublicAddress { get; set; }
                
        public byte IsDefaultWallet { get; set; }

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
        public void WalletPublicAddress(string publicAddress)
        {
            PublicAddress = publicAddress;
            Events.Add(new WalletPublicAddress<WalletMaster>(this));
        }
    }

}

