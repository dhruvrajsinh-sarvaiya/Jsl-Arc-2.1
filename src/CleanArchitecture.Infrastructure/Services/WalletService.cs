using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletService : IWalletService
    {
        public bool CreditWallet(int walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }

        public bool DebitWallet(int walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }

        public bool GetUserBalance(int walletId)
        {
            throw new NotImplementedException();
        }
    }
}
