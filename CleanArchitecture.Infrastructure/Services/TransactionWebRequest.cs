using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    class TransactionWebRequest : IWebApiConfiguration<TransactionApiConfigurationRequest, TransactionApiConfigurationResponse>
    {
        public Task SendAPIRequestAsync(string Url, string Request, string MethodType = "POST")
        {
            return Task.FromResult(0);
        }

        public Task<TransactionApiConfigurationResponse> TransactionParseResponse(string TransactionResponse)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionApiConfigurationResponse> GetAPIConfigurationAsync(TransactionApiConfigurationRequest Request)
        {
            throw new NotImplementedException();
        }
    }
}
