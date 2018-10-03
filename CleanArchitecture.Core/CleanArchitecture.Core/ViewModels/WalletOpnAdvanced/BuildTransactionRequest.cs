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
        
        public List<Recipient1> recipients { get; set; }
       
    } 
    public class Recipient1
    {
        public string address { get; set; }
        public int amount { get; set; }
    }
}
