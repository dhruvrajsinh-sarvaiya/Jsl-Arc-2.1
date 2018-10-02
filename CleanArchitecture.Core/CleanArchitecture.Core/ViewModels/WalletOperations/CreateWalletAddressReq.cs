using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class CreateWalletAddressReq
    {
        [Required]
        public string id { get; set; }
        public bool allowMigrated { get; set; }
        public int chain{ get; set;}
        public int gasPrice { get; set; }
        public bool lowPriority { get; set; }
        public string label { get; set; }
        public int count { get; set; }
    }
}
