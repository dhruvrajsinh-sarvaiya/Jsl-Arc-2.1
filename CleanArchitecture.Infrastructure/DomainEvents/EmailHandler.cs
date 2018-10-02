using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmailHandler : IRequestHandler<SendEmailRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<EmailQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;

        public EmailHandler(IMessageRepository<EmailQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
        }

        public async Task<CommunicationResponse> Handle(SendEmailRequest Request, CancellationToken cancellationToken)
        {
            try
            {
                var Email = new EmailQueue()
                {
                    EmailType = Request.EmailType,
                    Recepient = Request.Recepient,
                    Attachment = Request.Attachment,
                    BCC = Request.BCC,
                    CC = Request.CC,
                    Body = Request.Body,
                    Status = Convert.ToInt16(MessageStatusType.Initialize),
                    Subject = Request.Subject,
                    CreatedDate = DateTime.UtcNow
                };
                _MessageRepository.Add(Email);
                Email.InQueueMessage();
                _MessageRepository.Update(Email);
                IQueryable Result = await _MessageConfiguration.GetAPIConfigurationAsync(1, 2);
                foreach (CommunicationProviderList g in Result)
                {
                    string Resposne = await _MessageService.SendEmailAsync(Email.Recepient, Email.Subject, Email.BCC, Email.CC, Email.Body, g.SendURL,g.UserID , g.Password,Convert.ToInt16(g.SenderID));

                    if (Resposne != "Fail")
                    {
                        Email.Status = Convert.ToInt16(enMessageService.Success);
                        Email.SentMessage();
                        _MessageRepository.Update(Email);
                        break;
                    }
                    else
                    {
                        Email.Status = Convert.ToInt16(enMessageService.Fail);
                        Email.SentMessage();
                        _MessageRepository.Update(Email);
                        continue;
                    }
                }
                return await Task.FromResult(new CommunicationResponse { ErrorCode = 101, ReturnMsg = "Message sent."});
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse { ErrorCode = 99, ReturnMsg = "Message not sent."});
            }
        }


        //public Task<ToDoItemResponse> Handle(ToDoItemRequest request, CancellationToken cancellationToken)
        //{
        //    var toDoItem = _todoRepository.GetById(request.Id);
        //    toDoItem.MarkComplete();
        //    _todoRepository.Update(toDoItem);
        //    return Task.FromResult(new ToDoItemResponse { IsDone = toDoItem.IsDone });
        //}
    }
}
