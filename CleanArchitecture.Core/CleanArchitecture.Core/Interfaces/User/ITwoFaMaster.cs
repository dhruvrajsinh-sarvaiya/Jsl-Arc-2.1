using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.User
{
    public partial interface ITwoFaMaster
    {
        Task<TwoFaMasterViewModel> AddOtpAsync(int UserId, string Email = null, string Mobile = null);
        Task<TwoFaMasterViewModel> GetOtpData(int Id);
        void UpdateOtp(long Id);
    }
}
