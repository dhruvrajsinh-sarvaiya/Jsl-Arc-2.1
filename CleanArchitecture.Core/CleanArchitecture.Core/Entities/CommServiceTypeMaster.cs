using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Entities
{
    public  class CommServiceTypeMaster : BizBase
    {
        [Required]
        public long CommServiceTypeID { get; set; }

        [Required]
        public long ServiceTypeID { get; set; }

        [Required]
        [StringLength(60)]
        public string CommServiceTypeName { get; set; }

        public void DisableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceStatusEvent<CommServiceTypeMaster>(this));
        }

        public void EnableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<CommServiceTypeMaster>(this));
        }
    }
}
