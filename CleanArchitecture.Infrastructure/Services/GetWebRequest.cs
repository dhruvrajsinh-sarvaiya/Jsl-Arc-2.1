using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.DTOClasses;
using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Infrastructure.Interfaces;

namespace CleanArchitecture.Infrastructure.Services
{
    class GetWebRequest : IGetWebRequest
    {
        readonly ICommonRepository<RouteConfiguration> _routeRepository;
        readonly ICommonRepository<ThirPartyAPIConfiguration> _thirdPartyCommonRepository;
        readonly ICommonRepository<ProviderConfiguration> _providerRepository;

        public  GetWebRequest(ICommonRepository<RouteConfiguration> routeRepository, ICommonRepository<ThirPartyAPIConfiguration> thirdPartyCommonRepository,
              ICommonRepository<ProviderConfiguration> providerRepository)
        {
            _thirdPartyCommonRepository = thirdPartyCommonRepository;
            _routeRepository = routeRepository;
            _providerRepository = providerRepository;
        }

        public ThirdPartyAPIRequest MakeWebRequest(long routeID, long thirdpartyID, long serproID)
        {
            RouteConfiguration routeConfiguration;
            ThirPartyAPIConfiguration thirdPartyAPIConfiguration;
            ProviderConfiguration providerConfiguration;
            ThirdPartyAPIRequest thirdPartyAPIRequest = new ThirdPartyAPIRequest ();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            thirdPartyAPIConfiguration = _thirdPartyCommonRepository.GetById(thirdpartyID);
            routeConfiguration = _routeRepository.GetById(routeID);

            thirdPartyAPIRequest.RequestURL = thirdPartyAPIConfiguration.APISendURL;
            thirdPartyAPIRequest.RequestBody = thirdPartyAPIConfiguration.APIRequestBody;

            if (thirdPartyAPIConfiguration == null || routeConfiguration == null)
            {
                return thirdPartyAPIRequest;
            }

            keyValuePairs.Add("##SMSCODE##", routeConfiguration.OpCode);

            foreach (KeyValuePair<string, string> item in keyValuePairs)
            {
                thirdPartyAPIRequest.RequestURL = thirdPartyAPIRequest.RequestURL.Replace(item.Key, item.Value);
                thirdPartyAPIRequest.RequestBody = thirdPartyAPIRequest.RequestBody.Replace(item.Key, item.Value);
            }
            return thirdPartyAPIRequest;        
    }
    }
}
