using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Helpers;
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

        public ThirdPartyAPIRequest MakeWebRequest(long routeID, long thirdpartyID, long serproDetailID,TransactionQueue TQ)
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

            keyValuePairs.Add("#OPERATORCODE#", routeConfiguration.OpCode);
            //keyValuePairs.Add("#WALLETID#", routeConfiguration.ProviderWalletID);
            keyValuePairs.Add("#ORGADDRESS#", routeConfiguration.ProviderWalletID);// Org Wallet Address/ID  

            if (!string.IsNullOrEmpty(thirdPartyAPIConfiguration.TimeStamp))
                keyValuePairs.Add("#TIMESTAMP#", Helpers.UTC_To_IST().ToString(thirdPartyAPIConfiguration.TimeStamp));

            if (TQ != null)//Rita 25-10-2018 incase of transation
            {
                keyValuePairs.Add("#SMSCODE#", TQ.SMSCode);
                keyValuePairs.Add("#TRANSACTIONID#", TQ.Id.ToString());
                keyValuePairs.Add("#AMOUNT#", TQ.Amount.ToString());                
                keyValuePairs.Add("#USERADDRESS#", TQ.TransactionAccount);
                keyValuePairs.Add("#CONVERTEDAMT#", (routeConfiguration.ConvertAmount * TQ.Amount).ToString());
            }
            else//In case of Wallet Operation
            {

            }

            //Rushabh 11-10-2018 For Authorization Header
            if (thirdPartyAPIConfiguration.AuthHeader != string.Empty)
            {

                foreach (string mainItem in thirdPartyAPIConfiguration.AuthHeader.Split("###"))
                {

                    string[] item = mainItem.Split(":");
                    //thirdPartyAPIRequest.RequestURL = thirdPartyAPIRequest.RequestURL.Replace(item[0], item[1]);
                    //thirdPartyAPIRequest.RequestBody = thirdPartyAPIRequest.RequestBody.Replace(item[0], item[1]);
                    string authInfo = ServiceProConfiguration.UserName + ":" + ServiceProConfiguration.Password;
                    item[1] = item[1].Replace("#BASIC#", Convert.ToBase64String(Encoding.Default.GetBytes(authInfo)));
                    keyValuePairsHeader.Add(item[0], item[1]);

                }
            }

            foreach (KeyValuePair<string, string> item in keyValuePairs)
            {
                thirdPartyAPIRequest.RequestURL = thirdPartyAPIRequest.RequestURL.Replace(item.Key, item.Value);
                if(thirdPartyAPIRequest.RequestBody != null)
                {
                    thirdPartyAPIRequest.RequestBody = thirdPartyAPIRequest.RequestBody.Replace(item.Key, item.Value);
                }                
            }
            //Rita 25-10-2018 added in common dynamic header part
            //if(thirdPartyAPIConfiguration.AuthHeader == "RPC")
            //{
            //    string authInfo = ServiceProConfiguration.UserName + ":" + ServiceProConfiguration.Password;
            //    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            //    authInfo = "Basic " + authInfo;
            //    keyValuePairsHeader.Add("Authorization", authInfo);                
            //}
            thirdPartyAPIRequest.keyValuePairsHeader = keyValuePairsHeader;

            thirdPartyAPIRequest.DelayAddress = routeConfiguration.IsDelayAddress;
            thirdPartyAPIRequest.walletID = routeConfiguration.ProviderWalletID;
            return thirdPartyAPIRequest;        
    }
    }
}
