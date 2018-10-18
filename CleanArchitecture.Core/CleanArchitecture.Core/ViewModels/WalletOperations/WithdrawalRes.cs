using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WithdrawalRes : BizResponseClass
    {
        public long txid { get; set; }
        public string statusMsg { get; set; }
        //public Transfer transfer { get; set; }
        // public string tx { get; set; }
    }
    //public class WithdrawalRootObject
    //{
    //}


}

