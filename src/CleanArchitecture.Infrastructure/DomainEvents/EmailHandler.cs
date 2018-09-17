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
    public class EmailHandler : IRequestHandler<SendEmailRequest, SendEmailResponse>
    {
        private readonly IMessageRepository<EmailQueue> _MessageRepository;

        public EmailHandler(IMessageRepository<EmailQueue> MessageRepository)
        {
            _MessageRepository = MessageRepository;
        }

        public Task<SendEmailResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SendEmailResponse { });
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
