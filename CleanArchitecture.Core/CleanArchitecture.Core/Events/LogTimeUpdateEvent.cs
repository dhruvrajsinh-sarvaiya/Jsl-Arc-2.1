using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Events
{
    public class LogTimeUpdateEvent<T> : BaseDomainEvent
    {
        public T LogUpdateItem { get; set; }

        public LogTimeUpdateEvent(T logUpdateItem)
        {
            LogUpdateItem = logUpdateItem;
        }
    }
}
