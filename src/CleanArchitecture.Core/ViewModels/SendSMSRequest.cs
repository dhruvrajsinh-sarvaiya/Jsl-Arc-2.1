using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class SendSMSRequest : IRequest<SendSMSResponse>
    {
    }
}
