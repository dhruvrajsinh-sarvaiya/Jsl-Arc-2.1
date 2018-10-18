using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProductConfigurationGetAllResponse : BizResponseClass
    {
        public List<ProductConfigrationGetInfo> response { get; set; }
    }
}
