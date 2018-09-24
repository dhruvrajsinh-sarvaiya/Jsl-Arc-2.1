using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Events
{
    class BalanceCheckEvent : BaseDomainEvent
    {
        public WalletMaster WalletObj { get; set; }

        //public WalletMaster CheckUserBalance(long WalletID)
        //{
            
        //}

    }
}
