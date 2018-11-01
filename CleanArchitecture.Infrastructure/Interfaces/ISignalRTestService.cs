using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface ISignalRTestService
    {
        void MarkTransactionHold(long ID, string Token);
    }
}
