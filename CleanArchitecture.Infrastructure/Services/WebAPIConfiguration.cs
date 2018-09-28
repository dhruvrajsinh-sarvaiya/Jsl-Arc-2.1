using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Infrastructure.Services
{
    class TransactionWebAPIConfiguration : IWebApiRouteData<TransactionApiConfigurationRequest, TransactionApiConfigurationResponse>
    {
        //Take Transaction Route Data
        public Task<TransactionApiConfigurationResponse> GetAPIConfigurationAsync(TransactionApiConfigurationRequest Request)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendAPIRequestAsync(string Url, string Request, string MethodType = "POST")
        {
            string sdf = "";
            return Task.FromResult(sdf);
        }       
    }

    //Take SMS Route Data
    class SMSWebAPIConfiguration : IWebApiRouteData<TransactionApiConfigurationRequest, TransactionApiConfigurationResponse>
    {
        public Task<TransactionApiConfigurationResponse> GetAPIConfigurationAsync(TransactionApiConfigurationRequest Request)
        {
            throw new NotImplementedException();
        }     
    }

    //For All type of Web Request
    class WebAPISendRequestAsync : IWebApiSendRequest
    {
        public Task<string> SendAPIRequestAsync(string Url, string Request, string MethodType = "POST")
        {
            string Response = "";
            return Task.FromResult(Response);
        }
    }
    //Common Parsing method Implement Here
    class WebApiParseResponse : IWebApiParseResponse<WebAPIParseResponse>
    {
        public Task<WebAPIParseResponse> TransactionParseResponse(string TransactionResponse, long ThirPartyAPIID)
        {
            throw new NotImplementedException();
        }
    }
}
