using System;

namespace CleanArchitecture.Core.SharedKernel
{
    public abstract class BaseDateEvent
    {
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; protected set; } = DateTime.UtcNow;
    }
}