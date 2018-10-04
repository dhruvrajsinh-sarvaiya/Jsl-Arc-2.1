using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Infrastructure.DTOClasses;
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
        private readonly WebApiParseResponse _WebApiParseResponse;
        private GetDataForParsingAPI _GetDataForParsingAPI;

        public NotificationHandler(IMessageRepository<NotificationQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService, GetDataForParsingAPI GetDataForParsingAPI, WebApiParseResponse WebApiParseResponse)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
            _GetDataForParsingAPI = GetDataForParsingAPI;
            _WebApiParseResponse = WebApiParseResponse;
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
                foreach (CommunicationProviderList Provider in Result)
                {
                    string Resposne = await _MessageService.SendNotificationAsync(Notification.DeviceID,Notification.TickerText, Notification.ContentTitle, Notification.Message, Provider.SendURL,Provider.RequestFormat,Provider.SenderID,Provider.MethodType,Provider.ContentType);
                    CopyClass.CopyObject(Provider, ref _GetDataForParsingAPI);
                    WebAPIParseResponseCls GenerateResponse = _WebApiParseResponse.ParseResponseViaRegex(Resposne, _GetDataForParsingAPI);
                    if (GenerateResponse.Status == enTransactionStatus.Success)
                    {
                        Notification.SentMessage();
                        _MessageRepository.Update(Notification);
                        return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.NotificationSuccessMessage });
                    }
                    else
                    {
                        continue;
                    }
                }
                Notification.FailMessage();
                _MessageRepository.Update(Notification);
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.NotificationFailMessage });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = EnResponseMessage.NotificationExceptionMessage});
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
