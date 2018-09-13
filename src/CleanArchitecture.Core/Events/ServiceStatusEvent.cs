using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Events
{
    public class ServiceStatusEvent<T> : BaseDomainEvent
    {
        public T ChanegdServiceStatus { get; set; }

        public ServiceStatusEvent(T ChangedStatus)
        {
            ChanegdServiceStatus = ChangedStatus;
        }
    }
}