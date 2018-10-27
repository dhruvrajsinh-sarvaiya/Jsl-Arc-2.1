using CleanArchitecture.Core.ViewModels.AccountViewModels.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.User
{
    public interface ICustomPassword
    {
        Task<CustomtokenViewModel> AddPassword(CustomtokenViewModel model);
        Task<CustomtokenViewModel> GetPassword(long userid);
        Task<CustomtokenViewModel> IsValidPassword(string appkey, string otp);
        void UpdateOtp(long Id);
        Task<string> Get2FACustomToken(long UserId);
    }
}
