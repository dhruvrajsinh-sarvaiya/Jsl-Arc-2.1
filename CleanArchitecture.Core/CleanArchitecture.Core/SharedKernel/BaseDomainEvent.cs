using System;

namespace CleanArchitecture.Core.SharedKernel
{
    public abstract class BaseDomainEvent
    {
        // public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; protected set; } = DateTime.UtcNow;
    }
}