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
    public class NotificationHandler : IRequestHandler<SendNotificationRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<NotificationQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;

        public NotificationHandler(IMessageRepository<NotificationQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService)
        {
            _MessageRepository = MessageRepository;
        }

        public async Task<CommunicationResponse> Handle(SendNotificationRequest Request, CancellationToken cancellationToken)
        {
            try
            {
                var Notification = new NotificationQueue()
                {
                    Message = Request.Message,
                    Subject = Request.Subject,
                    DeviceID = Request.DeviceID,
                    TickerText = Request.TickerText,
                    ContentTitle = Request.ContentTitle,
                    Status = 0,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow
                };
                _MessageRepository.Add(Notification);
                Notification.InQueueMessage();
                _MessageRepository.Update(Notification);
                IQueryable Result = await _MessageConfiguration.GetAPIConfigurationAsync(1, 2);
                foreach (CommunicationProviderList g in Result)
                {
                    string Resposne = await _MessageService.SendNotificationAsync(Notification.DeviceID,Notification.TickerText, Notification.ContentTitle, Notification.Message, g.SendURL,g.RequestFormat,g.SenderID,g.MethodType,g.ContentType);

                    if (Resposne != "Fail")
                    {
                        Notification.Status = Convert.ToInt16(enMessageService.Success);
                        Notification.SentMessage();
                        _MessageRepository.Update(Notification);
                        break;
                    }
                    else
                    {
                        Notification.Status = Convert.ToInt16(enMessageService.Fail);
                        Notification.SentMessage();
                        _MessageRepository.Update(Notification);
                        continue;
                    }
                }
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = "Message sent." });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = "Message not sent."});
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
