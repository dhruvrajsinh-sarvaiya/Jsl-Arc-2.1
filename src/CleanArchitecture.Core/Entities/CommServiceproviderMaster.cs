using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;

namespace CleanArchitecture.Core.Entities
{
    public  class CommServiceproviderMaster : BizBase
    {
        public long CommSerproID { get; set; }
        public long CommServiceID { get; set; }
        public string SerproName { get; set; }

        public void DisableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceStatusEvent<CommServiceproviderMaster>(this));
        }

        public void EnableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<CommServiceproviderMaster>(this));
        }
    }
}
