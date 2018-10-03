using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class ListWalletTransfersRequest
    {
        [Required]
        public string id { get; set; }
        public string prevId { get; set; }
        public bool allTokens { get; set; }
        public bool includeHex { get; set; }
        public string searchLable { get; set; }
        public string type;
        //public string searchLable { get; set; } for 
       
    }
}
