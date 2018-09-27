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
    public class SMSHandler : IRequestHandler<SendSMSRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<MessagingQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;        

        public SMSHandler(IMessageRepository<MessagingQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
        }

        public async Task<CommunicationResponse> Handle(SendSMSRequest Request, CancellationToken cancellationToken)
        {
            try
            {
                var Message = new MessagingQueue()
                {
                    MobileNo = Request.MobileNo,
                    SMSText = Request.Message,
                    SMSSendBy = 0,
                    Status = Convert.ToInt16(MessageStatusType.Initialize),
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow
                };
                _MessageRepository.Add(Message);
                Message.InQueueMessage();
                _MessageRepository.Update(Message);
                IQueryable Result = await _MessageConfiguration.GetAPIConfigurationAsync(1,1);
                foreach (CommunicationProviderList g in Result)
                {
                    await _MessageService.SendSMSAsync(Message.MobileNo, Message.SMSText, g.SMSSendURL, g.SenderID, g.UserID, g.Password);
                }
                return await Task.FromResult(new CommunicationResponse { ErrorCode = 101, ReturnMsg = "Message sent." });
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse { ErrorCode = 99, ReturnMsg = "Message not sent." });
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
