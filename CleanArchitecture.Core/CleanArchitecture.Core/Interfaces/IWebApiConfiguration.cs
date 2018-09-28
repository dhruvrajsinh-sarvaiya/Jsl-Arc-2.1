using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IProviderDataList<in TRequest, TResponse>
    {
        IEnumerable<TResponse> GetProviderDataList(TRequest Request);
    }

    public interface IWebApiData<TResponse>
    {
        TResponse GetAPIConfiguration(long ThirPartyAPIID);
    }

    public interface IWebApiSendRequest
    {
        Task<String> SendAPIRequestAsync(string Url, string Request, string MethodType = "POST");
        //Task<TResponse> GetTemplateConfigurationAsync(TRequest Request);
    }

    public interface IWebApiParseResponse<TResponse>
    {        
        TResponse TransactionParseResponse(string TransactionResponse,long ThirPartyAPIID);       
      
    }
   
}