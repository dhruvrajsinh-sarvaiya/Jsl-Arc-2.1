using CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.User
{
    public partial interface IUserConfiguration
    {
       void  Add(int UserId, string Type, string ConfigurationValue, bool EnableStatus);

        Task <UserConfigurationMasterViewModel> Get(int Id);

        void update(int Id);


    }
}
