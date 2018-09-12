using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;

namespace CleanArchitecture.Core.Entities
{
    public  class CommServiceMaster : BaseEntity
    {
        public long CommServiceID { get; set; }
        public long RequestID { get; set; }
        public long CommSerproID { get; set; }
        public string ServiceName { get; set; }
        public short Status { get; set; }
        public DateTime CreatedDate { get; set; }

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
