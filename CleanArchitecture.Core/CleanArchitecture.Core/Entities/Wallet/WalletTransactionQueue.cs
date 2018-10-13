using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class WalletTransactionQueue: BizBase
    {
        [Required]
        public long TrnNo { get; set; }

        [Required]
        public string Guid { get; set; }

        [Required]
        public byte TrnType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public long TradeID { get; set; }

        [Required]
        public DateTime TrnDate { get; set; }

        [Required]
        public long WalletID { get; set; }

        [Required]
        public string WalletType { get; set; }

        [Required]
        public long MemberID { get; set; }

        public string TimeStamp { get; set; }
      //  public byte Status { get; set; }
        public string StatusMsg { get; set; }
    }
}
