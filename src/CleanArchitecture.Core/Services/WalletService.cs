using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Services
{   

    public class WalletService<T> : IWalletService<T> where T : BizBase
{ 
        private readonly IRepository<WalletMaster> _todoRepository;

        bool IWalletService<T>.GetUserBalance(int walletId)
        {
            throw new NotImplementedException();
        }
    }
}
