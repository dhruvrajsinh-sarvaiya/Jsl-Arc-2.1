using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class TempUserRegisterService : ITempUserRegisterService
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly ILogger<TempUserRegisterService> _log;
        private readonly IUserService _userService;
        private readonly ITempOtpService _tempOtpService;
        //private readonly IRepository<TempUserRegister> _repository;
        private readonly IMessageRepository<TempUserRegister> _temptodoRepository;

        public TempUserRegisterService(CleanArchitectureContext dbContext, ILogger<TempUserRegisterService> log, IUserService userService, ITempOtpService tempOtpService, IMessageRepository<TempUserRegister> temptodoRepository)
        {
            _dbContext = dbContext;
            _log = log;
            _userService = userService;
            _tempOtpService = tempOtpService;
            _temptodoRepository = temptodoRepository;
        }

        public bool GetMobileNumber(string MobileNumber)
        {
            var userdata = _dbContext.TempUserRegister.Where(i => i.Mobile == MobileNumber).FirstOrDefault();
            if (userdata?.Mobile == MobileNumber)
                return false;
            else
                return true;
        }

        public async Task<TempUserRegisterViewModel> AddTempRegister(TempUserRegisterViewModel model)
        {
            var currentTempReguser = new TempUserRegister
            {
                RegTypeId = model.RegTypeId,
                Mobile = model.Mobile,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = model.PasswordHash,
                Email = model.Email,
                CreatedDate = DateTime.UtcNow,
            };
            _dbContext.Add(currentTempReguser);
            _dbContext.SaveChanges();
            if (currentTempReguser.RegTypeId == Convert.ToInt16(Core.Enums.enRegisterType.Mobile))
            {
                var obj = await _tempOtpService.AddTempOtp((int)currentTempReguser.Id, currentTempReguser.RegTypeId);
                TempUserRegisterViewModel temp = new TempUserRegisterViewModel();
                temp.Id = currentTempReguser.Id;
                temp.RegTypeId = currentTempReguser.RegTypeId;
                temp.UserName = currentTempReguser.UserName;
                temp.Email = currentTempReguser.Email;
                temp.RegisterStatus = currentTempReguser.RegisterStatus;

                return temp;
            }
            else
            {
                return null;
            }
        }

        public async Task<TempUserRegisterViewModel> FindById(long Id)
        {
            var userdata = _dbContext.TempUserRegister.Find(Id);
            TempUserRegisterViewModel model = new TempUserRegisterViewModel();
            if (userdata != null)
            {
                model.UserName = userdata.UserName;
                model.FirstName = userdata.FirstName;
                model.LastName = userdata.LastName;
                model.Mobile = userdata.Mobile;
                model.PasswordHash = userdata.PasswordHash;
                model.RegisterStatus = userdata.RegisterStatus;
                model.Email = userdata.Email;
                model.Id = userdata.Id;
                return model;
            }
            else
                return null;
        }

        public void Update(long Id)
        {
            try
            {
                var tempdata = _temptodoRepository.GetById(Convert.ToInt16(Id));
                tempdata.SetAsStatus();
                //tempdata.RegisterStatus = true;
                _temptodoRepository.Update(tempdata);
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }
        }

        public async Task<bool> GetEmail(string Email)
        {
            var userdata = _dbContext.TempUserRegister.Where(i => i.Email == Email).FirstOrDefault();
            if (userdata?.Email == Email)
                return false;
            else
                return true;
        }

        public async Task<TempUserRegisterViewModel> GetMobileNo(string MobileNo)
        {
            var userdata = _dbContext.TempUserRegister.Where(i => i.Mobile == MobileNo).FirstOrDefault();
            TempUserRegisterViewModel model = new TempUserRegisterViewModel();
            if (userdata != null)
            {
                model.UserName = userdata.UserName;
                model.FirstName = userdata.FirstName;
                model.LastName = userdata.LastName;
                model.Mobile = userdata.Mobile;
                model.PasswordHash = userdata.PasswordHash;
                model.RegisterStatus = userdata.RegisterStatus;
                model.Email = userdata.Email;
                model.Id = userdata.Id;
                return model;
            }
            else
            {
                return null;
            }
        }
    }
}
