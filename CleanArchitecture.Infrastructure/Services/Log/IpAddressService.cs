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

    public class IpAddressService : IipAddressService
    {
        private readonly ICustomRepository<IpMaster> _ipMasterRepository;

        public IpAddressService(ICustomRepository<IpMaster> ipMasterRepository)
        {
            _ipMasterRepository = ipMasterRepository;
        }

        public async Task<long> AddIpAddress(IpMasterViewModel model)
        {
            var getIp = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && i.UserId == model.UserId && !i.IsDeleted);
            if (getIp != null)
            {
                return getIp.Id;
            }

            var currentIpAddress = new IpMaster
            {
                UserId = model.UserId,
                IpAddress = model.IpAddress,
                IsEnable = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = model.UserId,
                Status = 0,
                IpAliasName = model.IpAliasName

            };
            _ipMasterRepository.Insert(currentIpAddress);
            //_dbContext.SaveChanges();

            return currentIpAddress.Id;
        }

        public async Task<long> GetIpAddressByUserIdandAddress(string IpAddress, long UserId)
        {
            var getIp = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == IpAddress && i.UserId == UserId && !i.IsDeleted);
            if (getIp != null)
            {
                return getIp.Id;
            }

            return 0;
        }

        public async Task<IpMasterViewModel> GetIpAddressById(long Id)
        {
            var IpAddress = _ipMasterRepository.GetById(Id);
            if (IpAddress == null)
            {
                return null;
            }

            var currentIpAddress = new IpMasterViewModel
            {
                Id = IpAddress.Id,
                UserId = IpAddress.UserId,
                IpAddress = IpAddress.IpAddress,
                IsEnable = IpAddress.IsEnable,
                IsDeleted = IpAddress.IsDeleted,
                CreatedDate = IpAddress.CreatedDate,
                CreatedBy = IpAddress.CreatedBy,
                UpdatedDate = IpAddress.UpdatedDate,
                UpdatedBy = IpAddress.UpdatedBy,
                Status = 0,

            };

            return currentIpAddress;

        }

        public async Task<List<IpMasterGetViewModel>> GetIpAddressListByUserId(long UserId, int pageIndex, int pageSize)
        {
            

            var IpAddressList = _ipMasterRepository.Table.Where(i => i.UserId == UserId && !i.IsDeleted).ToList();
            if (IpAddressList == null)
            {
                return null;
            }

            var IpList = new List<IpMasterGetViewModel>();
            foreach (var item in IpAddressList)
            {
                IpMasterGetViewModel imodel = new IpMasterGetViewModel();
               // imodel.Id = item.Id;
               // imodel.UserId = item.UserId;
                imodel.IpAddress = item.IpAddress;
                imodel.IsEnable = item.IsEnable;
                //imodel.IsDeleted = item.IsDeleted;
                imodel.CreatedDate = item.CreatedDate;
              //  imodel.CreatedBy = item.CreatedBy;
                //imodel.UpdatedDate = item.UpdatedDate;
               // imodel.UpdatedBy = item.UpdatedBy;
                imodel.Status = item.Status;
                imodel.IpAliasName = item.IpAliasName;

                IpList.Add(imodel);
            }
            //return IpList;


            var total = IpList.Count();
            //var pageSize = 10; // set your page size, which is number of records per page

            //var page = 1; // set current page number, must be >= 1
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            if (pageSize == 0)
            {
                pageSize = 10;
            }

            var skip = pageSize * (pageIndex - 1);

            //var canPage = skip < total;

            //if (canPage) // do what you wish if you can page no further
            //    return null;

            return IpList.Skip(skip).Take(pageSize).ToList();

        }


        public async Task<long> UpdateIpAddress(IpMasterViewModel model)
        {
            var IpAddress = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && i.UserId == model.UserId && !i.IsDeleted);
            if (IpAddress != null)
            {
                IpAddress.IpAddress = model.IpAddress;
                IpAddress.IpAliasName = model.IpAliasName;
                IpAddress.UpdatedBy = IpAddress.UserId;
                IpAddress.UpdatedDate = DateTime.UtcNow;


                _ipMasterRepository.Update(IpAddress);
                return IpAddress.Id;
                //_dbContext.SaveChanges();

            }
            else
                return 0;


        }

        public async Task<long> DesableIpAddress(IpMasterViewModel model)
        {
            var IpAddress = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && i.UserId == model.UserId && !i.IsDeleted);
            if (IpAddress != null)
            {              // Status False
                IpAddress.SetAsIsDisabletatus();
                _ipMasterRepository.Update(IpAddress);
                //_dbContext.SaveChanges();
                return IpAddress.Id;
            }
            return 0;
        }

        public async Task<long> EnableIpAddress(IpMasterViewModel model)
        {
            var IpAddress = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && i.UserId == model.UserId && !i.IsEnable && !i.IsDeleted);
            if (IpAddress != null)
            {
                //Status True
                IpAddress.SetAsIsEnabletatus();
                _ipMasterRepository.Update(IpAddress);
                //_dbContext.SaveChanges();
                return IpAddress.Id;
            }
            return 0;
        }

        public async Task<long> DeleteIpAddress(IpMasterViewModel model)
        {
            var IpAddress = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && i.UserId == model.UserId && !i.IsDeleted);
            if (IpAddress != null)
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
                IpAddress.SetAsIpDeletetatus();
                _ipMasterRepository.Update(IpAddress);
                //_dbContext.SaveChanges();
                return IpAddress.Id;
            }
            return 0;

        }


    }
}
