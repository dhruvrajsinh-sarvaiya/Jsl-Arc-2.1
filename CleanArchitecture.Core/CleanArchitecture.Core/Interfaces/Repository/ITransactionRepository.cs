using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Repository
{
    public interface ITransactionRepository
    {
        IQueryable GetTradePairById(long PairID);
    }
}
