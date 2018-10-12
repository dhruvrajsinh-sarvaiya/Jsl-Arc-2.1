using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletMasterResponse
    {
        public string Walletname { get; set; }
        public decimal Balance { get; set; }
        public string CoinName { get; set; }
        public string AccWalletID { get; set; }  
        public string PublicAddress { get; set; }
    }
}
