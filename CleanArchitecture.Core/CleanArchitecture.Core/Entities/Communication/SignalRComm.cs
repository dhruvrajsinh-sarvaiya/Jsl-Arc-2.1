using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class SignalRComm
    {
        public enSignalREventType  Type { get; set; }
        public string EventTime
        {
            get { return Helpers.Helpers.GetUTCTime(); }
            set { EventTime = value;  }
        }
        public enMethodName Method { get; set; }
        public enReturnMethod ReturnMethod { get; set; }
        public enSubscriptionType  Subscription { get; set; }
        public enSignalRParmType ParamType { get; set; }
        public string Parameter { get; set; }
        public short IsIgnore { get; set; }
    }
}
