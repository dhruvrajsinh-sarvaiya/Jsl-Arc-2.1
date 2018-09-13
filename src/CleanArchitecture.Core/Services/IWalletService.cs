using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services
{
    
    public interface IWalletService<T> where T : BizBase
    {
        bool GetUserBalance(int walletId);

    }
}
