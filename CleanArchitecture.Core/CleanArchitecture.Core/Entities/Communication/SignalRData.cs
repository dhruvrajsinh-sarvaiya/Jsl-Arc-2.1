using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class SignalRData: SignalRComm, IRequest
    {
        public string Data { get; set; }
        public short IsBuyerMaker { get; set; }
    }
}
