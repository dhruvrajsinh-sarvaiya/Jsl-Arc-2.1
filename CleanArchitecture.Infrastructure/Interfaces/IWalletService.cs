using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    
    public interface IWalletService
    {
        decimal GetUserBalance(long walletId);

        bool WalletBalanceCheck(decimal amount, long walletId);

        bool IsValidWallet(long walletId);      
        
    }
}
