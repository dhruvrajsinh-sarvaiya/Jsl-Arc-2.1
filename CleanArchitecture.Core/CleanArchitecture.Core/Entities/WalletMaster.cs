using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class WalletMaster : BizBase
    {      

        public decimal Balance { get; set; }
        
        public long WalletTypeID { get; set; }

        public bool IsValid { get; set; }

        public void CreditBalance(decimal amount)
        {
            Balance = Balance + amount;
            Events.Add(new WalletCrEvent<WalletMaster>(this));

        }
        public void DebitBalance(decimal amount)
        {
            Balance = Balance - amount;
            Events.Add(new WalletDrEvent<WalletMaster>(this));

        }

    }

}
