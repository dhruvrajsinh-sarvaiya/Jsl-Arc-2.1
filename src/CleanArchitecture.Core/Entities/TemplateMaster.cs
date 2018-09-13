using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;

namespace CleanArchitecture.Core.Entities
{
    public  class TemplateMaster : BizBase
    {
        public long TemplateID { get; set; }
        public long CommServiceID { get; set; }
        public string TemplateName { get; set; }
        public string Content { get; set; }
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
