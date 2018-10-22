using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class ListWalletTransfersRequest
    {
        [Required]
        [JsonProperty(PropertyName = "Id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "PrevId")]
        public string prevId { get; set; }
        [JsonProperty(PropertyName = "AllTokens")]
        public bool allTokens { get; set; }
        [JsonProperty(PropertyName = "IncludeHex")]
        public bool includeHex { get; set; }
        [JsonProperty(PropertyName = "SearchLabel")]
        public string searchLable { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string type;
        //public string searchLable { get; set; } for 
       
    }
}
