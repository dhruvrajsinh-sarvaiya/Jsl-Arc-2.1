using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.DTOClasses;
using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Infrastructure.Interfaces;
using System.Net;

namespace CleanArchitecture.Infrastructure.Services
{
    public class GetWebRequest : IGetWebRequest
    {
        readonly ICommonRepository<RouteConfiguration> _routeRepository;
        readonly ICommonRepository<ThirdPartyAPIConfiguration> _thirdPartyCommonRepository;
        readonly ICommonRepository<ServiceProviderDetail> _serviceProviderDetail;
        readonly ICommonRepository<ServiceProConfiguration> _ServiceProConfiguration;
        //readonly ICommonRepository<ProviderConfiguration> _providerRepository;

        public  GetWebRequest(ICommonRepository<RouteConfiguration> routeRepository, ICommonRepository<ThirdPartyAPIConfiguration> thirdPartyCommonRepository,
              //ICommonRepository<ProviderConfiguration> providerRepository, 
              ICommonRepository<ServiceProConfiguration> ServiceProConfiguration,
              ICommonRepository<ServiceProviderDetail> serviceProviderDetail)
        {
            _thirdPartyCommonRepository = thirdPartyCommonRepository;
            _routeRepository = routeRepository;
            //_providerRepository = providerRepository;
            _ServiceProConfiguration = ServiceProConfiguration;
            _serviceProviderDetail = serviceProviderDetail;
        }

        public ThirdPartyAPIRequest MakeWebRequest(long routeID, long thirdpartyID, long serproDetailID)
        {
            RouteConfiguration routeConfiguration;
            ThirdPartyAPIConfiguration thirdPartyAPIConfiguration;
            //ProviderConfiguration providerConfiguration;
            //ServiceProviderDetail ServiceProviderDetail;
            ServiceProConfiguration ServiceProConfiguration;
            ThirdPartyAPIRequest thirdPartyAPIRequest = new ThirdPartyAPIRequest ();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            WebHeaderCollection keyValuePairsHeader = new WebHeaderCollection();


            thirdPartyAPIConfiguration = _thirdPartyCommonRepository.GetById(thirdpartyID);
            var SerProconfigID= _serviceProviderDetail.GetSingle(item => item.Id == serproDetailID).ServiceProConfigID;
            ServiceProConfiguration = _ServiceProConfiguration.GetSingle(item=>item.Id== SerProconfigID);
            
            routeConfiguration = _routeRepository.GetById(routeID);

            thirdPartyAPIRequest.RequestURL = thirdPartyAPIConfiguration.APISendURL;
            thirdPartyAPIRequest.RequestBody = thirdPartyAPIConfiguration.APIRequestBody;

            if (thirdPartyAPIConfiguration == null || routeConfiguration == null)
            {
                return thirdPartyAPIRequest;
            }

            keyValuePairs.Add("##SMSCODE##", routeConfiguration.OpCode);
            keyValuePairs.Add("##WALLETID##", routeConfiguration.ProviderWalletID); 


            foreach (KeyValuePair<string, string> item in keyValuePairs)
            {
                thirdPartyAPIRequest.RequestURL = thirdPartyAPIRequest.RequestURL.Replace(item.Key, item.Value);
                thirdPartyAPIRequest.RequestBody = thirdPartyAPIRequest.RequestBody.Replace(item.Key, item.Value);
            }
            if(thirdPartyAPIConfiguration.AuthHeader == "RPC")
            {
                string authInfo = ServiceProConfiguration.UserName + ":" + ServiceProConfiguration.Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                authInfo = "Basic " + authInfo;
                keyValuePairsHeader.Add("Authorization", authInfo);                
            }
            thirdPartyAPIRequest.keyValuePairsHeader = keyValuePairsHeader;

            thirdPartyAPIRequest.DelayAddress = routeConfiguration.IsDelayAddress;
            thirdPartyAPIRequest.walletID = routeConfiguration.ProviderWalletID;
            return thirdPartyAPIRequest;        
    }
    }
}
