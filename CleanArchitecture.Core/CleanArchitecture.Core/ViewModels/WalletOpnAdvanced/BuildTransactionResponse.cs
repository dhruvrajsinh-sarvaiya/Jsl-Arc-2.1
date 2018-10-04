using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class BuildTransactionResponse : BizResponseClass
    {
        public string txHex { get; set; }
        public TxInfo txInfo { get; set; }
        public FeeInfo feeInfo { get; set; }
        //public class Unspent
        //{
        //    public int chain { get; set; }
        //    public int index { get; set; }
        //    public string redeemScript { get; set; }
        //    public string id { get; set; }
        //    public string address { get; set; }
        //    public int value { get; set; }
        //}

        //public class TxInfo
        //{
        //    public int nP2SHInputs { get; set; }
        //    public int nSegwitInputs { get; set; }
        //    public int nOutputs { get; set; }
        //    public List<Unspent> unspents { get; set; }
        //    public List<string> changeAddresses { get; set; }
        //}

        //public class FeeInfo
        //{
        //    public int size { get; set; }
        //    public int fee { get; set; }
        //    public int feeRate { get; set; }
        //    public int payGoFee { get; set; }
        //    public string payGoFeeString { get; set; }
        //}

        //public class BuildTransactionRootObject : BizResponseClass
        //{
        //    public string txHex { get; set; }
        //    public TxInfo txInfo { get; set; }
        //    public FeeInfo feeInfo { get; set; }
        //}
    }
}
