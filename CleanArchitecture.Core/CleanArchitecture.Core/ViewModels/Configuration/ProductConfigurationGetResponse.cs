using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProductConfigurationGetResponse : BizResponseClass
    {
        public ProductConfigrationGetInfo response { get; set; }
    }
    public class ProductConfigrationGetInfo
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public long StateID { get; set; }
        public string StateName { get; set; }
    }
}
