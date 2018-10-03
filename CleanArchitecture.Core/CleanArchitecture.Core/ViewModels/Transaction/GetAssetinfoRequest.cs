using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetAssetInfoRequest
    {
        public string info { get; set; }
        public string aclass { get; set; }
        public string asset { get; set; }
    }
}
