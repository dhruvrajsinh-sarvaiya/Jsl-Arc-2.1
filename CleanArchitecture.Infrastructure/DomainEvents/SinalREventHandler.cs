using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.DomainEvents
{
    public class SinalREventHandler : IRequestHandler<SignalRData>
    {
        private readonly IMessageRepository<NotificationQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;
        private WebApiParseResponse _WebApiParseResponse;
        private GetDataForParsingAPI _GetDataForParsingAPI;
        private WebAPIParseResponseCls _GenerateResponse;

        public SinalREventHandler(IMessageRepository<NotificationQueue> MessageRepository, MessageConfiguration MessageConfiguration, MessageService MessageService, GetDataForParsingAPI GetDataForParsingAPI, WebApiParseResponse WebApiParseResponse, WebAPIParseResponseCls GenerateResponse)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
            _GetDataForParsingAPI = GetDataForParsingAPI;
            _WebApiParseResponse = WebApiParseResponse;
            _GenerateResponse = GenerateResponse;
        }

        public Task<Unit> Handle(SignalRData request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
