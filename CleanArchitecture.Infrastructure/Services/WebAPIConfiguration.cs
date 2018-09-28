using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Infrastructure.Services
{   
    //Take Transaction Route Data
    class TransactionWebAPIConfiguration : IWebApiData<WebApiConfigurationResponse>
    {       
        public WebApiConfigurationResponse GetAPIConfiguration(long ThirPartyAPIID)
        {
            throw new NotImplementedException();
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
