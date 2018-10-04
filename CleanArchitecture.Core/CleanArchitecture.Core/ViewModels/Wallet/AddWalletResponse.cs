using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class AddWalletResponse : BizResponseClass
    {
        public Wallet2 wallet { get; set; }
        //public class Wallet2
        //{
        //    public Wallet _wallet { get; set; }
        //}
    }
   
    //public class User
    //{
    //    public string user { get; set; }
    //    public List<string> permissions { get; set; }
    //}

    //public class Freeze
    //{
    //}

    //public class LastChainIndex
    //{
    //    public int __invalid_name__0 { get; set; }
    //    public int __invalid_name__1 { get; set; }
    //}

    //public class CoinSpecific
    //{
    //    public bool deployedInBlock { get; set; }
    //    public string deployTxHash { get; set; }
    //    public LastChainIndex lastChainIndex { get; set; }
    //    public string baseAddress { get; set; }
    //    public string feeAddress { get; set; }
    //    public bool pendingChainInitialization { get; set; }
    //    public List<object> creationFailure { get; set; }
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
    //    public List<object> pendingApprovals { get; set; }
    //}

    //public class Wallet
    //{
    //    public Wallet2 _wallet { get; set; }
    //}

    //public class AddWalletRootObject : BizResponseClass
    //{
    //    public Wallet wallet { get; set; }
    //}
    // }
}
