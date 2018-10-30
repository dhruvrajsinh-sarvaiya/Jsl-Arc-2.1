using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class SignalRComm
    {
        public string EventType { get; set; }
        public string EventTime
        {
            get { return Helpers.Helpers.GetUTCTime(); }
            set { EventTime = value; }
        }
        public string Method { get; set; }
        public string ReturnMethod { get; set; }
        public string Subscription { get; set; }
        public string ParamType { get; set; }
        public string Parameter { get; set; }
    }
}
