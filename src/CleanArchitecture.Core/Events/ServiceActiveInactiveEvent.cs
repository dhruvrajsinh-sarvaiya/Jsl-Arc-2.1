using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Events
{
    public class ServiceActiveInactiveEvent : BaseDomainEvent
    {
        public ServiceConfiguration ActivatedService { get; set; }
        public ServiceActiveInactiveEvent(ServiceConfiguration activatedService)
        {
            ActivatedService = activatedService;
        }
}
}
