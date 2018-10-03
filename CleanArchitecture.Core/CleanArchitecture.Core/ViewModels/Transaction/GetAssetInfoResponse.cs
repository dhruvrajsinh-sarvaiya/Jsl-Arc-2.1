using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetAssetInfoResponse
    {
        public string asset_name { get; set; }

        public AssetInformation asset_detail { get;set; }
    }
    public class AssetInformation
    {
        public string altname { get; set; }
        public string aclass { get; set; }
        public int decimals { get; set; }
        public int display_decimals { get; set; }
    }
}
