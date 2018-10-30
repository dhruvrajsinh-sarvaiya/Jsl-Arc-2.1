using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class SignalRComm<T>
    {
        public T Data;
        public string EventType { get; set; }
        public string EventTime
        {
            get { return Helpers.Helpers.GetUTCTime(); }
            set { EventTime = value; }
        }
        public string Parameter { get; set; }
        public string ParamType { get; set; }
        public string ReturnMethod { get; set; }
        public string Method { get; set; }
        public string Subscription { get; set; }
        public short IsBuyerMaker { get; set; }
        public short IsIgnore { get; set; }
    }
}
