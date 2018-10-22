using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class WalletMasterResponse
    {
        public string WalletName { get; set; }
        public decimal Balance { get; set; }
        public string CoinName { get; set; }
        public string AccWalletID { get; set; }
        public string PublicAddress { get; set; }       
        public byte IsDefaultWallet { get; set; }
    }
}
