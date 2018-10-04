using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ListWalletTransactionResponse : BizResponseClass
    {
        public List<Transaction> transactions { get; set; }
        public string coin { get; set; }
    }
        //public class Input
        //{
        //    public string id { get; set; }
        //    public string address { get; set; }
        //    public int value { get; set; }
        //    public string wallet { get; set; }
        //    public string fromWallet { get; set; }
        //    public int chain { get; set; }
        //    public int index { get; set; }
        //}

        //public class Output
        //{
        //    public string id { get; set; }
        //    public string address { get; set; }
        //    public int value { get; set; }
        //}

        //public class Entry
        //{
        //    public string account { get; set; }
        //    public int value { get; set; }
        //    public int inputs { get; set; }
        //    public int outputs { get; set; }
        //}

        //public class Transaction
        //{
        //    public string id { get; set; }
        //    public int size { get; set; }
        //    public int fee { get; set; }
        //    public DateTime date { get; set; }
        //    public string hex { get; set; }
        //    public string fromWallet { get; set; }
        //    public string blockHash { get; set; }
        //    public int blockHeight { get; set; }
        //    public int blockPosition { get; set; }
        //    public List<string> inputIds { get; set; }
        //    public string comment { get; set; }
        //    public List<Input> inputs { get; set; }
        //    public List<Output> outputs { get; set; }
        //    public List<Entry> entries { get; set; }
        //}

    //    public class ListWalletTransactionRootObject : BizResponseClass
    //    {
    //        public List<Transaction> transactions { get; set; }
    //        public string coin { get; set; }
    //    }
    //}
}
