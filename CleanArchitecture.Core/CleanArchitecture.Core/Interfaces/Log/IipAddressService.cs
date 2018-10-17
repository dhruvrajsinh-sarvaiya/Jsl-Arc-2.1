using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;

namespace CleanArchitecture.Core.Interfaces.Log
{
    public interface IipAddressService
    {
        Task<long> AddIpAddress(IpMasterViewModel model);
        Task<IpMasterViewModel> GetIpAddressById(long Id);
        Task<List<IpMasterViewModel>> GetIpAddressListByUserId(long UserId);
        void UpdateIpAddress(IpMasterViewModel model);
        Task<long> DesableIpAddress(IpMasterViewModel model);
        Task<long> DeleteIpAddress(IpMasterViewModel model);



    }
}
