using CleanArchitecture.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    class GetBalanceRequest : BizRequestClass
    {
        public long OWalletID {get;set;}
    }

    class GetBalanceResponse : BizResponseClass
    {
        public decimal Balance { get; set; }

        public byte isValid { get; set; }

    }


}
