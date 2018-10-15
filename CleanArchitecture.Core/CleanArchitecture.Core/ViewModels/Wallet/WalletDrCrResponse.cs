using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class WalletDrCrResponse : BizResponseClass
    {
        public long TrnNo { get; set; }

        public byte Status { get; set; }

        public string StatusMsg { get; set; }
    }
}
