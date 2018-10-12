using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class CreateWalletAddressReq
    {
        //[Required(ErrorMessage = "9,Please Enter Wallet ID,4003")]
        //[StringLength(250, ErrorMessage = "9,Invalid Wallet ID,4004")]
        public bool allowMigrated { get; set; }
        public int chain{ get; set;}
        public int gasPrice { get; set; }
        public bool lowPriority { get; set; }
        public string label { get; set; }
        public int count { get; set; }
    }
}
