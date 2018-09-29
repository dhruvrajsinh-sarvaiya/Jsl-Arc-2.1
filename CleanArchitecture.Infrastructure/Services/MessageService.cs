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
    public class MessageService : IMessageService, IEmailSender, IMessageSender
    {
        public EmailSettings _emailSettings { get; }
        public SMSSetting _smsSettings { get; set; }

        public MessageService(IOptions<EmailSettings> emailSettings, IOptions<SMSSetting> SmsSetting)
        {
            _emailSettings = emailSettings.Value;
            _smsSettings = SmsSetting.Value;
        }

        public Task SendEmailAsync(string Email, string Recepient, string Subject, string BCC, string CC, string Body, string Url, string UserID, string Password, string Port)
        {
            return Task.FromResult(0);
        }

        public Task SendSMSAsync(long Mobile, string Message, string Url, string SerderID, string UserID, string Password)
        {
            return Task.FromResult(0);
        }

        public Task SendNotificationAsync(string DeviceID, string Message, string Url)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        ///  This task are used send email. add by nirav savariya 9-28-2018
        /// </summary>  
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SendEmail(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        /// <summary>
        ///  This task are used send email process. add by nirav savariya 9-28-2018
        /// </summary>  
        public async Task SendEmail(string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail)
                };
                mail.To.Add(new MailAddress(toEmail));
                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = "JOSHI BIZTECH SOLUTIONS LIMITED  - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        ///  This task are used send sms. add by nirav savariya 9-28-2018 
        /// </summary>
        public Task SendSMSAsync(string phoneno, string message)
        {
            SendSMS(phoneno, message).Wait();
            return Task.FromResult(0);
        }

        /// <summary>
        ///  This task are used send sms process. add by nirav savariya 9-28-2018
        /// </summary>  
        public async Task SendSMS(string phoneno, string message)
        {
            try
            {
                //ASPSMS.SMS SMSSender = new ASPSMS.SMS();

                //SMSSender.Userkey = Options.SMSAccountIdentification;
                //SMSSender.Password = Options.SMSAccountPassword;
                //SMSSender.Originator = Options.SMSAccountFrom;

                //SMSSender.AddRecipient(phoneno);
                //SMSSender.MessageData = message;

                //SMSSender.SendTextSMS();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
