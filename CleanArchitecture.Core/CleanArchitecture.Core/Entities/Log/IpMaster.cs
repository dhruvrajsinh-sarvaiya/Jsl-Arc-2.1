using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities.Log
{
   public class IpMaster  : BizBase
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(15)]
        public string IpAddress { get; set; }
        [StringLength(150)]
        public string IpAliasName { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDeleted { get; set; }
       

        public void SetAsIsEnabletatus()
        {
            IsEnable = true;
            Events.Add(new ServiceStatusEvent<IpMaster>(this));
        }

        public void SetAsIsDisabletatus()
        {
            IsEnable = false;
            Events.Add(new ServiceStatusEvent<IpMaster>(this));
        }

        public void SetAsIpDeletetatus()
        {
            IsDeleted = true;
            Events.Add(new ServiceStatusEvent<IpMaster>(this));
        }
    }
}
