using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletConfiguration
{
    public class WalletTypeMasterResponse:BizResponseClass
    {
        public WalletTypeMaster walletTypeMaster { get; set; }
    }
}
