using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class GetWalletAddressReq
    {
        [Required]
        [JsonProperty(PropertyName = "Address")]
        public string walletId { get; set; }
        [Required]
        [JsonProperty(PropertyName = "AddressOrId")]
        public string addressOrId { get; set; }
    }
}
