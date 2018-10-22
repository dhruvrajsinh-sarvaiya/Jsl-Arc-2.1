using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WithdrawalRes : BizResponseClass
    {
        [JsonProperty(PropertyName = "TxId")]
        public long txid { get; set; }
        [JsonProperty(PropertyName = "StatusMsg")]
        public string statusMsg { get; set; }
        //public Transfer transfer { get; set; }
        // public string tx { get; set; }
    }
    //public class WithdrawalRootObject
    //{
    //}


}

