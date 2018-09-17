using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Entities
{
    public  class RequestFormatMaster : BizBase
    {
        [StringLength(60)]
        public long RequestID { get; set; }

        [Required]
        [StringLength(60)]
        public string contentType { get; set; }

        [Required]
        [StringLength(20)]
        public string MethodType { get; set; }

        [Required]
        [StringLength(500)]
        public string RequestFormat { get; set; }

        public void DisableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceStatusEvent<RequestFormatMaster>(this));
        }

        public void EnableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<RequestFormatMaster>(this));
        }
    }
}
