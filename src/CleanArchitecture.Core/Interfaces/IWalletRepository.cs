using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IWalletRepository<T> where T : BizBase
    {
        T GetById(long id);
    }
}
