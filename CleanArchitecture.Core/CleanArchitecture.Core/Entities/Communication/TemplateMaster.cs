using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Entities
{
    public  class TemplateMaster : BizBase
    {
        [Required]
        public long TemplateID { get; set; }

        [Required]
        public long CommServiceTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TemplateName { get; set; }

        [Required]
        [StringLength(1024)]
        public string Content { get; set; }

        [Required]
        [StringLength(200)]
        public string AdditionalInfo { get; set; }

        public void DisableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceStatusEvent<TemplateMaster>(this));
        }

        public void EnableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<TemplateMaster>(this));
        }
    }
}
