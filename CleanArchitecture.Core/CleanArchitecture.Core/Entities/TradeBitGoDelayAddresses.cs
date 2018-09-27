using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Core.Events;

namespace CleanArchitecture.Core.Entities
{
    public class TradeBitGoDelayAddresses : BizBase
    {
        [Required]
        public long WalletmasterID { get; set; }

        [Required]
        [StringLength(100)]
        public string TrnID { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public byte GenerateBit { get; set; }

        [Required]
        [StringLength(5)]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(100)]
        public string BitgoWalletId { get; set; }

        [Required]
        [StringLength(250)]
        public string CoinSpecific { get; set; }
               

        public void GetAddressInStatusCheck(byte generateBit,string address,string coinSpecific,long WalletID)
        {
            GenerateBit = generateBit;
            Address = address;
            CoinSpecific = coinSpecific;
            WalletmasterID = WalletID;
            Events.Add(new ServiceStatusEvent<TradeBitGoDelayAddresses>(this));
        }
    }

}
