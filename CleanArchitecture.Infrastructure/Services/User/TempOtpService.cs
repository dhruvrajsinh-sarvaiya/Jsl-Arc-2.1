using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces.User;

namespace CleanArchitecture.Infrastructure.Services.User
{
    public class TempOtpService : ITempOtpService
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly IUserService _userService;

        public TempOtpService(CleanArchitectureContext dbContext,  IUserService userService)
        {
            _dbContext = dbContext;          
            _userService = userService;
        }

        public async Task<TempOtpMaster> AddTempOtp(int UserId,int RegTypeId)
        {
            var currentTempotp = new TempOtpMaster
            {
                UserId = UserId,
                RegTypeId = RegTypeId,
                OTP = _userService.GenerateRandomOTP().ToString(),
                CreatedTime = DateTime.UtcNow,
                ExpirTime = DateTime.UtcNow.AddHours(2),
                EnableStatus = false,
                CreatedDate = DateTime.Now,
                CreatedBy = UserId

            };
            _dbContext.Add(currentTempotp);
            _dbContext.SaveChanges();

            return currentTempotp;
        }
    }
}
