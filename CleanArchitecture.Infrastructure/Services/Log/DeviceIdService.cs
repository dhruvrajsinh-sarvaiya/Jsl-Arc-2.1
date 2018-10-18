using System;
using System.Collections.Generic;
using System.Linq;
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
        public long AddDeviceId(DeviceMasterViewModel model)
        {
            var getDeviceId = _deviceMasterRepository.Table.FirstOrDefault(i => i.DeviceId == model.DeviceId && !i.IsDeleted);
            if (getDeviceId != null)
            {
                return getDeviceId.Id;
            }

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
      
        public List<DeviceMasterViewModel> GetDeviceListByUserId(long UserId)
        {
            var DeviceIdList = _deviceMasterRepository.Table.Where(i => i.UserId == UserId && !i.IsDeleted).ToList();
            if (DeviceIdList == null)
            {
                return null;
            }

            var IpList = new List<DeviceMasterViewModel>();
            foreach (var item in DeviceIdList)
            {
                DeviceMasterViewModel imodel = new DeviceMasterViewModel();
                imodel.Id = item.Id;
                imodel.UserId = item.UserId;
                imodel.DeviceId = item.DeviceId;
                imodel.IsEnable = item.IsEnable;
                imodel.IsDeleted = item.IsDeleted;
                imodel.CreatedDate = item.CreatedDate;
                imodel.CreatedBy = item.CreatedBy;
                imodel.UpdatedDate = item.UpdatedDate;
                imodel.UpdatedBy = item.UpdatedBy;
                imodel.Status = item.Status;

                IpList.Add(imodel);
            }
            return IpList;

        }


        public void UpdateDeviceId(DeviceMasterViewModel model)
        {
            var DeviceIddata = _deviceMasterRepository.Table.FirstOrDefault(i => i.DeviceId == model.DeviceId && i.UserId == model.UserId && i.IsDeleted);
            if (DeviceIddata != null)
            {
                var currentDeviceId = new DeviceMaster
                {
                    Id = DeviceIddata.Id,
                    UserId = DeviceIddata.UserId,
                    DeviceId = model.DeviceId,
                    IsEnable = DeviceIddata.IsEnable,
                    //IsDeleted = IpAddress.IsDeleted,                    
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = DeviceIddata.UserId
                };

                _deviceMasterRepository.Update(currentDeviceId);
                //_dbContext.SaveChanges();

            }
        }

        public long DesableDeviceId(DeviceMasterViewModel model)
        {
            var Devicedata = _deviceMasterRepository.Table.FirstOrDefault(i => i.DeviceId == model.DeviceId && i.UserId == model.UserId && i.IsDeleted);
            if (Devicedata != null)
            {
                Devicedata.SetAsIsEnabletatus();
                _deviceMasterRepository.Update(Devicedata);
                //_dbContext.SaveChanges();
                return Devicedata.Id;
            }
            return 0;
        }

        public long DeleteDeviceId(DeviceMasterViewModel model)
        {
            var DeviceData = _deviceMasterRepository.Table.FirstOrDefault(i => i.DeviceId == model.DeviceId && i.UserId == model.UserId && !i.IsDeleted);
            if (DeviceData != null)
            {
                //var currentIpAddress = new IpMaster
                //{
                //    Id = IpAddress.Id,
                //    UserId = IpAddress.UserId,
                //    //IpAddress = IpAddress.IpAddress,
                //    //IsEnable = IpAddress.IsEnable,
                //    IsDeleted = true,
                //    UpdatedDate = DateTime.UtcNow,
                //    UpdatedBy = IpAddress.UserId
                //};
                DeviceData.SetAsIpDeletetatus();
                _deviceMasterRepository.Update(DeviceData);
                //_dbContext.SaveChanges();
                return DeviceData.Id;
            }
            return 0;

        }
    }
}
