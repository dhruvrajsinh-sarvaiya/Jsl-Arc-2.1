using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;

namespace CleanArchitecture.Core.Interfaces.Log
{
    public interface IDeviceIdService
    {
        //Task<long> AddDeviceId(DeviceMasterViewModel model);
        long AddDeviceId(DeviceMasterViewModel model);
        List<DeviceMasterViewModel> GetDeviceListByUserId(long UserId);
        void UpdateDeviceId(DeviceMasterViewModel model);
        long DesableDeviceId(DeviceMasterViewModel model);
        long DeleteDeviceId(DeviceMasterViewModel model);
    }
}
