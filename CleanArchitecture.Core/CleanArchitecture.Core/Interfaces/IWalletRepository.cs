using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IWalletRepository<T> where T : BizBase
    {
        T GetById(long id);
        List<T> List();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T AddProduct(T entity);
        TradeBitGoDelayAddresses GetUnassignedETH();
    }
}
