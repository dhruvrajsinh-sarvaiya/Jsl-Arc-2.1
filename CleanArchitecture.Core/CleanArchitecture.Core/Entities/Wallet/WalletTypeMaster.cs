using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using Newtonsoft.Json;

namespace CleanArchitecture.Core.Entities
{
    public class WalletTypeMaster : BizBase
    {
        [Required]
        [StringLength(50)]
        [JsonProperty(PropertyName = "CoinName")]
        public string WalletTypeName { get; set; }

        [Required]
        [StringLength(100)]
        public string Discription { get; set; }

        [Required]
        public short IsDepositionAllow { get; set; }

        [Required]
        public short IsWithdrawalAllow { get; set; }

        [Required]
        public short IsTransactionWallet { get; set; }

        
        public short? IsDefaultWallet { get; set; }

        public void DisableStatus()
        {
            Status  = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new WalletStatusDisable<WalletTypeMaster>(this));
        }

    }
}
