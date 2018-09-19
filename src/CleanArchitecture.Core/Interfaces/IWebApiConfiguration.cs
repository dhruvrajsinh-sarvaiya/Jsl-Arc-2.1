using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IWebApiConfiguration<in TRequest,TResponse>
    {
        Task SendAPIRequestAsync(string Url, string Request, string MethodType="POST");
        Task<TResponse> TransactionParseResponse(string TransactionResponse);
        Task<TResponse> GetAPIConfigurationAsync(TRequest Request);
        //Task<TResponse> GetTemplateConfigurationAsync(TRequest Request);
    }
}