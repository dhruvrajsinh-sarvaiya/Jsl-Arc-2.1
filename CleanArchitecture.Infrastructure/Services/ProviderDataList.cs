using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    //Take Transaction Service Provider Data
    class ProviderDataList : IProviderDataList<TransactionApiConfigurationRequest, TransactionProviderResponse>
    {
        public IEnumerable<TransactionProviderResponse> GetProviderDataList(TransactionApiConfigurationRequest Request)
        {            
            throw new NotImplementedException();
        }

    }
}
