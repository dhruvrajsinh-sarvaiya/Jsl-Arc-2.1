using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    
    public interface IWalletService
    {
        decimal GetUserBalance(int walletId);

        bool CreditWallet(int walletId, ref decimal PostBal);

        bool WalletBalanceCheck(decimal amount,long walletId);

        bool DebitWallet(int walletId,ref decimal PostBal);

    }
}
