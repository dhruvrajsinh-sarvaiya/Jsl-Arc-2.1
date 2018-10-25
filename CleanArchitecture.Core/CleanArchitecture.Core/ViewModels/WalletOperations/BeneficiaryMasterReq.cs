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
        [Required(ErrorMessage = "Please Enter Required Parameter")]
        public string AccWalletID { get; set; }
        [Required(ErrorMessage = "Please Enter Required Parameter")]
        public string BeneAddress { get; set; }
        public string Name { get; set; }
    }

    public class BeneficiaryMasterRes
    {
        public string Address { get; set; }
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public short? IsWhiteListed { get; set; }
    }
    public class BeneficiaryResponse
    {
        public List<BeneficiaryMasterRes> Beneficiaries { get; set; }
        public BizResponseClass BizResponse { get; set; }
    }
}
