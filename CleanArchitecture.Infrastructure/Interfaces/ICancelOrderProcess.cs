using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface ICancelOrderProcess
    {
        Task<BizResponse> ProcessCancelOrderAsync(CancelOrderRequest Req,string accessToken);
    }
}
