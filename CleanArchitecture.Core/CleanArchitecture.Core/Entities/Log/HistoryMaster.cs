using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities.Log
{
    public class HistoryMaster : BizBase
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public long HistoryTypeId { get; set; }
        [Required]
        [StringLength(250)]
        public string ServiceUrl { get; set; }
        [Required]
        public long IpId { get; set; }
        [Required]
        public long DeviceId { get; set; }
        [Required]
        [StringLength(10)]
        public string Mode { get; set; }
        [Required]
        [StringLength(250)]
        public  string HostName { get; set; }
        public bool IsDeleted { get; set; }

        public void SetAsIpDeletetatus()
        {
            IsDeleted = true;
            Events.Add(new ServiceStatusEvent<HistoryMaster>(this));
        }
    }
}
