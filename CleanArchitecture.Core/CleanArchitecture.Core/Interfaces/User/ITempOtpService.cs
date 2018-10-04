using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;

namespace CleanArchitecture.Core.Interfaces.User
{
   public interface ITempOtpService
    {
        Task<TempOtpViewModel> AddTempOtp(int UserId, int RegTypeId);
        Task<TempOtpViewModel> GetTempData(int Id);
        void Update(long Id);
    }
}
