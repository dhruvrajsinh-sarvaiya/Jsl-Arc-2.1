using CleanArchitecture.Core.ViewModels.AccountViewModels.UserKey;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.User
{
    public interface IUserKeyMasterService
    {

        string Get2FACustomToken(long UserId);
        void UpdateOtp(long Id);
        List<UserKeyViewModel> GetUserUniqueKeyList(long userid);
        UserKeyViewModel GetUserUniqueKey(string useruniqueKey);
        UserKeyViewModel AddUniqueKey(UserKeyViewModel model);
    }
}
