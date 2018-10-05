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
    public class SMSHandler : IRequestHandler<SendSMSRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<MessagingQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;       
        private WebApiParseResponse _WebApiParseResponse;
        private GetDataForParsingAPI _GetDataForParsingAPI;
        private WebAPIParseResponseCls _GenerateResponse;

        public SMSHandler(IMessageRepository<MessagingQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService, GetDataForParsingAPI GetDataForParsingAPI, WebApiParseResponse WebApiParseResponse, WebAPIParseResponseCls GenerateResponse)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
            _GetDataForParsingAPI = GetDataForParsingAPI;
            _WebApiParseResponse = WebApiParseResponse;
            _GenerateResponse = GenerateResponse;
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
                    Status = Convert.ToInt16(enMessageService.Init),
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow
                };
                _MessageRepository.Add(Message);
                Message.InQueueMessage();
                _MessageRepository.Update(Message);
                IQueryable Result = await _MessageConfiguration.GetAPIConfigurationAsync(1,1);
                foreach (CommunicationProviderList Provider in Result)
                {
                    string Response = await _MessageService.SendSMSAsync(Message.MobileNo, Message.SMSText, Provider.SendURL, Provider.SenderID, Provider.UserID, Provider.Password);
                    //string Response = " GUID:2387354506261631296 , responseTime:2018-10-05 16:00:46";
                    CopyClass.CopyObject(Provider, ref _GetDataForParsingAPI);
                    _GenerateResponse = _WebApiParseResponse.ParseResponseViaRegex(Response, _GetDataForParsingAPI);
                    if (_GenerateResponse.Status == enTransactionStatus.Success)
                    {                       
                        Message.SentMessage();
                        Message.RespText = Response;
                        _MessageRepository.Update(Message);
                        return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SMSSuccessMessage });
                    }
                    else
                    {                     
                        continue;
                    }
                }
                Message.FailMessage();
                _MessageRepository.Update(Message);
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SMSFailMessage });
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = EnResponseMessage.SMSExceptionMessage });
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
