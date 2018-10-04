using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class SignTransactionRequest
    {
        [Required]
        public TransactionRootObject root { get; set; }
        public string prv { get; set; }
        public string coldDerivationSeed { get; set; }
        public List<KeyChainRootObject> keychain { get; set; }
        public string walletPassphrase { get; set; }
        //public recipientsRootObject recipients { get; set; }
        public List<RecipientRoot> recipients { get; set; }
    }
    public class TxPrebuild
    {
        public string txHex { get; set; }
        public TxInfo txInfo { get; set; }
        public FeeInfo feeInfo { get; set; }
    }

    public class Key
    {
        public string encryptedPrv { get; set; }
        public string id { get; set; }
        public string pub { get; set; }
        public List<string> users { get; set; }
    }

    public class KeyChainRootObject
    {
        public List<Key> keys { get; set; }
        public int limit { get; set; }
        public string nextBatchPrevId { get; set; }
    }

    public class TransactionRootObject
    {
        public TxPrebuild txPrebuild { get; set; }
        public string prv { get; set; }
    }

    public class RecipientRoot
    {
        public string address { get; set; }
        public int amount { get; set; }
    }

    //public class recipientsRootObject
    //{
    //    public List<Recipient> recipients { get; set; }
    //}
    //public class FeeInfo
    //{
    //    public int size { get; set; }
    //    public int fee { get; set; }
    //    public int feeRate { get; set; }
    //    public int payGoFee { get; set; }
    //    public string payGoFeeString { get; set; }
    //}
    //public class TxInfo
    //{
    //    public int nP2SHInputs { get; set; }
    //    public int nSegwitInputs { get; set; }
    //    public int nOutputs { get; set; }
    //    public List<Unspent> unspents { get; set; }
    //    public List<string> changeAddresses { get; set; }
    //}
    //public class Unspent
    //{
    //    public int chain { get; set; }
    //    public int index { get; set; }
    //    public string redeemScript { get; set; }
    //    public string id { get; set; }
    //    public string address { get; set; }
    //    public int value { get; set; }
    //}

}
