using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;

namespace CleanArchitecture.Core.Entities
{
    public  class RequestFormatMaster : BaseEntity
    {
        public long RequestID { get; set; }
        public string contentType { get; set; }
        public string MethodType { get; set; }
        public string RequestFormat { get; set; }
        public short Status { get; set; }
        public DateTime CreatedDate { get; set; }

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
