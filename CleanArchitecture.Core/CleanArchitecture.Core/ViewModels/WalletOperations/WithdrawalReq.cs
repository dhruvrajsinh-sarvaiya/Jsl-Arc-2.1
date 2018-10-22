using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WithdrawalReq
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4220")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid  Parameters,4221")]
        [JsonProperty(PropertyName = "SourceAddress")]
        public string sourceAddress { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4215")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid  Parameters,4216")]
        [JsonProperty(PropertyName = "DestinationAddress")]
        public string destinationAddress { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4217")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid  Parameters,4218")]
        [JsonProperty(PropertyName = "AddressLabel")]
        public string addressLabel { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4219")]
        [Range(0,9999999999.99999999,ErrorMessage = "1,Please Enter Valid  Parameters,4222")]
        [JsonProperty(PropertyName = "Amount")]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal amount { get; set; }

        // Temporarily Removed
        //[Required]
        //public string walletPassphrase { get; set; } 




        //public string prv { get; set; }
        //public int numBlocks { get; set; }
        //public int feeRate { get; set; }
        //public string comment { get; set; }
        //public string[] unspents { get; set; }
        //public int minConfirms { get; set; }
        //public bool enforceMinConfirmsForChange { get; set; }
        //public int targetWalletUnspents { get; set; }
        //public bool noSplitChange { get; set; }
        //public int minValue { get; set; }
        //public int maxValue { get; set; }
        //public int maxFeeRate { get; set; }
        //public int gasPrice { get; set; }
        //public int gasLimit { get; set; }
        //public int sequenceId { get; set; }
        //public bool segwit { get; set; }
        //public int lastLedgerSequence { get; set; }
        //public string ledgerSequenceDelta { get; set; }
        //public string changeAddress { get; set; }
        //public string data { get; set; }
    }
}
