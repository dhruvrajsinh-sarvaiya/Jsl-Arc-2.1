using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class TransferInOutRes
    {
        public List<TransfersRes> Transfers { get; set; }
        public BizResponseClass BizResponseObj { get; set; }
    }
    public class TransfersRes
    {
        public long AutoNo { get; set; }

        public string TrnID { get; set; }

        public string WalletType { get; set; }

        public string Address { get; set; }

        public string User { get; set; }

        public long Confirmations { get; set; }

        public decimal Amount { get; set; }

        public string ConfirmedTime { get; set; }

        public short? ConfirmationCount { get; set; }

  
    }
}
