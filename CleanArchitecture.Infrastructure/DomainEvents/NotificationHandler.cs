using CleanArchitecture.Core.Entities;
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
    public class NotificationHandler : IRequestHandler<SendNotificationRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<NotificationQueue> _MessageRepository;

        public NotificationHandler(IMessageRepository<NotificationQueue> MessageRepository)
        {
            _MessageRepository = MessageRepository;
        }

        public Task<CommunicationResponse> Handle(SendNotificationRequest Request, CancellationToken cancellationToken)
        {
            try
            {
                var Notification = new NotificationQueue()
                {
                    Message = Request.Message,
                    Subject = Request.Subject,
                    DeviceID = Request.DeviceID,
                    Status = 0,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow
                };
                _MessageRepository.Add(Notification);
                return Task.FromResult(new CommunicationResponse { ErrorCode = 101, ReturnMsg = "Message sent."});
            }
            catch (Exception ex)
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
