using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.Log;
using CleanArchitecture.Core.Interfaces.Log;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;

namespace CleanArchitecture.Infrastructure.Services.Log
{
    public class DeviceIdService : IDeviceIdService
    {
        private readonly ICustomRepository<DeviceMaster> _deviceMasterRepository;
        public DeviceIdService(ICustomRepository<DeviceMaster> deviceMasterRepository)
        {
            _deviceMasterRepository = deviceMasterRepository;
        }
        public async Task<long> AddDeviceId(DeviceMasterViewModel model)
        {
            var currentDeviceId = new DeviceMaster
            {
                UserId = model.UserId,
                DeviceId = model.DeviceId,
                IsEnable = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = model.UserId,
                Status = 0,

            };
            _deviceMasterRepository.Insert(currentDeviceId);
            //_dbContext.SaveChanges();
                      
            return currentDeviceId.Id;
        }
    }
}
