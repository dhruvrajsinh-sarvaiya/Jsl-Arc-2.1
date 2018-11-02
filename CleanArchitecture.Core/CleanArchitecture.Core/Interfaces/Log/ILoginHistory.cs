using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Log
{
   public interface ILoginHistory
    {
         long LoginHistory(LoginhistoryViewModel model);
        List<LoginhistoryViewModel> GetLoginHistoryByUserId(long UserId, int pageIndex, int pageSize);
    }
}
