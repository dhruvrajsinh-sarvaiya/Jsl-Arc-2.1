using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class CreateWalletResponse : BizResponseClass
    {

        public string PublicAddress { get; set; }

        public string AccWalletID { get; set; }
        //vsolanki 25-10-2018
        public List<WalletLimitConfigurationRes> Limits { get; set; }
        // public Wallet2 wallet { get; set; }
    }
    //public class Wallet2
    //{
    //    public Wallet _wallet { get; set; }
    //}
    //public class User
    //{
    //    public string user { get; set; }
    //    public List<string> permissions { get; set; }
    //}

    //public class Freeze
    //{
    //}

    //public class CoinSpecific
    //{
    //}

    //public class CoinSpecific2
    //{
    //    public string redeemScript { get; set; }
    //}

    //public class ReceiveAddress
    //{
    //    public string address { get; set; }
    //    public int chain { get; set; }
    //    public int index { get; set; }
    //    public string coin { get; set; }
    //    public string wallet { get; set; }
    //    public CoinSpecific2 coinSpecific { get; set; }
    //}

    //public class Wallet2
    //{
    //    public string id { get; set; }
    //    public List<User> users { get; set; }
    //    public string coin { get; set; }
    //    public string label { get; set; }
    //    public int m { get; set; }
    //    public int n { get; set; }
    //    public List<string> keys { get; set; }
    //    public List<string> tags { get; set; }
    //    public bool disableTransactionNotifications { get; set; }
    //    public Freeze freeze { get; set; }
    //    public bool deleted { get; set; }
    //    public int approvalsRequired { get; set; }
    //    public bool isCold { get; set; }
    //    public CoinSpecific coinSpecific { get; set; }
    //    public int balance { get; set; }
    //    public int confirmedBalance { get; set; }
    //    public int spendableBalance { get; set; }
    //    public string balanceString { get; set; }
    //    public string confirmedBalanceString { get; set; }
    //    public string spendableBalanceString { get; set; }
    //    public ReceiveAddress receiveAddress { get; set; }
    //    public List<object> pendingApprovals { get; set; }
    //}

    //public class Wallet
    //{
    //    public Wallet2 _wallet { get; set; }
    //}

    //public class CreateWalletRootObject : BizResponseClass
    //{
    //    public Wallet wallet { get; set; }
    //}

}
