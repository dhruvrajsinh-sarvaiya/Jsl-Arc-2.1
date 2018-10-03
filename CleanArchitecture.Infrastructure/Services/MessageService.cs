using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        //public EmailSettings _emailSettings { get; }
        //public SMSSetting _smsSettings { get; set; }
        public WebAPISendRequest<MessageService> _WebAPISendRequest { get; set; }

        public MessageService(WebAPISendRequest<MessageService> WebAPISendRequest)
        {
            //_emailSettings = emailSettings.Value;
            //_smsSettings = SmsSetting.Value;
            _WebAPISendRequest = WebAPISendRequest;
        }

        public async Task<string> SendEmailAsync(string Recepient, string Subject, string BCC, string CC, string Body, string Url, string UserID, string Password, int Port)
        {
            try
            {
                string toEmail = Recepient;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(UserID)
                };
                mail.To.Add(new MailAddress(toEmail));
                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(Url,Port))
                {
                    smtp.Credentials = new NetworkCredential(UserID, Password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
                return await Task.FromResult("Success");
            }
            catch (Exception ex)
            {
                ex.ToString();
                return await Task.FromResult("Fail");
            }            
        }

        public async Task<string> SendSMSAsync(long Mobile, string Message, string Url, string SerderID, string UserID, string Password)
        {
            string Response="";
            try
            {
                Url = Url.Replace("[USERID]", UserID);
                Url = Url.Replace("[AUTHKEY]", Password);
                Url = Url.Replace("[MOBILENO]", Mobile.ToString());
                Url = Url.Replace("[SENDERID]", SerderID);
                Url = Url.Replace("[MSGTEXT]", Message);
                Response = await _WebAPISendRequest.SendRequestAsync(Url);
                return await Task.FromResult(Response);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public Task<string> SendNotificationAsync(string DeviceID, string Message, string Url)
        {
            return Task.FromResult("");
        }

        //public async Task SendEmail(string email, string subject, string message)
        //{
        //    try
        //    {
        //        string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;
        //        MailMessage mail = new MailMessage()
        //        {
        //            From = new MailAddress(_emailSettings.UsernameEmail)
        //        };
        //        mail.To.Add(new MailAddress(toEmail));
        //        //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

        //        mail.Subject = "JOSHI BIZTECH SOLUTIONS LIMITED  - " + subject;
        //        mail.Body = message;
        //        mail.IsBodyHtml = true;
        //        mail.Priority = MailPriority.High;

        //        using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
        //        {
        //            smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
        //            smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(mail);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}     
    }
}
