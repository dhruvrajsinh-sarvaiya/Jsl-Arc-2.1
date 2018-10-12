using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.ViewModels.WalletOperations;
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

        CreateWalletAddressRes GenerateAddress(long walletid,string coin);

        //vsolanki 8-10-2018
        IEnumerable<WalletTypeMaster> GetWalletTypeMaster();

        //vsolanki 10-10-2018
        CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId);
    }
}
