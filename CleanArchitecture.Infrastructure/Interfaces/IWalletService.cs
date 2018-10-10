using CleanArchitecture.Core.Entities;
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

        string GenerateAddress(long walletid);

        //vsolanki 8-10-2018
        IEnumerable<WalletTypeMaster> GetWalletTypeMaster();

    }
}
