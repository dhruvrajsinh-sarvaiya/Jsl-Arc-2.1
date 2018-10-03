using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ListWalletRequest
    {
        public int limit { get; set; }
        public string prevId { get; set; }  
        public bool allTokens { get; set; }
    }
}
