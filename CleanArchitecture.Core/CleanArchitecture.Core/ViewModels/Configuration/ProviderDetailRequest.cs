﻿using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProviderDetailRequest : ProviderDetailViewModel  
    {
    }
    public class ProviderDetailResponce : BizResponseClass
    {
        public ProviderDetailGetAllResponce response { get; set; }
    }
    public class ProviderDetailResponceList : BizResponseClass
    {
        public IEnumerable<ProviderDetailGetAllResponce> response { get; set; }
    }
    public class ProviderDetailGetAllResponce
    {
        public long Id { get; set; }
        public ServiceProviderViewModel Provider { get; set;}
        public ProviderTypeViewModel ProviderType { get; set; }
        public AppTypeViewModel AppType { get; set; }
        public long TrnType { get; set; }
        ///public object  TrnType;
        public LimitViewModel Limit { get; set; }
        //public Object Limit { get; set; }
        public DemonconfigurationViewModel DemonConfiguration { get; set; }
        public ProviderConfigurationViewModel ProviderConfiguration { get; set; }
        //public ThirdPartyViewModel thirdParty { get; set; }
        public ThirdPartyAPIConfigViewModel   thirdParty { get; set; }

    }
}
