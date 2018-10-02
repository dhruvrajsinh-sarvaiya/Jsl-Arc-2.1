using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class SignTransactionRequest
    {
        

        //public class Recipient
        //{
        //    public string address { get; set; }
        //    public int amount { get; set; }
        //}   
        //public class Key
        //{
        //    public string address { get; set; }
        //    public string data { get; set; }
        //    public int amount { get; set; }
        //}
        public class Unspent
        {
            public int chain { get; set; }
            public int index { get; set; }
            public string redeemScript { get; set; }
            public string id { get; set; }
            public string address { get; set; }
            public int value { get; set; }
        }
        public class TxInfo
        {
            public int nP2SHInputs { get; set; }
            public int nSegwitInputs { get; set; }
            public int nOutputs { get; set; }
            public List<Unspent> unspents { get; set; }
            public List<string> changeAddresses { get; set; }
        }

        public class FeeInfo
        {
            public int size { get; set; }
            public int fee { get; set; }
            public int feeRate { get; set; }
            public int payGoFee { get; set; }
            public string payGoFeString { get; set; }
        }

        public class TxPrebuild
        {
            public string txHex { get; set; }
            public TxInfo txInfo { get; set; }
            public FeeInfo feeInfo { get; set; }
        }
        public class Recipient
        {
            public string address { get; set; }
            public int amount { get; set; }
        }
        public class SignTransactionRootObject
        {
            [Required]
            public string id { get; set; }

            [Required]
            public string coin { get; set; }

            [Required]
            public TxPrebuild txPrebuild { get; set; }
            public string coldDerivationSeed { get; set; }
            public string walletPassphrase { get; set; }
            public string prv { get; set; }
            public List<Recipient> recipients { get; set; }
            //public List<Key> key { get; set; }
            //public List<Key> keychain { get; set; }
        }

    }
}
