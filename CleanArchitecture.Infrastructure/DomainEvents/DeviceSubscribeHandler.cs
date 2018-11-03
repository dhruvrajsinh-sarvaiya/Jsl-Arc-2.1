﻿using CleanArchitecture.Core.ApiModels;
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
    public class DeviceSubscribeHandler : IRequestHandler<DeviceRegistrationRequest, CommunicationResponse>
    {
        private readonly IMessageRepository<DeviceStore> _MessageRepository;

        public DeviceSubscribeHandler(IMessageRepository<DeviceStore> MessageRepository)
        {
            _MessageRepository = MessageRepository;
        }

        public async Task<CommunicationResponse> Handle(DeviceRegistrationRequest Request, CancellationToken cancellationToken)
        {
            try
            {
                CommunicationResponse Response = new CommunicationResponse();
                if (Request.SubsscrptionType == EnDeviceSubsscrptionType.Subsscribe)
                {
                    Response = await SubscibePushNotification(Request);
                    return Response;
                }
                else if (Request.SubsscrptionType == EnDeviceSubsscrptionType.UnSubsscribe)
                {
                    Response = await SubscibePushNotification(Request);
                    return Response;
                }
                return await Task.FromResult(new CommunicationResponse
                {
                    ReturnCode = enResponseCode.Fail,
                    ReturnMsg = EnResponseMessage.PushNotificationSubscriptionFail,
                    ErrorCode = enErrorCode.PushNotificationSubscriptionFail
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = EnResponseMessage.PushNotificationSubscriptionFail,
                    ErrorCode = enErrorCode.PushNotificationSubscriptionFail
                });
            }
        }

        public async Task<CommunicationResponse> SubscibePushNotification(DeviceRegistrationRequest Request)
        {
            try
            {
                var DeviceStore = new DeviceStore();
                DeviceStore = _MessageRepository.GetById(Request.UserID);
                if (DeviceStore != null && DeviceStore.UserID > 0)
                {
                    DeviceStore.Active();
                    _MessageRepository.Update(DeviceStore);
                }
                else
                {
                    DeviceStore = new DeviceStore()
                    {
                        DeviceID = Request.DeviceID,
                        UserID = Request.UserID,
                        Status = Convert.ToInt16(ServiceStatus.Active),
                        CreatedBy = 1,
                        CreatedDate = DateTime.UtcNow
                    };
                    _MessageRepository.Add(DeviceStore);
                }
                return await Task.FromResult(new CommunicationResponse
                {
                    ReturnCode = enResponseCode.Success,
                    ReturnMsg = EnResponseMessage.PushNotificationSubscriptionSuccess,
                    ErrorCode = enErrorCode.PushNotificationSubscriptionSuccess
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = EnResponseMessage.PushNotificationSubscriptionFail,
                    ErrorCode = enErrorCode.PushNotificationSubscriptionFail
                });
            }
        }

        public async Task<CommunicationResponse> UnsbscibePushNotification(DeviceRegistrationRequest Request)
        {
            try
            {
                var DeviceStore = new DeviceStore();
                DeviceStore = _MessageRepository.GetById(Request.UserID);
                DeviceStore.InActive();
                _MessageRepository.Update(DeviceStore);
                return await Task.FromResult(new CommunicationResponse
                {
                    ReturnCode = enResponseCode.Success,
                    ReturnMsg = EnResponseMessage.PushNotificationunsubscriptionSuccess,
                    ErrorCode = enErrorCode.PushNotificationunsubscriptionSuccess
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new CommunicationResponse
                {
                    ReturnCode = enResponseCode.InternalError,
                    ReturnMsg = EnResponseMessage.PushNotificationUnsubscriptionFail,
                    ErrorCode = enErrorCode.PushNotificationUnsubscriptionFail
                });
            }
        }
    }
}