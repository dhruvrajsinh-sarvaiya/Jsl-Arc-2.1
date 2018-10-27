using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ListWalletResponse : BizResponseClass
    {
        // public List<Wallet> wallets { get; set; }
         public List<WalletMasterResponse> Wallets { get; set; }
         public short? IsWhitelisting { get; set; }
    }
    //public class ListWalletResponse
    //{
    //    public class User
    //    {
    //        public string user { get; set; }
    //        public List<string> permissions { get; set; }
    //    }

    //    public class Freeze
    //    {
    //    }

    //    public class CoinSpecific
    //    {
    //    }

    //    public class Wallet
    //    {
    //        public string id { get; set; }
    //        public List<User> users { get; set; }
    //        public string coin { get; set; }
    //        public string label { get; set; }
    //        public int m { get; set; }
    //        public int n { get; set; }
    //        public List<string> keys { get; set; }
    //        public List<string> tags { get; set; }
    //        public bool disableTransactionNotifications { get; set; }
    //        public Freeze freeze { get; set; }
    //        public bool deleted { get; set; }
    //        public int approvalsRequired { get; set; }
    //        public CoinSpecific coinSpecific { get; set; }
    //    }

    //    public class ListWalletRootObject : ApiModels.BizResponseClass
    //    {
    //        public List<Wallet> wallets { get; set; }
    //    }
    //}
}
