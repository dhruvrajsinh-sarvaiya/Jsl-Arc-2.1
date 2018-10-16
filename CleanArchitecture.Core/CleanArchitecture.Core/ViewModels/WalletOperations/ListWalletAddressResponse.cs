using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class ListWalletAddressResponse : BizResponseClass
    {
        public List<AddressMasterResponse> AddressList { get; set; }
    }
}
