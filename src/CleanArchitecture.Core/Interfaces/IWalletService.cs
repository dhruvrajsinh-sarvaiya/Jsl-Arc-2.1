using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    
    public interface IWalletService<T> where T : BizBase
    {
        bool GetUserBalance(int walletId);

        bool CreditWallet(int walletId, ref decimal PostBal);

        bool DebitWallet(int walletId,ref decimal PostBal);

    }
}
