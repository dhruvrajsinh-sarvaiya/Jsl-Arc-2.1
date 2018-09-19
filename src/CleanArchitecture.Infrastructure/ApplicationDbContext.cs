using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {        
        public DbSet<ServiceConfiguration> ServiceConfigurations { get; set; }
        public DbSet<WalletTypeMaster> WalletTypeMasters { get; set; }
        public DbSet<WalletMaster> WalletMasters { get; set; }
        public DbSet<WalletOrder> WalletOrders { get; set; }
        public DbSet<TransactionAccount> TransactionAccounts { get; set; }
        public DbSet<WalletLedger> WalletLedger { get; set; }


    }
}
