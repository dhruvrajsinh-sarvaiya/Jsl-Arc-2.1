using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    
    public interface IWalletService
    {
        decimal GetUserBalance(long walletId);

        bool CreditWallet(long walletId, ref decimal PostBal);

        bool WalletBalanceCheck(decimal amount, long walletId);

        bool DebitWallet(long walletId, ref decimal PostBal);

        bool IsValidWallet(long walletId);

        
    }
}
