﻿using CleanArchitecture.Core.Entities;
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
    public class SMSHandler : IRequestHandler<SendSMSRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<MessagingQueue> _MessageRepository;

        public SMSHandler(IMessageRepository<MessagingQueue> MessageRepository)
        {
            _MessageRepository = MessageRepository;
        }

        public Task<CommunicationResponse> Handle(SendSMSRequest Request, CancellationToken cancellationToken)
        {
            try
            {
                var Message = new MessagingQueue()
                {
                    MobileNo = Request.MobileNo,
                    SMSText = Request.Message,
                    SMSSendBy = 0,
                    Status = 0,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow
                };
                _MessageRepository.Add(Message);
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
