using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmailHandler : IRequestHandler<SendEmailRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<EmailQueue> _MessageRepository;

        public EmailHandler(IMessageRepository<EmailQueue> MessageRepository)
        {
            _MessageRepository = MessageRepository;
        }

        public Task<CommunicationResponse> Handle(SendEmailRequest Request, CancellationToken cancellationToken)
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
                return Task.FromResult(new CommunicationResponse { ErrorCode = 101, ReturnMsg = "Message sent."});
            }
            catch(Exception ex)
            {
                return Task.FromResult(new CommunicationResponse { ErrorCode = 99, ReturnMsg = "Message not sent."});
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
