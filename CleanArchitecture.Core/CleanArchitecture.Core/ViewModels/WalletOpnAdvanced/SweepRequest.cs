using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class SweepRequest
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string walletPassphrase { get; set; }

        [Required]
        public string address { get; set; }

        public string xprv { get; set; }
        public string otp { get; set; }
        public int feeRate { get; set; }
        public int maxFeeRate { get; set; }
        public int feeTxConfirmTarget { get; set; }
        public int gasPrice { get; set; }
        public int sequenceId { get; set; }
        public int lastLedgerSequence { get; set; }
        public string ledgerSequenceDelta { get; set; }     
    }
}
