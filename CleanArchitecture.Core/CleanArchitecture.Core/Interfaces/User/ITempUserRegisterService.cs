using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;

namespace CleanArchitecture.Core.Interfaces.User
{
    public interface ITempUserRegisterService
    {
        bool GetMobileNumber(string MobileNumber);
        Task<TempUserRegister> AddTempRegister(SignUpWithMobileViewModel model);
    }
}
