using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class HistoryObject
    {
        public string CoinName { get; set; }//coin
        public string Information { get; set; }//information
        public DateTime Date { get; set; }
        public short Status { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string StatusStr { get; set; }
        public long Confirmations { get; set; }
    }
}
