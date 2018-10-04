using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.ApiModels;


namespace CleanArchitecture.Core.Interfaces
{
    public interface IWebApiRepository
    {
        WebApiConfigurationResponse GetThirdPartyAPIData(long ThirPartyAPIID);

        GetDataForParsingAPI GetDataForParsingAPI(long ThirPartyAPIID);

        //ntrivedi fetch route
        List<TransactionProviderResponse> GetProviderDataList(TransactionApiConfigurationRequest Request);

    }
}
