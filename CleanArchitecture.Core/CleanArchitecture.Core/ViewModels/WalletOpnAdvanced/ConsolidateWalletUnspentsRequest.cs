using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ConsolidateWalletUnspentsRequest
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string walletPassphrase { get; set; }

        public int numUnspentsToMake { get; set; }
        public int limit { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public int minHeight { get; set; }
        public int feeRate { get; set; }
        public int feeTxConfirmTarget { get; set; }
        public int maxFeePercentage { get; set; }
        public int minConfirms { get; set; }
        public bool enforceMinConfirmsForChange { get; set; }
    }
}
