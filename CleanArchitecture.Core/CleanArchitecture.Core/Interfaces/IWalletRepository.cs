using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IWalletRepository
    {
        //T GetById(long id); moved to icommonrepository
        //List<T> List();
        //T Add(T entity);
        //void Update(T entity);
        //void Delete(T entity);
        //T AddProduct(T entity);
        TradeBitGoDelayAddresses GetUnassignedETH();
        bool WalletOperation(WalletLedger wl1, WalletMaster wm1, TransactionAccount ta1, WalletLedger wl2, WalletMaster wm2, TransactionAccount ta2);
    }
}
