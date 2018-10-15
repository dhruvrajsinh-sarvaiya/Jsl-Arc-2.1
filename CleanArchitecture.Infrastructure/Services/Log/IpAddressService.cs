﻿using System;
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
            var getIp = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && !i.IsDeleted);
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

            };
            _ipMasterRepository.Insert(currentIpAddress);
            //_dbContext.SaveChanges();

            return currentIpAddress.Id;
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

        public async Task<List<IpMasterViewModel>> GetIpAddressListByUserId(long UserId)
        {
            var IpAddressList = _ipMasterRepository.Table.Where(i => i.UserId == UserId && !i.IsDeleted).ToList();
            if (IpAddressList == null)
            {
                return null;
            }

            var IpList = new List<IpMasterViewModel>();
            foreach (var item in IpAddressList)
            {
                IpMasterViewModel imodel = new IpMasterViewModel();
                imodel.Id = item.Id;
                imodel.UserId = item.UserId;
                imodel.IpAddress = item.IpAddress;
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


        public void UpdateIpAddress(IpMasterViewModel model)
        {
            var IpAddress = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && !i.IsDeleted);
            if (IpAddress != null)
            {
                var currentIpAddress = new IpMaster
                {
                    Id = IpAddress.Id,
                    UserId = IpAddress.UserId,
                    IpAddress = model.IpAddress,
                    IsEnable = IpAddress.IsEnable,
                    //IsDeleted = IpAddress.IsDeleted,                    
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = IpAddress.UserId
                };

                _ipMasterRepository.Update(currentIpAddress);
                //_dbContext.SaveChanges();

            }
        }

        public void DeleteIpAddress(IpMasterViewModel model)
        {
            var IpAddress = _ipMasterRepository.Table.FirstOrDefault(i => i.IpAddress == model.IpAddress && !i.IsDeleted);
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

            }

        }


    }
}
