using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SignalR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class SignalRData: IRequest
    {
        public string DataObj { get; set; }
        public enMethodName Method { get; set; }
        public string Parameter { get; set; }
    }
}
