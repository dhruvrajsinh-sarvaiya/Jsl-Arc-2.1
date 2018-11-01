using CleanArchitecture.Core.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISettlementRepository<T>
    {
        Task<T> PROCESSSETLLEMENT(T _Resp, TradeBuyRequest TradeBuyRequestObj,ref List<long> HoldTrnNos);
    }
}
