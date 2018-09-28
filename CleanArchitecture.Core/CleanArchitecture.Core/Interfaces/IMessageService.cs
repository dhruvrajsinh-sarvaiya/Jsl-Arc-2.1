using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IMessageService
    {
        Task SendEmailAsync(string Email, string Recepient, string Subject, string BCC,string CC, string Body, string Url, string UserID, string Password,string Port);
        //Task SendEmailViaSendgridAsync(string Email, string Recepient, string Subject, string BCC,string CC, string Body, string Url, string UserID, string Password);
        Task SendSMSAsync(long Mobile, string Message, string Url, string SerderID, string UserID, string Password);
        Task SendNotificationAsync(string DeviceID, string Message, string Url);
    }
}
