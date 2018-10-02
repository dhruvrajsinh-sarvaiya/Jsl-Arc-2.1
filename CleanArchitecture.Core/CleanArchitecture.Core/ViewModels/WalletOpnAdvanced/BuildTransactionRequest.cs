using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class BuildTransactionRequest
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string coin { get; set; }

        public List<Recipient> recipients { get; set; }
        public class Recipient
        {
            public string address { get; set; }
            public int amount { get; set; }
        }   
    }
}
