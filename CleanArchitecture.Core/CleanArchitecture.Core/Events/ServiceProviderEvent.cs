using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Events
{
    public class ServiceProviderEvent<T> : BaseDomainEvent
    {
        public T ChangedProviderEvent { get; set; }

        public ServiceProviderEvent(T ChangedEvent)
        {
            ChangedProviderEvent = ChangedEvent;
        }
    }
}
