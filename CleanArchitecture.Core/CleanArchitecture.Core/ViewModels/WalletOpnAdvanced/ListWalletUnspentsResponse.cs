using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ListWalletUnspentsResponse : BizResponseClass
    {
        public List<Unspent> unspents { get; set; }
        public string coin { get; set; }
        //public class Unspent
        //{
        //    public string id { get; set; }
        //    public string address { get; set; }
        //    public int value { get; set; }
        //    public int blockHeight { get; set; }
        //    public DateTime date { get; set; }
        //    public string wallet { get; set; }
        //    public object fromWallet { get; set; }
        //    public int chain { get; set; }
        //    public int index { get; set; }
        //    public string redeemScript { get; set; }
        //    public bool isSegwit { get; set; }
        //}

        //public class ListWalletUnspentsRootObject : BizResponseClass
        //{
        //    public List<Unspent> unspents { get; set; }
        //    public string coin { get; set; }
        //}
    }
}
