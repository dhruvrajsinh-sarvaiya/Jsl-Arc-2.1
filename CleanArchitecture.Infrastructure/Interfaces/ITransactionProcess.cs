using CleanArchitecture.Infrastructure.DTOClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface ITransactionProcess
    {
        Task<BizResponse> ProcessNewTransactionAsync(NewTransactionRequestCls Req);
    }
}
