using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IWebApiRouteData<in TRequest, TResponse>
    {
        Task<TResponse> GetAPIConfigurationAsync(TRequest Request);
    }

    public interface IWebApiSendRequest
    {
        Task<String> SendAPIRequestAsync(string Url, string Request, string MethodType = "POST");
        //Task<TResponse> GetTemplateConfigurationAsync(TRequest Request);
    }

    public interface IWebApiParseResponse<TResponse>
    {        
        Task<TResponse> TransactionParseResponse(string TransactionResponse,long ThirPartyAPIID);       
      
    }
   
}