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
    public class EmailHandler : IRequestHandler<SendEmailRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<EmailQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;
        private WebApiParseResponse _WebApiParseResponse;
        private GetDataForParsingAPI _GetDataForParsingAPI;
        private WebAPIParseResponseCls _GenerateResponse;

        public EmailHandler(IMessageRepository<EmailQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService, GetDataForParsingAPI GetDataForParsingAPI, WebApiParseResponse WebApiParseResponse, WebAPIParseResponseCls GenerateResponse)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
            _GetDataForParsingAPI = GetDataForParsingAPI;
            _WebApiParseResponse = WebApiParseResponse;
            _GenerateResponse = GenerateResponse;
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
                foreach (CommunicationProviderList Provider in Result)
                {
                    string Response = await _MessageService.SendEmailAsync(Email.Recepient, Email.Subject, Email.BCC, Email.CC, Email.Body, Provider.SendURL,Provider.UserID , Provider.Password,Convert.ToInt16(Provider.SenderID));
                    CopyClass.CopyObject(Provider, ref _GetDataForParsingAPI);
                    _GenerateResponse = _WebApiParseResponse.ParseResponseViaRegex(Response, _GetDataForParsingAPI);
                    if (_GenerateResponse.Status == enTransactionStatus.Success)
                    {
                        Email.SentMessage();
                        _MessageRepository.Update(Email);
                        return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.EMailSuccessMessage });
                    }
                    else
                    {                        
                        continue;
                    }
                }
                Email.SentMessage();
                _MessageRepository.Update(Email);
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.EmailFailMessage});
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = EnResponseMessage.EmailExceptionMessage});
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
