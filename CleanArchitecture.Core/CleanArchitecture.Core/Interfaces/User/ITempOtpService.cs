using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;

namespace CleanArchitecture.Core.Interfaces.User
{
   public interface ITempOtpService
    {
        Task<TempOtpMaster> AddTempOtp(int UserId, int RegTypeId);
    }
}
