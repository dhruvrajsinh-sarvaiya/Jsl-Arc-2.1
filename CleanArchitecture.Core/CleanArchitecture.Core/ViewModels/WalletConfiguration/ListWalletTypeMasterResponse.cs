using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletConfiguration
{
    public class ListWalletTypeMasterResponse: BizResponseClass
    {
        public IEnumerable<WalletTypeMaster> walletTypeMasters { get; set; }
    }
}
