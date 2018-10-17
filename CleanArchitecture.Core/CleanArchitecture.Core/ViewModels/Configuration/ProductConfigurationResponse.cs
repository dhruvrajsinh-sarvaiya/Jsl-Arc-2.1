using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProductConfigurationResponse : BizResponseClass
    {
        public ProductConfigurationInfo response { get; set; }
    }
    public class ProductConfigurationInfo
    {
        public long ProductId { get; set; }
    }
}
