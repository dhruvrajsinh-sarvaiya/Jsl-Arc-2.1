using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IMessageSender
    {
        Task SendSMSAsync(string phoneno, string message);
    }
}
