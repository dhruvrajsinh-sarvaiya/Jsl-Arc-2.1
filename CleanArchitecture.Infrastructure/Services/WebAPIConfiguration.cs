using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.Data.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;

namespace CleanArchitecture.Infrastructure.Services
{   
    //Take Transaction Route Data
    public class TransactionWebAPIConfiguration : IWebApiData
    {
        private readonly WebApiDataRepository _webapiDataRepository;
        public TransactionWebAPIConfiguration(WebApiDataRepository webapiDataRepository)
        {
            _webapiDataRepository = webapiDataRepository;
        }

        public WebApiConfigurationResponse GetAPIConfiguration(long ThirPartyAPIID)
        {
            return _webapiDataRepository.GetThirdPartyAPIData(ThirPartyAPIID);            
        }            
    }

    //Now same as transaction data
    //Take SMS Route Data
    //class SMSWebAPIConfiguration : IWebApiData<WebApiConfigurationResponse>
    //{
    //    public WebApiConfigurationResponse GetAPIConfiguration(long ThirPartyAPIID)
    //    {
    //        throw new NotImplementedException();
    //    }     
    //} 
}
