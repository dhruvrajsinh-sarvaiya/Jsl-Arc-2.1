using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Configuration;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Data
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, IMessageSender
    {
        public EmailSettings _emailSettings { get; }
        public SMSSetting _smsSettings { get; set; }
 
        public AuthMessageSender(IOptions<EmailSettings> emailSettings,IOptions<SMSSetting> SmsSetting)
        {
            _emailSettings = emailSettings.Value;
            _smsSettings = SmsSetting.Value;
        }
        /// <summary>
        ///  This task are used send email.
        /// </summary>  
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SendEmail(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        /// <summary>
        ///  This task are used send email process.
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
        ///  This task are used send sms. 
        /// </summary>
        public Task SendSMSAsync(string phoneno, string message)
        {
            SendSMS(phoneno, message).Wait();
            return Task.FromResult(0);
        }

        /// <summary>
        ///  This task are used send sms process.
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
