using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities.Log
{
    public class DeviceMaster : BizBase
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(2000)]
        public string DeviceId { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDeleted { get; set; }

        public void SetAsIpDeletetatus()
        {
            IsDeleted = true;
            Events.Add(new ServiceStatusEvent<DeviceMaster>(this));
        }

        public void SetAsIsEnabletatus()
        {
            IsEnable = false;
            Events.Add(new ServiceStatusEvent<DeviceMaster>(this));
        }     
    }
}
