using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class BeneficiaryMasterReq
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4223")]
        public string AccWalletID { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4224")]
        public string BeneAddress { get; set; }
        public string Name { get; set; }
    }

    public class BeneficiaryUpdateReq
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4224")]
        public long BenefifiaryID { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4231")]
        public string Name { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4228")]
        public int WhitelistingBit { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4232")]
        public short Status { get; set; }
    }
    public class BulkBeneUpdateReq
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4225")]
        public long ID { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4226")]
        public short WhitelistingBit { get; set; }
    }

    public class BeneficiaryMasterRes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long BeneficiaryID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CoinName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public short? IsWhiteListed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
    }
    public class BeneficiaryResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<BeneficiaryMasterRes> Beneficiaries { get; set; }
        public BizResponseClass BizResponse { get; set; }
    }
}
