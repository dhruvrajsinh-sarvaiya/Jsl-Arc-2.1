using CleanArchitecture.Core.ViewModels.ManageViewModels.UserChangeLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.UserChangeLog
{
   public  interface IUserChangeLog
    {
        long  AddPassword(UserChangeLogViewModel model);
    }
}
