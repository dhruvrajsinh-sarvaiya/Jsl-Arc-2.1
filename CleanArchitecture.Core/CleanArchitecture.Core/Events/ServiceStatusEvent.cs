using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Events
{
    public class ServiceStatusEvent<T> : BaseDomainEvent
    {
        public T ChangedServiceStatus { get; set; }

        public ServiceStatusEvent(T ChangedStatus)
        {
            ChangedServiceStatus = ChangedStatus;
        }
    }
}