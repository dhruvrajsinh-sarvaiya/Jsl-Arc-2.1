using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class SendSMSResponse : IRequest
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
