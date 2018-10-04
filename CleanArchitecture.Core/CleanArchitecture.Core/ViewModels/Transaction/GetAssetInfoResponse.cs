using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetAssetInfoResponse : BizResponseClass
    {
        public List<GetAssetInfoList> response = new List<GetAssetInfoList>();
    }
    public class AssetInformation
    {
        public string altname { get; set; }
        public string aclass { get; set; }
        public int decimals { get; set; }
        public int display_decimals { get; set; }
    }
    public class GetAssetInfoList
    {
        public string asset_name { get; set; }
        public AssetInformation asset_detail { get; set; }
    }
}
