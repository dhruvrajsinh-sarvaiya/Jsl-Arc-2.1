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
        String SendAPIRequestAsync(string Url, string Request, string ContentType, int Timeout,Dictionary<string,string> headerDictionary ,string MethodType = "POST");
        //Task<TResponse> GetTemplateConfigurationAsync(TRequest Request);
        // ntrivedi 04-10-2018 do not need async method due to single wait process
    }

    public interface IWebApiParseResponse<TResponse>
    {        
        TResponse TransactionParseResponse(string TransactionResponse,long ThirPartyAPIID);       
      
    }
   
}