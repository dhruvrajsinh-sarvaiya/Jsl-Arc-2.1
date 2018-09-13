using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Entities;



namespace CleanArchitecture.Core.Events
{
    public class TransactionAccountSetAsSettled : BaseDomainEvent
    {
        public TransactionAccount SettledItem { get; set; }
        public TransactionAccountSetAsSettled(TransactionAccount settledItem)
        {
            SettledItem = settledItem;
        }
    }
}
