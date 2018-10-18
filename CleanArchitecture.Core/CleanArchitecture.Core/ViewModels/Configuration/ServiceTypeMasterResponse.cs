using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceTypeMasterResponse : BizResponseClass
    {
        public List<ServiceTypeMasterInfo> response { get; set; }
    }
    public class ServiceTypeMasterInfo
    {
        public long Id { get; set; }
        public string SerTypeName { get; set; }
    }
}
