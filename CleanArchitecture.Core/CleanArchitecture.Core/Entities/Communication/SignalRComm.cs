using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class SignalRComm
    {
        public int Type { get; set; }
        public int Method { get; set; }
        public int ReturnMethod { get; set; }
        public short Subscription { get; set; }
    }
}
