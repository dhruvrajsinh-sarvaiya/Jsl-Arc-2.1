using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class TempOtpService : ITempOtpService
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly IUserService _userService;
        private readonly IMessageRepository<TempOtpMaster> _temptodoRepository;
        public TempOtpService(CleanArchitectureContext dbContext, IUserService userService, IMessageRepository<TempOtpMaster> temptodoRepository)
        {
            _dbContext = dbContext;
            _userService = userService;
            _temptodoRepository = temptodoRepository;
        }

        public async Task<TempOtpMaster> AddTempOtp(int UserId, int RegTypeId)
        {
            var currentTempotp = new TempOtpMaster
            {
                UserId = UserId,
                RegTypeId = RegTypeId,
                OTP = _userService.GenerateRandomOTP().ToString(),
                CreatedTime = DateTime.UtcNow,
                ExpirTime = DateTime.UtcNow.AddHours(2),
                Status = 0,
                CreatedDate = DateTime.Now,
                CreatedBy = UserId

            };
            _dbContext.Add(currentTempotp);
            _dbContext.SaveChanges();

            return currentTempotp;
        }



        public async Task<TempOtpViewModel> GetTempData(int Id)
        {
            var tempotp = _dbContext.TempOtpMaster.Where(i => i.UserId == Id).FirstOrDefault();
            TempOtpViewModel model = new TempOtpViewModel();
            if (tempotp != null)
            {
                model.UserId = tempotp.UserId;
                model.RegTypeId = tempotp.RegTypeId;
                model.OTP = tempotp.OTP;
                model.CreatedTime = tempotp.CreatedTime;
                model.ExpirTime = tempotp.ExpirTime;
                model.Status = tempotp.Status;
                model.Id = tempotp.Id;
                return model;
            }
            else
                return null;
        }

        public void Update(long Id)
        {
            var tempdata = _temptodoRepository.GetById(Convert.ToInt16(Id));
            tempdata.SetAsOTPStatus();
            //tempdata.Status = true;
            _temptodoRepository.Update(tempdata);
        }
    }
}
