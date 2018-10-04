using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class FreezeWalletResponse : BizResponseClass
    {
        public DateTime time { get; set; }
        public DateTime expires { get; set; }
    }
}
