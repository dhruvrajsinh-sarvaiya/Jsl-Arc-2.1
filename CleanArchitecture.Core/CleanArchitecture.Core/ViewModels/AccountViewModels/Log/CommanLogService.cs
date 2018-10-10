using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Interfaces.Log;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Log
{
   public class CommanLogService
    {
        private readonly IipAddressService _iipAddressService;
        private readonly IDeviceIdService _iDeviceIdService;
        private readonly IHistoryService _iHistoryService;
        public CommanLogService(IDeviceIdService iDeviceIdService, IipAddressService iipAddressService, IHistoryService iHistoryService)
        {
            _iDeviceIdService = iDeviceIdService;
            _iipAddressService = iipAddressService;
            _iHistoryService = iHistoryService;

        }
        public async void AddLog(object model)
        {
            //var currentHistory = new HistoryMaster
            //{
            //    UserId = model.UserId,
            //    HistoryTypeId = model.HistoryTypeId,
            //    ServiceUrl = model.ServiceUrl,
            //    IpId = model.IpId,
            //    DeviceId = model.DeviceId,
            //    Mode = model.Mode,
            //    HostName = model.HostName,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow,
            //    CreatedBy = model.UserId,
            //    Status = 0,

            //};
            //_historyMasterRepository.Insert(currentHistory);
            ////_dbContext.SaveChanges();

            //return currentHistory.Id;
        }
    }

}
