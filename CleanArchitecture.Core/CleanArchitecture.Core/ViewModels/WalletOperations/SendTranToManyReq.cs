using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class SendTranToManyReq
    {
        [Required]
        public List<Recipient> recipients { get; set; }
        [Required]
        public string walletPassphrase { get; set; }
        public string prv { get; set; }
        public int numBlocks { get; set; }
        public int feeRate { get; set; }
        public string comment { get; set; }
        public List<Unspents> unspents { get; set; }
        public int minConfirms { get; set; }
        public bool enforceMinConfirmsForChange { get; set; }
        public int targetWalletUnspents { get; set; }
        public bool noSplitChange { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public int maxFeeRate { get; set; }
        public int gasPrice { get; set; }
        public int gasLimit { get; set; }
        public int sequenceId { get; set; }
        public bool segwit { get; set; }
        public int lastLedgerSequence { get; set; }
        public string ledgerSequenceDelta { get; set; }
        public string changeAddress { get; set; }
    }
    public class Recipient
    {
        public string address { get; set; }
        public int amount { get; set; }
    }

    public class Unspents
    {
        public string prevTxId { get; set; }
        public string nOutput { get; set; }
    }
}
