using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class WalletLedgerRes
    {
        public long   LedgerId { get; set; }

        public decimal PreBal { get; set; }

        public decimal PostBal { get; set; }

        public decimal CrAmount { get; set; }

        public decimal DrAmount { get; set; }

        public string Remarks { get; set; }

        public decimal Amount { get; set; }

        public DateTime TrnDate { get; set; }
    }
    public class ListWalletLedgerRes
    {
        public List<WalletLedgerRes> WalletLedgers { get; set; }
        public BizResponseClass BizResponseObj { get; set; }
    }

    public class ListWalletLedgerResponse : BizResponseClass
    {
        public List<WalletLedgerResponse> WalletLedgers { get; set; }
    }


    public class WalletLedgerResponse
    {
        public long LedgerId { get; set; }

        public decimal PreBal { get; set; }

        public decimal PostBal { get; set; }

        public decimal CrAmount { get; set; }

        public decimal DrAmount { get; set; }

        public string Remarks { get; set; }

        public decimal Amount { get; set; }

        public string TrnDate { get; set; }
    }
}
