using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;

namespace CleanArchitecture.Core.Entities
{
    class WalletOrderSub : BizBase
    {
        [Required]
        public long WalletOrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string OBranchName { get; set; }

        [Required]
        [StringLength(20)]
        public string OAccountNo { get; set; }

        [Required]
        [StringLength(20)]
        public string OChequeNo { get; set; }

        [Required]
        public DateTime? OChequeDate { get; set; }
                
        public long? RefNo { get; set; }

        public void SetAsSuccess()
        {
            Status = Convert.ToInt16(EnOrderStatus.Success);
            Events.Add(new ServiceStatusEvent<WalletOrderSub>(this));
        }
        public void SetAsRejected()
        {
            Status = Convert.ToInt16(EnOrderStatus.Rejected);
            Events.Add(new ServiceStatusEvent<WalletOrderSub>(this));
        }
    }
}
