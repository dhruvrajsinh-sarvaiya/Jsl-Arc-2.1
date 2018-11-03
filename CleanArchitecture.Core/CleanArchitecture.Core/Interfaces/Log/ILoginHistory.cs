using CleanArchitecture.Core.ViewModels.AccountViewModels.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Log
{
   public interface ILoginHistory
    {
         long AddLoginHistory(LoginhistoryViewModel model);
        List<LoginHistoryDataViewModel> GetLoginHistoryByUserId(long UserId, int pageIndex, int pageSize);
    }
}
