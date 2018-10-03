using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class TempUserRegisterService: ITempUserRegisterService
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly ILogger<TempUserRegisterService> _log;
        private readonly IUserService _userService;
        private readonly ITempOtpService _tempOtpService;
        public TempUserRegisterService(CleanArchitectureContext dbContext, ILogger<TempUserRegisterService> log, IUserService userService, ITempOtpService tempOtpService)
        {
            _dbContext = dbContext;
            _log = log;
            _userService = userService;
            _tempOtpService = tempOtpService;
        }

        public bool GetMobileNumber(string MobileNumber)
        {
            var userdata = _dbContext.TempUserRegister.Where(i => i.Mobile == MobileNumber).FirstOrDefault();
            if (userdata?.Mobile == MobileNumber)
                return false;
            else
                return true;
        }

        public async Task<TempUserRegister> AddTempRegister(SignUpWithMobileViewModel model)
        {
            //using (CustomerEntities entities = new CustomerEntities())
            //{
            //    entities.Customers.Add(customer);
            //    entities.SaveChanges();
            //    int id = customer.CustomerId;
            //}

            var currentTempReguser = new TempUserRegister
            {
                RegTypeId = 1,
                Mobile = model.Mobile,                 
                UserName = model.Mobile
            };
            _dbContext.Add(currentTempReguser);
            _dbContext.SaveChanges();

            //// int id = (int)currentTempReguser.Id;
            //var currentTempotp = new TempOtpMaster
            //{
            //    UserId = (int)currentTempReguser.Id,
            //    RegTypeId = 1,
            //    OTP = _userService.GenerateRandomOTP().ToString()
            //};
            var obj= _tempOtpService.AddTempOtp((int)currentTempReguser.Id,1);         

            return currentTempReguser;
          

            
        }
    }
}
