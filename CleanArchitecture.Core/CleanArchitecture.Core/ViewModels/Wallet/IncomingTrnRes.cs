using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ListIncomingTrnRes
    {
        public List<IncomingTrnRes> IncomingTransactions { get; set; }
        public BizResponseClass BizResponseObj { get; set; }
    }
    public class IncomingTrnRes
    {
        public  long AutoNo { get; set; }

        public string TrnID { get; set; }

        public string WalletType { get; set; }

        public string Address { get; set; }
        
        public long Confirmations { get; set; }

        public decimal Amount { get; set; }

        public short? ConfirmationCount { get; set; }

        public List<ExplorerData> ExplorerLink { get; set; }
    }
}
