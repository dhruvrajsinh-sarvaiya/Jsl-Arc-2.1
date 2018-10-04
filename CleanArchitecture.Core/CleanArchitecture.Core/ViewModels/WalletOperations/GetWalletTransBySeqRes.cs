using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class GetWalletTransBySeqRes : BizResponseClass
    {
        public string id { get; set; }
        public string coin { get; set; }
        public string wallet { get; set; }
        public string txid { get; set; }
        public int height { get; set; }
        public DateTime date { get; set; }
        public int confirmations { get; set; }
        public string type { get; set; }
        public int value { get; set; }
        public int bitgoFee { get; set; }
        public double usd { get; set; }
        public double usdRate { get; set; }
        public string state { get; set; }
        public List<string> tags { get; set; }
        public string sequenceId { get; set; }
        public List<History> history { get; set; }
        public List<Entry> entries { get; set; }
        public List<Output> outputs { get; set; }
        public List<Input> inputs { get; set; }
        public DateTime confirmedTime { get; set; }
        public DateTime createdTime { get; set; }


        //public class GetWalletTransBySeqRootObject
        //{
            
        //}
    }
    
}
