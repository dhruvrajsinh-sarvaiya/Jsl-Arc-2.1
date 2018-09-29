using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Entities
{
    public  class CommServiceMaster : BizBase
    {
        [Required]
        public long CommServiceID { get; set; }

        [Required]
        public long RequestID { get; set; }

        [Required]
        public long CommSerproID { get; set; }

        [Required]
        [StringLength(60)]
        public string ServiceName { get; set; }

        public void DisableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceStatusEvent<CommServiceMaster>(this));
        }

        public void EnableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<CommServiceMaster>(this));
        }
    }
}
