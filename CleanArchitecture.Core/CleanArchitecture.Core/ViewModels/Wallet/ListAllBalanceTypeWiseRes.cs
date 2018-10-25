using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ListAllBalanceTypeWiseRes
    {
        public List<AllBalanceTypeWiseRes> Wallets { get; set; }
        public BizResponseClass BizResponseObj { get; set; }
    }
    public class AllBalanceTypeWiseRes
    {
        public WalletResponse Wallet { get; set; }
        //public WalletMasterResponse Wallet { get; set; }
        //public Balance Balance { get; set; }        
    }
    public class WalletResponse
    {
        public string WalletName { get; set; }
        public string TypeName { get; set; }
        public string AccWalletID { get; set; }
        public string PublicAddress { get; set; }
        public byte IsDefaultWallet { get; set; }
        public Balance Balance { get; set; }
    }
}
