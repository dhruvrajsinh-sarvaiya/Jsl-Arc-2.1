using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Events
{
    public class ModeStatusEvent : BaseDomainEvent
    {
        public Mode ModeUpdateItem { get; set; }

        public ModeStatusEvent(Mode modeUpdateItem)
        {
            ModeUpdateItem = modeUpdateItem;
        }
    }
}
